using SantanderChallenge.WebApi.Services.HackerNews.Models;

namespace SantanderChallenge.WebApi.Services.HackerNews;

public interface IHackerNewsService
{
    IEnumerable<HackerNewsStory> GetTopStories(int count);
}