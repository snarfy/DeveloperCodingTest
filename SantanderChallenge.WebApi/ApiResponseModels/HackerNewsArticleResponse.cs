namespace SantanderChallenge.WebApi.ApiResponseModels;

public record HackerNewsArticleResponse(
    string Title,
    string Uri,
    string PostedBy,
    DateTime Time,
    int Score,
    int CommentCount
);