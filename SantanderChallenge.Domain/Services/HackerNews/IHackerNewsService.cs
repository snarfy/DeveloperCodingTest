using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews;

public interface IHackerNewsService
{
    Task<IEnumerable<HackerNewsStory>> GetTopStoriesAsync(int count);
}