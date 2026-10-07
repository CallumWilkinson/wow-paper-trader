using WowPaperTrader.Persistence.Tests.TestFixtures;

namespace WowPaperTrader.Persistence.Tests.DatabaseCollections;

[CollectionDefinition(Name)]
public sealed class PostgreSqlDatabaseCollection : ICollectionFixture<PostgreSqlTestDbFixture>
{
    public const string Name = "PostgreSql Database";
}