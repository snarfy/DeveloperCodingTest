using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SantanderChallenge.WebApi;
using SantanderChallenge.WebApi.ResponseModels;
using SantanderChallenge.WebApi.Services;
using Shouldly;
using Xunit;

namespace SantanderChallenge.Tests;

public class TopStoriesTests
{
    [Fact]
    public async void Can_retrieve_the_correct_amount_requested()
    {
        var mockHackerNewsProvider = A.Fake<IHackerNewsProvider>();
        A.CallTo(() => mockHackerNewsProvider.GetRecords())
            .Returns(new List<HackerNewsArticleResponse>
                { new(), new(), new(), new(), new() });

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { services.AddSingleton(mockHackerNewsProvider); });
            });

        var client = application.CreateClient();

        var response = await client.GetStringAsync("/articles/top-stories/5");
        var testObject = JsonConvert.DeserializeObject<List<HackerNewsArticleResponse>>(response);

        testObject.Count.ShouldBe(5);
    }
}