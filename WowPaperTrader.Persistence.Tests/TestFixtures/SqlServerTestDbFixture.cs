using Microsoft.EntityFrameworkCore;

namespace WowPaperTrader.Persistence.Tests.TestFixtures;

public sealed class SqlServerTestDbFixture : IAsyncLifetime
{
    private static readonly string ConnectionString =
        Environment.GetEnvironmentVariable("WOW_PAPER_TRADER_TEST_DB")
        ?? throw new InvalidOperationException(
            "Environment variable 'WOW_PAPER_TRADER_TEST_DB' is not set. " +
            "Set it before running integration tests."
        );
    // copy-paste this to set environment variable to a new local test db that doesn't interact with regular project db
    // [System.Environment]::SetEnvironmentVariable( "WOW_PAPER_TRADER_TEST_DB", "Server=CallumPC\SQLEXPRESS;Database=WowPaperTrader_TestDb;Trusted_Connection=True;TrustServerCertificate=True;", "User" )

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