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
    [Route("top-stories")]
    public string Get()
    {
        return "Coming soon...";
    }
}