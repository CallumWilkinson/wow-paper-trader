using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public sealed class SqliteInMemoryDbFixture : IAsyncLifetime
{
    private SqliteConnection _connection = default!;

    public DbContextOptions<ApplicationDbContext> Options { get; private set; } = default!;

    //calls automatically because this fixture inherits from IAsyncLifetime
    public async Task InitializeAsync()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        await _connection.OpenAsync();

        Options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        await using var db = new ApplicationDbContext(Options);
        await db.Database.EnsureCreatedAsync();

    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    public ApplicationDbContext CreateAssertDbContext()
    {
        return CreateDbContext();
    }

    //arrangeDbContext must reset the database
    public async Task<ApplicationDbContext> CreateArrangeDbContextAsync()
    {
        await ResetDatabaseAsync();
        return CreateDbContext();
    }

    private async Task ResetDatabaseAsync()
    {
        if (Options == null)
        {
            throw new InvalidOperationException("Fixture not initialized. Did you forget to use the fixture via IClassFixture?");
        }
        await using var db = new ApplicationDbContext(Options);
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }

    private ApplicationDbContext CreateDbContext()
    {
        if (Options == null)
        {
            throw new InvalidOperationException("Fixture not initialized. Did you forget to use the fixture via IClassFixture?");
        }
        return new ApplicationDbContext(Options);
    }
}
