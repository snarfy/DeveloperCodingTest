using SantanderChallenge.Domain.Services.HackerNews.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Transport;

public class HackerNewsApiClient : IHackerNewsApiClient
{
    public Task<IEnumerable<int>> GetTopStoryIdsAsync(int count)
    {
        var enumerable = new List<int> { 1, 2, 3, 4, 5 }.AsEnumerable();
        return Task.FromResult(enumerable);
    }

    public Task<HackerNewsStory> GetStoryByIdAsync(int id)
    {
        return Task.FromResult(HackerNewsStory.Stub);
    }
}