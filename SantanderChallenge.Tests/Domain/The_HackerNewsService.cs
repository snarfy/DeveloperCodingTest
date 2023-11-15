using FakeItEasy;
using SantanderChallenge.Domain.Services.HackerNews;
using SantanderChallenge.Domain.Services.HackerNews.Client;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;
using Shouldly;
using Xunit;

namespace SantanderChallenge.Tests.Domain;

// ReSharper disable once InconsistentNaming
public class The_HackerNewsService
{
    [Fact]
    public async void Returns_correct_data_from_HackerNewsApiClient()
    {
        // Arrange
        var dateTimeNow = DateTime.Now;

        var mockHackerNewsTransport = A.Fake<IHackerNewsApi>();
        A.CallTo(() => mockHackerNewsTransport.GetTopStoryIdsAsync())
            .Returns(new List<int> { 123, 232, 15 });
        A.CallTo(() => mockHackerNewsTransport.GetStoryByIdAsync(123))
            .Returns(new HackerNewsStory("title123", "uri123", "pete", dateTimeNow, 100, 1000));

        // Act
        var service = new HackerNewsService(mockHackerNewsTransport);
        var topStories = await service.GetTopStoriesAsync(3);

        // Assert
        topStories.Count().ShouldBe(3, "Expected 3 articles");

        topStories.First().Title.ShouldBe("title123");
        topStories.First().Uri.ShouldBe("uri123");
        topStories.First().PostedBy.ShouldBe("pete");
        topStories.First().Time.ShouldBe(dateTimeNow);
        topStories.First().Score.ShouldBe(100);
        topStories.First().CommentCount.ShouldBe(1000);
    }
}