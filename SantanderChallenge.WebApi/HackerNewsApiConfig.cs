namespace SantanderChallenge.WebApi;

// This would eventually (and ideally) come from a .json file
// (given my time constraints, this config class should suffice for now)
internal class HackerNewsApiConfig
{
    public int MaxConcurrentArticleFetching { get; set; }
    public int RefreshArticleOrderSeconds { get; set; }
}