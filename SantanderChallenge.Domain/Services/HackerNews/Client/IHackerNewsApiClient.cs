using SantanderChallenge.Domain.Services.HackerNews.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Transport;

public interface IHackerNewsApiClient
{
    Task<IEnumerable<int>> GetTopStoryIdsAsync(int count);
    Task<HackerNewsStory> GetStoryByIdAsync(int id);
}

