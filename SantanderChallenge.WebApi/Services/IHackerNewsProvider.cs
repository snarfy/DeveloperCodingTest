using SantanderChallenge.WebApi.ResponseModels;

namespace SantanderChallenge.WebApi.Services;

public interface IHackerNewsProvider
{
    IEnumerable<HackerNewsArticleResponse> GetRecords();
}