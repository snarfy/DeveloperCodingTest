using SantanderChallenge.Domain.Services.HackerNews.Models;

namespace SantanderChallenge.Domain.Services.HackerNews;

public interface IHackerNewsService
{
    Task<IEnumerable<HackerNewsStory>> GetTopStories(int count);
}