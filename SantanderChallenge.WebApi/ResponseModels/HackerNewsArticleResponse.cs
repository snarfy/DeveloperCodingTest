namespace SantanderChallenge.WebApi.ResponseModels;

public record HackerNewsArticleResponse(
    string Title,
    string Uri,
    string PostedBy,
    DateTime Time,
    int Score,
    int CommentCount
)
{
    public static HackerNewsArticleResponse Stub =>
        new("title", "uri", "by", DateTime.Now, 123, 567);
}