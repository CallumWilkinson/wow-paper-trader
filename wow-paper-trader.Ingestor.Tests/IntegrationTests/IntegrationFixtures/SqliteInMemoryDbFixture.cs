using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public sealed class SqliteInMemoryDbFixture : IAsyncLifetime
{
    private SqliteConnection _connection = default!;

    public DbContextOptions<IngestorDbContext> Options { get; private set; } = default!;

    //calls automatically if becuase this fixture inherits from IAsyncLifetime
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
        if (Options == null)
        {
            throw new InvalidOperationException("Fixture not initialized. Did you forget to use the fixture via IClassFixture?");
        }
        return new IngestorDbContext(Options);
    }

    public async Task ResetDatabaseAsync()
    {
        if (Options == null)
        {
            throw new InvalidOperationException("Fixture not initialized. Did you forget to use the fixture via IClassFixture?");
        }
        await using var db = new IngestorDbContext(Options);
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }
}
