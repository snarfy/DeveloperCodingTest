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
    public async void Returns_HackerNewsService_data_in_the_correct_format()
    {
        // Arrange
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


        // Act
        var client = application.CreateClient();
        var response = await client.GetStringAsync("/articles/top-stories/1");
        var resultSet = JsonConvert.DeserializeObject<List<HackerNewsArticleResponse>>(response) 
                        ?? new List<HackerNewsArticleResponse>();
        var firstItem = resultSet.First();

        // Assert
        firstItem.Title.ShouldBe("A uBlock Origin update was rejected from the Chrome Web Store");
        firstItem.Uri.ShouldBe("https://github.com/uBlockOrigin/uBlock-issues/issues/745");
        firstItem.PostedBy.ShouldBe("ismaildonmez");
        firstItem.Time.ShouldBe(new DateTime(2019, 10, 12, 13, 43, 1));
        firstItem.Score.ShouldBe(1716);
        firstItem.CommentCount.ShouldBe(572);
    }

    [Fact]
    public async void Handles_exceptions_gracefully()
    {
        // Arrange
        var mockBrokenHackerNewsService = A.Fake<IHackerNewsService>();
        A.CallTo(() => mockBrokenHackerNewsService.GetTopStoriesAsync(1))
            .Throws(new Exception("Kaboooom! [Broken service simulation]"));

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { services.AddSingleton(mockBrokenHackerNewsService); });
            });

        // Act
        var client = application.CreateClient();
        var expectedErrorResponse = await client.GetAsync("/articles/top-stories/1");

        // Assert
        expectedErrorResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        var errorMessage = await expectedErrorResponse.Content.ReadAsStringAsync();
        errorMessage.ShouldBe("An internal server error occurred");
    }
}