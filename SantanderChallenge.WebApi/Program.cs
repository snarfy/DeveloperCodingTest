using SantanderChallenge.Domain.Services.HackerNews;
using SantanderChallenge.Domain.Services.HackerNews.Client;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer;

namespace SantanderChallenge.WebApi;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = new HackerNewsApiConfig
        {
            MaxConcurrentArticleFetching = 3
        };

        RegisterServices(builder, config);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void RegisterServices(WebApplicationBuilder builder, HackerNewsApiConfig hackerNewsApiConfig)
    {
        builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();
        builder.Services.AddTransient<HackerNewsApiClient>();
        builder.Services.AddSingleton<IHackerNewsApi>(provider =>
        {
            var decoratedService =
                provider.GetRequiredService<HackerNewsApiClient>(); // Replace with the actual implementation type
            var logger =
                provider
                    .GetRequiredService<
                        ILogger<ResourceLimitingDecorator>>(); // Replace with the actual implementation type
            return new ResourceLimitingDecorator(decoratedService, logger,
                hackerNewsApiConfig.MaxConcurrentArticleFetching, hackerNewsApiConfig.RefreshArticleOrderSeconds);
        });
        builder.Services.AddTransient<HttpClient>();
    }
}