using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using Testcontainers.PostgreSql;

namespace WowPaperTrader.Persistence.Tests.TestFixtures;

public abstract class PostgreSqlTestDbFixtureBase(string databaseName) : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder("postgres:18-bookworm")
        .WithDatabase(databaseName)
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();
    
    private NpgsqlDataSource? _dataSource;
    
    private DbContextOptions<ApplicationDbContext>? _dbContextOptions;

    protected NpgsqlDataSource DataSource =>
        _dataSource ?? throw new InvalidOperationException(
            "The test database fixture has not been initialized.");

    private DbContextOptions<ApplicationDbContext> DbContextOptions =>
        _dbContextOptions ?? throw new InvalidOperationException(
            "The test database fixture has not been initialized.");

    private string ConnectionString => _postgreSqlContainer.GetConnectionString();

    protected virtual string? TargetMigration => null;

    public virtual async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        _dataSource = NpgsqlDataSource.Create(ConnectionString);

        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(DataSource)
            .Options;

        await CreateDatabaseSchemaAsync();
    }

    private async Task CreateDatabaseSchemaAsync()
    {
        await using ApplicationDbContext db = CreateDbContext();

        var migrator = db.Database.GetService<IMigrator>();
        await migrator.MigrateAsync(TargetMigration);
    }

    public ApplicationDbContext CreateDbContext()
    {
        return new ApplicationDbContext(DbContextOptions);
    }

    public async Task DisposeAsync()
    {
        if (_dataSource is not null)
        {
            await _dataSource.DisposeAsync();
        }

        await _postgreSqlContainer.DisposeAsync();
    }
}
