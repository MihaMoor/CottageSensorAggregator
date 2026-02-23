using CottageSensorAggregator.Api.Middlewares;
using CottageSensorAggregator.BackgroundWorkers;
using CottageSensorAggregator.Core;
using CottageSensorAggregator.Core.Loggers;
using CottageSensorAggregator.ZontApi;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using Serilog;

namespace CottageSensorAggregator;

public class Program
{
    public static void Main(string[] args)
    {
        Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogging(builder);
        ConfigureServices(builder.Services, builder.Configuration);
        ConfigureBackgroundServices(builder.Services);
        ConfigureSwagger(builder.Services, builder.Configuration);

        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            var kestrelSection = context.Configuration.GetRequiredSection("Kestrel");
            options
                .Configure(kestrelSection, true)
                .Endpoint("HTTP", listenOPtions =>
                {
                });

        });

        AddHealthCheck(builder);

        try
        {
            var app = builder.Build();

            app.UseSerilogRequestLogging();
            app.UseMiddleware<HealthCheckLoggingMiddleware>();
            app.MapHealthChecks("/health");

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.DocumentTitle = AppSettings.AppName;
                options.RoutePrefix = string.Empty;
                options.EnableTryItOutByDefault();
            });

            app.MapOpenApi();
            app.MapControllers();

            AuthInZont(app.Services.CreateScope());

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, ex.Message, ex.StackTrace);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ZontSettings>(configuration.GetSection("ZontSettings"));
        services.Configure<HealthCheckSettings>(configuration.GetSection("HealthCheckSettings"));

        services.AddSingleton(typeof(HealthCheckLogger<>));
        services.AddSingleton(typeof(CollectZontDeviceDataLogger<>));
        services.AddSingleton(typeof(ApplicationLogger<>));

        ZontSettings zontSettings = configuration.GetSection("ZontSettings").Get<ZontSettings>()!;

        services.AddHttpClient(AppSettings.ZontHttpClientName, httpClient =>
        {
            var authString = $"{zontSettings.Login}:{zontSettings.Password}";
            var base64AuthString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authString));

            httpClient.BaseAddress = new Uri(zontSettings.ApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-ZONT-Client", zontSettings.Email);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                base64AuthString
            );
        });

        services.AddSingleton<ZontRepository>();

        services.AddControllers();
        services.AddOpenApi();
    }

    private static void ConfigureBackgroundServices(IServiceCollection services)
    {
        services.AddHostedService<HealthCheckReporter>();
        services.AddHostedService<CollectZontDeviceData>();
    }

    private static void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = AppSettings.AppName, Version = "v1" });
        });
    }

    private static void AddHealthCheck(WebApplicationBuilder builder)
    {
        HealthCheckSettings healthCheckSettings = builder.Configuration.GetSection("HealthCheckSettings").Get<HealthCheckSettings>()!;

        builder.Services.AddHealthChecks();

        builder.Services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = healthCheckSettings.Delay;  // Задержка перед первым запуском
            options.Period = healthCheckSettings.Period; // Интервал между проверками
        });
    }

    private static async void AuthInZont(IServiceScope scope, CancellationToken cancellationToken = default)
    {
        var zontRepository = scope.ServiceProvider.GetRequiredService<ZontRepository>();
        var isSuccess = false;
        while (!isSuccess)
        {
            try
            {
                isSuccess = await zontRepository.AuthorizeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, ex.Message, ex.StackTrace);
            }
        }
    }
}