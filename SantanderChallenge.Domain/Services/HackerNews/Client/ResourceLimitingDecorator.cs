using Microsoft.Extensions.Logging;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Client;

/// <summary>
///     Decorator that limits the api calls to one per article or aggregated list
///     as well as supplying caching to the app (could eventually be moved to another decorator - no time now!)
/// </summary>
public class ResourceLimitingDecorator : IHackerNewsApi
{
    //decorated interface
    private readonly IHackerNewsApi _decorated;

    //synchronization methods (monitor/semaphores etc)
    private readonly Semaphore _getArticlesSemaphore; // let's limit getArticleById sparingly to only a few calls

    private readonly object
        _getByIdLock =
            new(); // we need to funnel all requests through one lock, to ensure only one download a new record at a time

    private readonly Dictionary<int, object> _getByIdLockArray = new(); // we lock per getbyid requests
    private readonly object _getByIdLockArrayGateKeeper = new(); // a gatekeeper for the array object
    private readonly object _getIdsLock = new(); // let's limit getIds to one call only (only one is needed)

    //dependencies
    private readonly ILogger<ResourceLimitingDecorator> _logger;
    private readonly int _refreshArticleOrderSeconds;

    //caching (*NOTE: caching could be split into another decorator, but for the purpose of this demo, I'll keep it relatively simple
    private readonly Dictionary<int, HackerNewsStory> _storyByIdCache = new();
    private readonly object _storyByIdCacheLock = new(); // a gatekeeper for the _storyByIdCache object
    private DateTime _nextTopStoryIdsRefresh = DateTime.Now.AddSeconds(-1);
    private List<int> _topStoryIdListCache;

    //C'tor
    public ResourceLimitingDecorator(
        IHackerNewsApi decorated,
        ILogger<ResourceLimitingDecorator> logger,
        int maxConcurrentGetArticleCount,
        int refreshArticleOrderSeconds)
    {
        _decorated = decorated;
        _logger = logger;
        _refreshArticleOrderSeconds = refreshArticleOrderSeconds;
        _getArticlesSemaphore = new Semaphore(maxConcurrentGetArticleCount, maxConcurrentGetArticleCount);
    }

    public async Task<IEnumerable<int>> GetTopStoryIdsAsync()
    {
        lock (_getIdsLock)
        {
            var mustRefresh = _topStoryIdListCache == null || _nextTopStoryIdsRefresh < DateTime.Now;
            if (mustRefresh)
            {
                _topStoryIdListCache = _decorated.GetTopStoryIdsAsync()
                    .GetAwaiter().GetResult().ToList(); //since you can't await inside a lock

                _nextTopStoryIdsRefresh = DateTime.Now.AddSeconds(_refreshArticleOrderSeconds);
            }
        }

        return _topStoryIdListCache;
    }

    public Task<HackerNewsStory> GetStoryByIdAsync(int id)
    {
        var lockObjectForSpecificStoryId = GetLockForStoryById(id);

        lock (lockObjectForSpecificStoryId)
        {
            lock (_storyByIdCacheLock)
            {
                if (_storyByIdCache.ContainsKey(id))
                    return Task.FromResult(_storyByIdCache[id]);
            }

            _getArticlesSemaphore.WaitOne();
            var result = _decorated.GetStoryByIdAsync(id).GetAwaiter().GetResult();
            _getArticlesSemaphore.Release();

            lock (_storyByIdCacheLock)
            {
                _storyByIdCache.TryAdd(id, result);
            }

            return Task.FromResult(result);
        }
    }

    //locking helper (one lock per id, for synchronizing fetching articles byId)
    private object GetLockForStoryById(int id)
    {
        object lockObjectForSpecificStoryId;

        lock (_getByIdLockArrayGateKeeper)
        {
            if (!_getByIdLockArray.ContainsKey(id))
            {
                lockObjectForSpecificStoryId = new object();
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