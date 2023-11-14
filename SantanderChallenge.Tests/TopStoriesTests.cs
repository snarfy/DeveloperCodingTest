using Microsoft.AspNetCore.Mvc.Testing;
using SantanderChallenge.WebApi;
using Shouldly;
using Xunit;

namespace SantanderChallenge.Tests;

public class TopStoriesTests
{
    [Fact]
    public async void Can_retrieve_coming_soon_text()
    {
        var application = new WebApplicationFactory<Program>();

        var client = application.CreateClient();

        var response = await client.GetStringAsync("/articles/top-stories");

        response.ShouldBe("Coming soon...");
    }
}