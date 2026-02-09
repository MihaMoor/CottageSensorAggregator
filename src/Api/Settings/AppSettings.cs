using System.ComponentModel.DataAnnotations;

namespace CottageSensorAggregator.Settings;

public record AppSettings(
    [Required] ZontSettings ZontSettings)
{
    public AppSettings() : this(new ZontSettings()) { }
}

public record ZontSettings(
    [Required] string Email,
    [Required] string Login,
    [Required] string Password,
    [Required] string ApiUrl)
{
    public ZontSettings() : this(string.Empty, string.Empty, string.Empty, string.Empty) { }
}
