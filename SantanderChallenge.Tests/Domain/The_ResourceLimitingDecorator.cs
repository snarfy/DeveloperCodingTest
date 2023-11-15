using FakeItEasy;
using SantanderChallenge.Domain.Services.HackerNews.Client;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;
using Xunit;

namespace SantanderChallenge.Tests.Domain;

// ReSharper disable once InconsistentNaming
public class The_ResourceLimitingDecorator
{
    [Fact]
    public async void Synchronizes_and_limits_GetTopStoryIdsAsync_calls_to_just_one_at_a_time_while_cache_is_valid()
    {
        // Arrange
        var mockedDecoratedHackerNewsApi = A.Fake<IHackerNewsApi>();
        A.CallTo(() => mockedDecoratedHackerNewsApi.GetTopStoryIdsAsync())
            .Returns(new List<int> { 123, 232, 15 });

        // Act
        var service = new ResourceLimitingDecorator(mockedDecoratedHackerNewsApi, null, 1, 100);
        var tasks = new List<Task>
        {
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync()
        };
        await Task.WhenAll(tasks);

        // Assert
        A.CallTo(() => mockedDecoratedHackerNewsApi.GetTopStoryIdsAsync())
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async void Calls_out_to_GetTopStoryIdsAsync_every_time_cache_expires()
    {
        // Arrange
        const int refreshArticleOrderSeconds = -1; // forces caching to always be expired

        var mockedDecorated = A.Fake<IHackerNewsApi>();
        A.CallTo(() => mockedDecorated.GetTopStoryIdsAsync())
            .Returns(new List<int> { 123, 232, 15 });

        // Act
        var service = new ResourceLimitingDecorator(mockedDecorated, null, 1, refreshArticleOrderSeconds);
        var tasks = new List<Task>
        {
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync()
        };
        await Task.WhenAll(tasks);

        // Assert
        A.CallTo(() => mockedDecorated.GetTopStoryIdsAsync())
            .MustHaveHappened(3, Times.Exactly);
    }


    [Fact]
    public async void Synchronizes_and_limits_GetStoryByIdAsync_calls_to_only_one_at_a_time_while_cache_is_valid()
    {
        // Arrange
        var mockedDecorated = A.Fake<IHackerNewsApi>();
        A.CallTo(() => mockedDecorated.GetStoryByIdAsync(123))
            .Returns(new HackerNewsStory("title123", "123", "pete", DateTime.Now, 100, 1000));

        // Act
        var service = new ResourceLimitingDecorator(mockedDecorated, null, 1, 100);
        var tasks = new List<Task>
        {
            service.GetStoryByIdAsync(123),
            service.GetStoryByIdAsync(123),
            service.GetStoryByIdAsync(123),
            service.GetStoryByIdAsync(123)
        };
        await Task.WhenAll(tasks);

        // Assert
        A.CallTo(() => mockedDecorated.GetStoryByIdAsync(123))
            .MustHaveHappenedOnceExactly();
    }
}