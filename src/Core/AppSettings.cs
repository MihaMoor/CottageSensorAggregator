namespace CottageSensorAggregator;

public class AppSettings
{
    public const string AppName = "Сборщик данных с датчиков дома";
    public const string ZontHttpClientName = "ZontHttpClient";

    public ZontSettings ZontSettings { get; init; } = null!;
}
