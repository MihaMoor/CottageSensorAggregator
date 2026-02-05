using CottageSensorAggregator.Settings;
using Microsoft.OpenApi;

namespace CottageSensorAggregator;

public class Program
{
    private const string AppName = "—борщик данных с датчиков дома";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureSwagger(builder.Services);
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", " v1");
            options.DocumentTitle = AppName;
            options.RoutePrefix = string.Empty;
            options.EnableTryItOutByDefault();
        });

        app.MapOpenApi();
        app.MapControllers();

        app.Run();
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = AppName, Version = "v1" });
        });
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        services.AddControllers();
        services.AddOpenApi();
    }
}