namespace PostgresDb.ZontEntities;

public class ZontDeviceEntity
{
    public int Id { get; set; }
    public int ZontId { get; set; }
    public string DeviceId { get; set; }
    public string Name { get; set; }
    public bool IsOnline { get; set; }
    public string DeviceModel { get; set; }
    public string SoftwareVersion { get; set; }
    public string HardwareVersion { get; set; }
    /// <summary>
    /// Дата и время получение данных из Zont Api.
    /// </summary>
    public DateTime? FetchedAt { get; set; }
}
