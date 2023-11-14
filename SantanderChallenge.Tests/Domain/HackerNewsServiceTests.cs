using FakeItEasy;
using SantanderChallenge.Domain.Services.HackerNews;
using SantanderChallenge.Domain.Services.HackerNews.Models;
using SantanderChallenge.Domain.Services.HackerNews.Transport;
using Shouldly;
using Xunit;

namespace SantanderChallenge.Tests.Domain;

public class HackerNewsServiceTests
{
    [Fact]
    public async void Returns_correct_data_from_HackerNewsApiClient()
    {
        var mockHackerNewsTransport = A.Fake<IHackerNewsApiClient>();
        A.CallTo(() => mockHackerNewsTransport.GetTopStoryIdsAsync(3))
            .Returns(new List<int> { 123, 232, 15 });
        A.CallTo(() => mockHackerNewsTransport.GetStoryByIdAsync(123))
            .Returns(new HackerNewsStory("title123", "123", "pete", DateTime.Now, 100, 1000));

        var service = new HackerNewsService(mockHackerNewsTransport);
        var topStories = await service.GetTopStoriesAsync(3);

        topStories.Count().ShouldBe(3);

        topStories.First().Title.ShouldBe("title123");
        topStories.First().Uri.ShouldBe("123");
        topStories.First().PostedBy.ShouldBe("pete");
        topStories.First().Score.ShouldBe(100);
        topStories.First().CommentCount.ShouldBe(1000);
    }
}