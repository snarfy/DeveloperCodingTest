using SantanderChallenge.Domain.Services.HackerNews;
using SantanderChallenge.Domain.Services.HackerNews.Transport;

namespace SantanderChallenge.WebApi;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        RegisterServices(builder);

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

    private static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();
        builder.Services.AddTransient<IHackerNewsProvider, HackerNewsProvider>();
        builder.Services.AddTransient<IHackerNewsApiClient, HackerNewsApiClient>();
    }
}