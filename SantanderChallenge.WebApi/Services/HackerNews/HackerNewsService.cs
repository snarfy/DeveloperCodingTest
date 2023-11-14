using SantanderChallenge.WebApi.Services.HackerNews.Models;

namespace SantanderChallenge.WebApi.Services.HackerNews;

public class HackerNewsService : IHackerNewsService
{
    public IEnumerable<HackerNewsStory> GetTopStories(int count)
    {
        for (var i = 0; i < count; i++) yield return HackerNewsStory.Stub;
    }
}