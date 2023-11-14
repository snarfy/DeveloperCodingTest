using SantanderChallenge.Domain.Services.HackerNews.Models;

namespace SantanderChallenge.Domain.Services.HackerNews;

public class HackerNewsService : IHackerNewsService
{
    public async Task<IEnumerable<HackerNewsStory>> GetTopStories(int count)
    {
        return Enumerable.Repeat(HackerNewsStory.Stub, count);
    }
}