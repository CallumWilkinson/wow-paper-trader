using Npgsql;
using Respawn;
using Respawn.Graph;

namespace WowPaperTrader.Persistence.Tests.TestFixtures;

public sealed class PostgreSqlIntegrationTestDbFixture() : PostgreSqlTestDbFixtureBase("wowpapertrader_test")
{
    private Respawner? _respawner;
    
    private Respawner DatabaseRespawner => _respawner ?? throw new InvalidOperationException("Respawner is not initialized");

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await CreateRespawnerAsync();
    }

    private async Task CreateRespawnerAsync()
    {
        await using NpgsqlConnection connection = 
            await DataSource.OpenConnectionAsync();

        _respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude =
                [
                    "public"
                ],
                TablesToIgnore =
                [
                    new Table("public", "__EFMigrationsHistory")
                ]
            });
    }
    
    public async Task ResetDatabaseAsync()
    {
        await using NpgsqlConnection connection =
            await DataSource.OpenConnectionAsync();

        await DatabaseRespawner.ResetAsync(connection);
    }

}
