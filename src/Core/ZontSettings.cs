using System.ComponentModel.DataAnnotations;

namespace CottageSensorAggregator;

public record ZontSettings(
    [Required] string Email,
    [Required] string Login,
    [Required] string Password,
    [Required] string ApiUrl,
    [Required] TimeSpan CollectDeviceDataInterval)
{
    public ZontSettings() : this(string.Empty, string.Empty, string.Empty, string.Empty, TimeSpan.MaxValue) { }
}
