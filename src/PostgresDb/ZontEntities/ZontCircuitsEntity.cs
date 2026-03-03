namespace PostgresDb.ZontEntities;

public class ZontCircuitsEntity
{
    public int Id { get; set; }
    public int ZontId { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public double ActualTemp { get; set; }
    public double CurrentTemp { get; set; }
    public bool IsActive { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public int Step { get; set; }
    /// <summary>
    /// Дата и время получение данных из Zont Api.
    /// </summary>
    public DateTime? FetchedAt { get; set; }
}
