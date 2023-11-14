using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SantanderChallenge.Domain.Services.HackerNews;
using SantanderChallenge.Domain.Services.HackerNews.Models;
using SantanderChallenge.WebApi;
using SantanderChallenge.WebApi.ResponseModels;
using Shouldly;
using Xunit;

namespace SantanderChallenge.Tests.Controllers;

public class HackerNewsServiceTests
{
    [Fact]
    public async void Returns_correct_number_of_HackerNews_stories()
    {
        var service = new HackerNewsService();
        var topStories = await service.GetTopStories(5);

        topStories.Count().ShouldBe(5);
    }
}