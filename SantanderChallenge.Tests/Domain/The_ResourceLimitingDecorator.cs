using FakeItEasy;
using SantanderChallenge.Domain.Services.HackerNews.Client;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;
using Xunit;

namespace SantanderChallenge.Tests.Domain;

// ReSharper disable once InconsistentNaming
public class The_ResourceLimitingDecorator
{
    [Fact]
    public async void Will_synchronise_and_limit_calls_for_GetTopStoryIdsAsync_to_one_at_a_time_while_cache_is_valid()
    {
        var mockedDecorated = A.Fake<IHackerNewsApi>();
        A.CallTo(() => mockedDecorated.GetTopStoryIdsAsync())
            .Returns(new List<int> { 123, 232, 15 });

        var service = new ResourceLimitingDecorator(mockedDecorated, null, 1, 100);

        var tasks = new List<Task>
        {
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync()
        };
        await Task.WhenAll(tasks);

        A.CallTo(() => mockedDecorated.GetTopStoryIdsAsync())
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async void Will_call_out_to_GetTopStoryIdsAsync_every_time_cache_expires()
    {
        const int refreshArticleOrderSeconds = -1; // forces caching to always be expired

        var mockedDecorated = A.Fake<IHackerNewsApi>();
        A.CallTo(() => mockedDecorated.GetTopStoryIdsAsync())
            .Returns(new List<int> { 123, 232, 15 });

        var service = new ResourceLimitingDecorator(mockedDecorated, null, 1, refreshArticleOrderSeconds);

        var tasks = new List<Task>
        {
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync(),
            service.GetTopStoryIdsAsync()
        };
        await Task.WhenAll(tasks);

        A.CallTo(() => mockedDecorated.GetTopStoryIdsAsync())
            .MustHaveHappened(3, Times.Exactly);
    }


    [Fact]
    public async void Will_synchronise_and_limit_calls_far_GetStoryByIdAsync_to_only_one_at_a_time()
    {
        var mockedDecorated = A.Fake<IHackerNewsApi>();
        A.CallTo(() => mockedDecorated.GetStoryByIdAsync(123))
            .Returns(new HackerNewsStory("title123", "123", "pete", DateTime.Now, 100, 1000));

        var service = new ResourceLimitingDecorator(mockedDecorated, null, 1, 100);

        var tasks = new List<Task>
        {
            service.GetStoryByIdAsync(123),
            service.GetStoryByIdAsync(123),
            service.GetStoryByIdAsync(123),
            service.GetStoryByIdAsync(123)
        };
        await Task.WhenAll(tasks);

        A.CallTo(() => mockedDecorated.GetStoryByIdAsync(123))
            .MustHaveHappenedOnceExactly();
    }
}