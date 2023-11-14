using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Converters;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer;

/// <summary>
///     HackerNews Client that actually makes the HackerNews Api calls
/// </summary>
public class HackerNewsApiClient : IHackerNewsApi
{
    private const string _apiUrlForFetchingArticlesAccordingToRank =
        "https://hacker-news.firebaseio.com/v0/beststories.json";

    private const string _apiUrlForFetchingArticle = "https://hacker-news.firebaseio.com/v0/item/";

    //this HttpClient could also be stubbed out, but for the purpose of this demo, I'll keep it simple
    private readonly HttpClient _client;
    private readonly ILogger<HackerNewsApiClient> _logger;

    public HackerNewsApiClient(HttpClient client, ILogger<HackerNewsApiClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IEnumerable<int>> GetTopStoryIdsAsync()
    {
        _logger.LogInformation("[Outgoing HTTP Request] Fetching all top-article-ids from HackerNews Api");
        var response = await _client.GetAsync(_apiUrlForFetchingArticlesAccordingToRank);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<int>>(responseContent);
        }

        throw new Exception("Non successful response code from API");
    }

    public async Task<HackerNewsStory> GetStoryByIdAsync(int id)
    {
        _logger.LogInformation($"[Outgoing HTTP Request] Fetching articleById({id}) from HackerNews Api");
        var response = await _client.GetAsync($"{_apiUrlForFetchingArticle}{id}.json");

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var hackerNewsStoryFromApi = JsonConvert.DeserializeObject<ExternalHackerNewsStoryResult>(responseContent);

            return HackerNewsStoryFromApiToDomainConverter.Convert(hackerNewsStoryFromApi);
        }

        throw new Exception("Non successful response code from API");
    }
}