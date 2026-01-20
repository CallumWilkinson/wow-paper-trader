using Microsoft.EntityFrameworkCore;

public sealed class IngestorDbContext : DbContext
{
    public IngestorDbContext(DbContextOptions<IngestorDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }

    public DbSet<IngestionRun> IngestionRuns { get; set; } = null!;

}