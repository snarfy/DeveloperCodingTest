using SantanderChallenge.Domain.Services.HackerNews.Models;

namespace SantanderChallenge.Domain.Services.HackerNews;

public class HackerNewsService : IHackerNewsService
{
    public IEnumerable<HackerNewsStory> GetTopStories(int count)
    {
        for (var i = 0; i < count; i++)
            yield return HackerNewsStory.Stub;
    }
}