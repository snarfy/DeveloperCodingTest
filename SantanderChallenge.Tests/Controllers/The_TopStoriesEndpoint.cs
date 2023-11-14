using System.Net;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SantanderChallenge.Domain.Services.HackerNews;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;
using SantanderChallenge.WebApi;
using SantanderChallenge.WebApi.ApiResponseModels;
using Shouldly;
using Xunit;

namespace SantanderChallenge.Tests.Controllers;

// ReSharper disable once InconsistentNaming
public class The_TopStoriesEndpoint
{
    [Fact]
    public async void Return_data_from_HackerNewsService_in_correct_format()
    {
        var mockHackerNewsService = A.Fake<IHackerNewsService>();
        A.CallTo(() => mockHackerNewsService.GetTopStoriesAsync(1))
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

    [Fact]
    public async void Handles_exceptions_gracefully()
    {
        var mockHackerNewsService = A.Fake<IHackerNewsService>();
        A.CallTo(() => mockHackerNewsService.GetTopStoriesAsync(1))
            .Throws(new Exception("Kaboooom! [Broken service simulation]"));

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { services.AddSingleton(mockHackerNewsService); });
            });

        var client = application.CreateClient();

        var expectedErrorResponse = await client.GetAsync("/articles/top-stories/1");

        expectedErrorResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        var errorMessage = await expectedErrorResponse.Content.ReadAsStringAsync();
        errorMessage.ShouldBe("An internal server error occurred");
    }
}