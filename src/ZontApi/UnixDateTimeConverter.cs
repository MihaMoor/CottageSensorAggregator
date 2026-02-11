using System.Text.Json;
using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi;

internal class UnixDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Читаем число и преобразуем в DateTime
        return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()).DateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // При записи превращаем DateTime обратно в Unix Timestamp
        writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeSeconds());
    }
}

internal class UnixNullableDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.Number)
        {
            return null;
        }

        // Читаем число и преобразуем в DateTime
        return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()).DateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value != null)
        {
            // При записи превращаем DateTime обратно в Unix Timestamp
            writer.WriteNumberValue(new DateTimeOffset(value.Value).ToUnixTimeSeconds());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
