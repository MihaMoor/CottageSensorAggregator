using CottageSensorAggregator.Settings;
using Microsoft.OpenApi;
using ZontApi;

namespace CottageSensorAggregator;

public class Program
{
    private const string AppName = "—борщик данных с датчиков дома";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureSwagger(builder.Services);
        ConfigureServices(builder.Services, builder.Configuration);

        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            var kestrelSection = context.Configuration.GetRequiredSection("Kestrel");
            options
                .Configure(kestrelSection, true)
                .Endpoint("HTTP", listenOPtions =>
                {
                });

        });

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
        services.AddSingleton(options =>
        {
            AppSettings appSettings = configuration.GetSection("AppSettings").Get<AppSettings>()!;
            ZontCredentials zontCredentials = new(
                appSettings.ZontSettings.Login,
                appSettings.ZontSettings.Password,
                appSettings.ZontSettings.Email);
            return new ZontRepository(zontCredentials, new()
            {
                BaseAddress = new(appSettings.ZontSettings.ApiUrl)
            });
        });

        services.AddControllers();
        services.AddOpenApi();
    }
}