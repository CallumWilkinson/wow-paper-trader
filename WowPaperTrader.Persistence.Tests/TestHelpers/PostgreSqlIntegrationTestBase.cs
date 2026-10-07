using WowPaperTrader.Persistence.Tests.DatabaseCollections;
using WowPaperTrader.Persistence.Tests.TestFixtures;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

[Collection(PostgreSqlDatabaseCollection.Name)]
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