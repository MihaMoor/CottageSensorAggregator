namespace PostgresDb.ZontEntities;

public class ZontSensoreEntity
{
    public int Id { get; set; }
    public int ZontId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public double Value { get; set; }
    public string Unit { get; set; }
    /// <summary>
    /// Дата и время получение данных из Zont Api.
    /// </summary>
    public DateTime? FetchedAt { get; set; }
}
