using SantanderChallenge.Domain.Services.HackerNews.Client;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews;

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsApi _hackerNewsApi;

    public HackerNewsService(IHackerNewsApi hackerNewsApi)
    {
        _hackerNewsApi = hackerNewsApi;
    }

    public async Task<IEnumerable<HackerNewsStory>> GetTopStoriesAsync(int count)
    {
        var ids = await _hackerNewsApi.GetTopStoryIdsAsync();
        var fetchTasks = ids.Take(count)
            .Select(id => _hackerNewsApi.GetStoryByIdAsync(id));
        var result = await Task.WhenAll(fetchTasks);
        return result;
    }
}