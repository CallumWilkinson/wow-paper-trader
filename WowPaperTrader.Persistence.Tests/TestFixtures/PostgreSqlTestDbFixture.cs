using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace WowPaperTrader.Persistence.Tests.TestFixtures;

public sealed class PostgreSqlTestDbFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer =
        new PostgreSqlBuilder("postgres:18-bookworm")
            .WithDatabase("wowpapertrader_test")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .Build();
    
    private NpgsqlDataSource? _dataSource;
    
    private DbContextOptions<ApplicationDbContext>? _options;
    
    private Respawner? _respawner;
    
    //getters
    private DbContextOptions<ApplicationDbContext> Options =>
        _options ?? throw new InvalidOperationException("The test database fixture has not been initialized. Did you forget to use the fixture via IClassFixture?");
    
    private NpgsqlDataSource DataSource =>
        _dataSource ?? throw new InvalidOperationException("The test database fixture has not been initialized. Did you forget to use the fixture via IClassFixture?");
    
    private Respawner DatabaseRespawner =>
    _respawner ?? throw new InvalidOperationException("The test database fixture has not been initialized. Did you forget to use the fixture via IClassFixture?");

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        _dataSource = NpgsqlDataSource.Create(_postgreSqlContainer.GetConnectionString());
        
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(DataSource)
            .Options;

        await CreateDatabaseSchemaAsync();
        await CreateRespawnerAsync();
    }

    public async Task DisposeAsync()
    {
        if (_dataSource is not null)
        {
            await _dataSource.DisposeAsync();
        }
        
        await _postgreSqlContainer.DisposeAsync();
    }

    private async Task ResetDatabaseAsync()
    {
        await using NpgsqlConnection connection = await DataSource.OpenConnectionAsync();
        
        await DatabaseRespawner.ResetAsync(connection);
    }
    
    private ApplicationDbContext CreateDbContext()
    {
        return new ApplicationDbContext(Options);
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

    private async Task CreateDatabaseSchemaAsync()
    {
        await using ApplicationDbContext db = CreateDbContext();
        
        await db.Database.EnsureCreatedAsync();
        
    }

    private async Task CreateRespawnerAsync()
    {
        await using NpgsqlConnection connection = await DataSource.OpenConnectionAsync();

        _respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude =
                [
                    "public"
                ]
            });
    }
}