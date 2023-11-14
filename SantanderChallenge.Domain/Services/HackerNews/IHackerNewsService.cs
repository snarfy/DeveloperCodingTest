using SantanderChallenge.Domain.Services.HackerNews.Models;

namespace SantanderChallenge.Domain.Services.HackerNews;

public interface IHackerNewsService
{
    IEnumerable<HackerNewsStory> GetTopStories(int count);
}