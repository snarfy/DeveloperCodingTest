using Microsoft.AspNetCore.Mvc;
using SantanderChallenge.WebApi.ResponseModels;

namespace SantanderChallenge.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ILogger<ArticlesController> _logger;

    public ArticlesController(ILogger<ArticlesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("top-stories/{count:int}")]
    public IEnumerable<HackerNewsArticleResponse> Get(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new HackerNewsArticleResponse();
    }
}