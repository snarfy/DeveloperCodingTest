namespace SantanderChallenge.WebApi;

// This would eventually (and ideally) come from a .json file
// (for purpose of this demo this config class should suffice)
internal class HackerNewsApiConfig
{
    public int MaxConcurrentArticleFetching { get; set; }
    public int RefreshArticleOrderSeconds { get; set; }
}