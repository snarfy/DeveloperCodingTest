using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SantanderChallenge.WebApi;
using SantanderChallenge.WebApi.ResponseModels;
using SantanderChallenge.WebApi.Services.HackerNews;
using SantanderChallenge.WebApi.Services.HackerNews.Models;
using Shouldly;
using Xunit;

namespace SantanderChallenge.Tests;

public class TopStoriesEndpointTests
{
    [Fact]
    public async void Return_data_from_HackerNewsService_in_correct_format()
    {
        var mockHackerNewsService = A.Fake<IHackerNewsService>();
        A.CallTo(() => mockHackerNewsService.GetTopStories(1))
            .Returns(new List<HackerNewsStory>
            {
                new("A uBlock Origin update was rejected from the Chrome Web Store",
                    "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
                    "ismaildonmez",
                    new DateTime(2019, 10, 12, 13, 43, 1),
                    1716,
                    572)
            });

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { services.AddSingleton(mockHackerNewsService); });
            });

        var client = application.CreateClient();

        var response = await client.GetStringAsync("/articles/top-stories/1");
        var testObjects = JsonConvert.DeserializeObject<List<HackerNewsArticleResponse>>(response);
        var testObject = testObjects.First();

        testObject.Title.ShouldBe("A uBlock Origin update was rejected from the Chrome Web Store");
        testObject.Uri.ShouldBe("https://github.com/uBlockOrigin/uBlock-issues/issues/745");
        testObject.PostedBy.ShouldBe("ismaildonmez");
        testObject.Time.ShouldBe(new DateTime(2019, 10, 12, 13, 43, 1));
        testObject.Score.ShouldBe(1716);
        testObject.CommentCount.ShouldBe(572);
    }
}