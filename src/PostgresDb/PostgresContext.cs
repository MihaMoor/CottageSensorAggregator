using Microsoft.EntityFrameworkCore;

namespace PostgresDb;

public class PostgresContext : DbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
