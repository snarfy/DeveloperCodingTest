using Microsoft.AspNetCore.Mvc;

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
    public string Get(int count)
    {
        return $"fetching {count}";
    }
}