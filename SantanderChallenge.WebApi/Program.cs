namespace SantanderChallenge.WebApi;

internal class Program
{
    public static void Main(string[] args)
    {
        //var builderFactory = new WebApplicationBuilderFactory();
        //var builder = builderFactory.Get();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        //var server = new TestServer(builder.WebHost);
        //var _client = server.CreateClient();

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
}