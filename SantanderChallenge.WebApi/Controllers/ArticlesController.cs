using Microsoft.AspNetCore.Mvc;
using SantanderChallenge.Domain.Services.HackerNews;
using SantanderChallenge.WebApi.Converters;

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
    public async Task<IActionResult> GetAsync(int count)
    {
        _logger.LogInformation($"Requesting top {count} articles from top-stories endpoint");

        try
        {
            var response = (await _hackerNewsService.GetTopStoriesAsync(count))
                .Take(count)
                .Select(HackerNewsArticleToResponseConverter.ToResponseModel);

            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching top articles.");

            return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred");
        }
    }
}