using WowPaperTrader.Persistence.Tests.DatabaseCollections;
using WowPaperTrader.Persistence.Tests.TestFixtures;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

[Collection(PostgreSqlDatabaseCollection.Name)]
public abstract class PostgreSqlIntegrationTestBase(PostgreSqlIntegrationTestDbFixture db) : IAsyncLifetime
{
    protected PostgreSqlIntegrationTestDbFixture Db { get; } = db;
    
    public Task InitializeAsync()
    {
        return Db.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}