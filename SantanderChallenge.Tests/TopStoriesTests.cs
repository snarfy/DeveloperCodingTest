using Microsoft.AspNetCore.Mvc.Testing;
using SantanderChallenge.WebApi;
using Shouldly;
using Xunit;

namespace SantanderChallenge.Tests;

public class TopStoriesTests
{
    [Fact]
    public async void Can_pass_on_the_amount_requested()
    {
        var application = new WebApplicationFactory<Program>();

        var client = application.CreateClient();

        (await client.GetStringAsync("/articles/top-stories/1"))
            .ShouldBe("fetching 1");

        (await client.GetStringAsync("/articles/top-stories/5"))
            .ShouldBe("fetching 5");
    }
}