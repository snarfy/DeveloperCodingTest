namespace SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

public class ExternalHackerNewsStoryResult
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
 
    public string by { get; set; }
    public int descendants { get; set; }
    public int id { get; set; }
    public int[] kids { get; set; }
    public int score { get; set; }
    public int time { get; set; }
    public string title { get; set; }
    public string type { get; set; }
    public string url { get; init; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}