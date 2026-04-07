using Microsoft.EntityFrameworkCore;

namespace WowPaperTrader.Persistence.Tests.TestFixtures;

public sealed class SqlServerTestDbFixture : IAsyncLifetime
{
    private static readonly string ConnectionString =
        Environment.GetEnvironmentVariable("WowPaperTrader_TEST_DB")
        ?? throw new InvalidOperationException(
            "Environment variable 'WowPaperTrader_TEST_DB' is not set. " +
            "Set it before running integration tests."
        );

    public DbContextOptions<ApplicationDbContext> Options { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        Options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        await ResetDatabaseAsync();
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

    public async Task<ApplicationDbContext> CreateArrangeDbContextAsync()
    {
        await ResetDatabaseAsync();
        return CreateDbContext();
    }

    public ApplicationDbContext CreateAssertDbContext()
    {
        return CreateDbContext();
    }

    private ApplicationDbContext CreateDbContext()
    {
        if (Options == null)
        {
            throw new InvalidOperationException("Fixture not initialized. Did you forget to use the fixture via IClassFixture?");
        }
        return new ApplicationDbContext(Options);
    }

    public async Task DisposeAsync()
    {
        if (Options == null)
        {
            return;
        }

        await using var db = new ApplicationDbContext(Options);

        await db.Database.EnsureDeletedAsync();
    }

}