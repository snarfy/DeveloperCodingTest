using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer;

/// <summary>
///     HackerNews Client that actually makes the HackerNews Api calls
/// </summary>
public class HackerNewsApiClient : IHackerNewsApi
{
    private const string _apiUrlForFetchingArticlesAccordingToRank = "https://hacker-news.firebaseio.com/v0/beststories.json";
    private const string _apiUrlForFetchingArticle = "https://hacker-news.firebaseio.com/v0/item/";

    // Dependencies
    private readonly HttpClient
        _client; // This HttpClient could also be stubbed out given more time, but for the purpose of this demo, I'll keep it simple

    private readonly ILogger<HackerNewsApiClient> _logger;
    private readonly IMapper _mapper;

    public HackerNewsApiClient(HttpClient client, ILogger<HackerNewsApiClient> logger, IMapper mapper)
    {
        _client = client;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<int>> GetTopStoryIdsAsync()
    {
        _logger?.LogInformation("[Outgoing HTTP Request] Fetching all top-article-ids from HackerNews Api");
        var response = await _client.GetAsync(_apiUrlForFetchingArticlesAccordingToRank);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<int>>(responseContent)
                   ?? Enumerable.Empty<int>();
        }

        throw new Exception("Non successful response code from API");
    }

    public async Task<HackerNewsStory> GetStoryByIdAsync(int id)
    {
        _logger?.LogInformation($"[Outgoing HTTP Request] Fetching articleById({id}) from HackerNews Api");
        var response = await _client.GetAsync($"{_apiUrlForFetchingArticle}{id}.json");

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var hackerNewsStoryFromApi = JsonConvert.DeserializeObject<ExternalHackerNewsStoryResult>(responseContent);

            return _mapper.Map<HackerNewsStory>(hackerNewsStoryFromApi);
        }

        throw new Exception("Non successful response code from API");
    }
}