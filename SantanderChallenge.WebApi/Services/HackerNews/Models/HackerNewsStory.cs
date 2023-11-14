namespace SantanderChallenge.WebApi.Services.HackerNews.Models;

public record HackerNewsStory(
    string Title,
    string Uri,
    string PostedBy,
    DateTime Time,
    int Score,
    int CommentCount
)
{
    public static HackerNewsStory Stub =>
        new("Title", "Uri", "PostedBy", DateTime.Now,
            123, 567);
}