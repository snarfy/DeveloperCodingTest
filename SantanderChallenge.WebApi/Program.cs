using AutoMapper;
using SantanderChallenge.Domain.MappingProfiles;
using SantanderChallenge.Domain.Services.HackerNews;
using SantanderChallenge.Domain.Services.HackerNews.Client;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer;
using SantanderChallenge.WebApi.MappingProfiles;

namespace SantanderChallenge.WebApi;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = new HackerNewsApiConfig
        {
            MaxConcurrentArticleFetching = 10 // Config - would have moved it to .json config file given plenty of time
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
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new HackerNewsStoryMappingProfile());
            cfg.AddProfile(new ExternalHackerNewsStoryResultProfile());
        });
        builder.Services.AddSingleton(mapperConfig.CreateMapper());
        builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();
        builder.Services.AddTransient<HackerNewsApiClient>();
        builder.Services.AddSingleton<IHackerNewsApi>(provider =>
        {
            var decoratedService = provider.GetRequiredService<HackerNewsApiClient>();
            var logger = provider.GetRequiredService<ILogger<ResourceLimitingDecorator>>();

            return new ResourceLimitingDecorator(decoratedService, logger,
                hackerNewsApiConfig.MaxConcurrentArticleFetching, hackerNewsApiConfig.RefreshArticleOrderSeconds);
        });
        builder.Services.AddTransient<HttpClient>();
    }
}