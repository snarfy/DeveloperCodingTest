using SantanderChallenge.Domain.Services.HackerNews.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Transport;

public class HackerNewsProvider : IHackerNewsProvider
{
    private readonly IHackerNewsApiClient _client;

    private readonly object _getIdsLock = new(); // let's limit getIds to one call only (only one is needed)

    public HackerNewsProvider(IHackerNewsApiClient client)
    {
        _client = client;
    }

    public Task<IEnumerable<int>> GetTopStoryIdsAsync(int count)
    {
        lock (_getIdsLock)
        {
            var enumerable = new List<int> { 1, 2, 3, 4, 5 }.AsEnumerable();
            return Task.FromResult(enumerable);
        }
    }

    public Task<HackerNewsStory> GetStoryByIdAsync(int id)
    {
        return Task.FromResult(HackerNewsStory.Stub);
    }
}