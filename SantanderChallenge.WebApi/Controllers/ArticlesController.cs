using Microsoft.AspNetCore.Mvc;
using SantanderChallenge.WebApi.Converters;
using SantanderChallenge.WebApi.ResponseModels;
using SantanderChallenge.WebApi.Services.HackerNews;

namespace SantanderChallenge.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IHackerNewsService _hackerNewsService;
    private readonly ILogger<ArticlesController> _logger;

    public ArticlesController(
        ILogger<ArticlesController> logger,
        IHackerNewsService hackerNewsService)
    {
        _logger = logger;
        _hackerNewsService = hackerNewsService;
    }

    [HttpGet]
    [Route("top-stories/{count:int}")]
    public IEnumerable<HackerNewsArticleResponse> Get(int count)
    {
        _logger.LogInformation($"Requesting top {count} HackerNews articles");
        return _hackerNewsService.GetTopStories(count)
            .Select(HackerNewsArticleToResponseConverter.ToResponseModel);
    }
}