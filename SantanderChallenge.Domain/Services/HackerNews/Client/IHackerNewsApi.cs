using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Client;

public interface IHackerNewsApi
{
    Task<IEnumerable<int>> GetTopStoryIdsAsync();
    Task<HackerNewsStory> GetStoryByIdAsync(int id);
}