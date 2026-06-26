using WowPaperTrader.Persistence.Tests.TestFixtures;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

[Collection( "PostgreSql Database")]
public abstract class PostgreSqlIntegrationTestBase(PostgreSqlTestDbFixture db) : IAsyncLifetime
{
    protected PostgreSqlTestDbFixture Db { get; } = db;
    
    public Task InitializeAsync()
    {
        return Db.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}