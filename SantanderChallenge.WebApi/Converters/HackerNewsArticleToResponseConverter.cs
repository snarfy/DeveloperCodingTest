using SantanderChallenge.Domain.Services.HackerNews.Models;
using SantanderChallenge.WebApi.ResponseModels;

namespace SantanderChallenge.WebApi.Converters;

public static class HackerNewsArticleToResponseConverter
{
    public static HackerNewsArticleResponse ToResponseModel(HackerNewsStory src)
    {
        return new HackerNewsArticleResponse(src.Title, src.Uri, src.PostedBy, src.Time, src.Score, src.CommentCount);
    }
}