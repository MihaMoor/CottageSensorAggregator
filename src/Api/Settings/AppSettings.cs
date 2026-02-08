using System.ComponentModel.DataAnnotations;

namespace CottageSensorAggregator.Settings;

public record AppSettings(
    [Required] ZontSettings ZontSettings);

public record ZontSettings(
    [Required] string Email,
    [Required] string Login,
    [Required] string Password);
