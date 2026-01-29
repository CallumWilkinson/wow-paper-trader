using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public sealed class SqliteInMemoryDbFixture : IAsyncLifetime
{
    private SqliteConnection _connection = default!;

    public DbContextOptions<IngestorDbContext> Options { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        await _connection.OpenAsync();

        Options = new DbContextOptionsBuilder<IngestorDbContext>()
            .UseSqlite(_connection)
            .Options;

        await using var db = new IngestorDbContext(Options);
        await db.Database.EnsureCreatedAsync();

    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    public IngestorDbContext CreateDbContext()
    {
        return new IngestorDbContext(Options);
    }
}