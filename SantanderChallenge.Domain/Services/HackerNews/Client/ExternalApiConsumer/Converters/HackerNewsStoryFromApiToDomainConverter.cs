using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Converters;

// Eventually I'll swap these helpers out for something like AutoMapper (I don't have time now)
public static class HackerNewsStoryFromApiToDomainConverter
{
    public static HackerNewsStory Convert(ExternalHackerNewsStoryResult src)
    {
        // var srcTime = src.time;
        var srcTime = DateTime.Now;
        return new HackerNewsStory(
            src.title, src.url, src.by, srcTime, src.score, -1);
    }
}