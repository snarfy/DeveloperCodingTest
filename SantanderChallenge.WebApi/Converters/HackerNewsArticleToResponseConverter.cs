using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;
using SantanderChallenge.WebApi.ApiResponseModels;

namespace SantanderChallenge.WebApi.Converters;

// Eventually I'll swap these helpers out for something like AutoMapper (I don't have time now)
public static class HackerNewsArticleToResponseConverter
{
    public static HackerNewsArticleResponse ToResponseModel(HackerNewsStory src)
    {
        return new HackerNewsArticleResponse(src.Title, src.Uri, src.PostedBy, src.Time, src.Score, src.CommentCount);
    }
}