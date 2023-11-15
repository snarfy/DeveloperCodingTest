using Microsoft.Extensions.Logging;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Client;

/// <summary>
///     Decorator that limits the api calls to one per article or aggregated list
///     as well as supplying caching to the app (could eventually be moved to another decorator - no time now!)
/// </summary>
public class ResourceLimitingDecorator : IHackerNewsApi
{
    // Dependencies
    private readonly ILogger<ResourceLimitingDecorator> _logger;
    private readonly IHackerNewsApi _decorated; // Decorated interface

    // Config
    private readonly int _topArticleIdsCacheTtlSecs;


    // Synchronization methods (monitor/semaphores etc)
    private readonly SemaphoreSlim _limitConcurrentCallsSemaphore; // limits concurrent calls to getArticleById Api
    private readonly object _getByIdDictionaryAccessLock = new(); // a lock for accessing the dictionary of locks
    private readonly object _getIdsLock = new(); // let's limit getIds to one call only (only one is needed)
    private readonly Dictionary<int, SemaphoreSlim> _getByIdLockArray = new(); // dictionary of locks, one per getById request


    // Caching Objects (*NOTE: caching could be split into another decorator, but for the purpose of this demo, I'll keep it relatively simple
    private readonly Dictionary<int, HackerNewsStory> _storyByIdCache = new();
    private readonly object _storyByIdCacheAccessLock = new(); // a gatekeeper for the _storyByIdCache object
    private DateTime _nextTopStoryIdsRefresh = DateTime.Now.AddSeconds(-1);
    private List<int> _topStoryIdListCache;

    // C'tor
    public ResourceLimitingDecorator(
        IHackerNewsApi decorated,
        ILogger<ResourceLimitingDecorator> logger,
        int maxConcurrentGetArticleCalls,
        int topArticleIdsCacheTtlSecs)
    {
        _decorated = decorated;
        _logger = logger;
        _topArticleIdsCacheTtlSecs = topArticleIdsCacheTtlSecs;
        _limitConcurrentCallsSemaphore = new SemaphoreSlim(maxConcurrentGetArticleCalls, maxConcurrentGetArticleCalls);
    }

    public async Task<IEnumerable<int>> GetTopStoryIdsAsync()
    {
        lock (_getIdsLock)
        {
            var invalidCache = _topStoryIdListCache == null || _nextTopStoryIdsRefresh < DateTime.Now;

            if (invalidCache)
            {
                _logger?.LogInformation("Storing TopStoryIdList to cache");
                _topStoryIdListCache = _decorated.GetTopStoryIdsAsync()
                    .GetAwaiter().GetResult().ToList(); //since you can't await inside a lock

                _nextTopStoryIdsRefresh = DateTime.Now.AddSeconds(_topArticleIdsCacheTtlSecs);
            }
            else
            {
                _logger?.LogInformation("Fetching TopStoryIdList from cache");
            }
        }

        return _topStoryIdListCache;
    }

    public async Task<HackerNewsStory> GetStoryByIdAsync(int id)
    {
        var asyncLockForSpecificStoryIdAccess = GetLockForStoryById(id);

        asyncLockForSpecificStoryIdAccess.WaitAsync();

        lock (_storyByIdCacheAccessLock)
        {
            if (_storyByIdCache.ContainsKey(id))
            {
                _logger?.LogInformation($"Fetching StoryById({id}) from cache");

                asyncLockForSpecificStoryIdAccess.Release();
                return _storyByIdCache[id];
            }
        }

        _limitConcurrentCallsSemaphore.WaitAsync();
        var result = await _decorated.GetStoryByIdAsync(id);
        _limitConcurrentCallsSemaphore.Release();

        lock (_storyByIdCacheAccessLock)
        {
            _logger?.LogInformation($"Storing StoryById({id}) result to cache");
            _storyByIdCache.Add(id, result);
        }

        asyncLockForSpecificStoryIdAccess.Release();
        return result;
    }

    // Locking helper (one lock per id, for synchronizing fetching & persisting articles byId)
    private SemaphoreSlim GetLockForStoryById(int id)
    {
        SemaphoreSlim lockObjectForSpecificStoryId;

        lock (_getByIdDictionaryAccessLock)
        {
            if (!_getByIdLockArray.ContainsKey(id))
            {
                lockObjectForSpecificStoryId = new SemaphoreSlim(1);
                _getByIdLockArray.Add(id, lockObjectForSpecificStoryId);
            }
            else
            {
                lockObjectForSpecificStoryId = _getByIdLockArray[id];
            }
        }

        return lockObjectForSpecificStoryId;
    }
}