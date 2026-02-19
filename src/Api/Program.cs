using CottageSensorAggregator.ZontApi;
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

        var app = builder.Build();

        app.UseSerilogRequestLogging();

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

    private static void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = AppSettings.AppName, Version = "v1" });
        });
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        AppSettings appSettings = configuration.GetSection("AppSettings").Get<AppSettings>()!;

        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        services.AddHttpClient(AppSettings.ZontHttpClientName, httpClient =>
        {
            var authString = $"{appSettings.ZontSettings.Login}:{appSettings.ZontSettings.Password}";
            var base64AuthString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authString));

            httpClient.BaseAddress = new Uri(appSettings.ZontSettings.ApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-ZONT-Client", appSettings.ZontSettings.Email);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                base64AuthString
            );
        });

        services.AddScoped<ZontRepository>();

        services.AddControllers();
        services.AddOpenApi();
    }

    private static async void AuthInZont(IServiceScope scope, CancellationToken cancellationToken = default)
    {
        var zontRepository = scope.ServiceProvider.GetRequiredService<ZontRepository>();
        await zontRepository.AuthorizeAsync(cancellationToken);
    }
}