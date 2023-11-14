using SantanderChallenge.Domain.Services.HackerNews.Models;
using SantanderChallenge.Domain.Services.HackerNews.Transport;

namespace SantanderChallenge.Domain.Services.HackerNews;

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsProvider _hackerNewsProvider;

    public HackerNewsService(IHackerNewsProvider hackerNewsProvider)
    {
        _hackerNewsProvider = hackerNewsProvider;
    }

    public async Task<IEnumerable<HackerNewsStory>> GetTopStoriesAsync(int count)
    {
        var ids = await _hackerNewsProvider.GetTopStoryIdsAsync(count);
        var fetchTasks = ids.Select(id => _hackerNewsProvider.GetStoryByIdAsync(id));
        var result = await Task.WhenAll(fetchTasks);
        return result;
    }
}