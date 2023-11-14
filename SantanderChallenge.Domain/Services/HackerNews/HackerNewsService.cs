using SantanderChallenge.Domain.Services.HackerNews.Models;
using SantanderChallenge.Domain.Services.HackerNews.Transport;

namespace SantanderChallenge.Domain.Services.HackerNews;

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsApiClient _hackerNewsApiClient;

    public HackerNewsService(IHackerNewsApiClient hackerNewsApiClient)
    {
        _hackerNewsApiClient = hackerNewsApiClient;
    }

    public async Task<IEnumerable<HackerNewsStory>> GetTopStoriesAsync(int count)
    {
        var ids = await _hackerNewsApiClient.GetTopStoryIdsAsync(count);
        var fetchTasks = ids.Select(id => _hackerNewsApiClient.GetStoryByIdAsync(id));
        var result = await Task.WhenAll(fetchTasks);
        return result;
    }
}