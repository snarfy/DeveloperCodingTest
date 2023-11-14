namespace SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

public record HackerNewsStory(
    string Title,
    string Uri,
    string PostedBy,
    DateTime Time,
    int Score,
    int CommentCount
);