using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;
using WowPaperTrader.Persistence.Tests.TestFixtures;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.SchemaTests;

public sealed class MigrationTests(PostgreSqlMigrationTestFixture db) : IClassFixture<PostgreSqlMigrationTestFixture>
{
    [Fact]
    public async Task FreshDb_AppliesAllMigrations()
    {
        await using var dbContext = db.CreateDbContext();

        var migrations = dbContext.Database
            .GetMigrations()
            .ToList();

        var appliedMigrations = (await dbContext.Database
                .GetAppliedMigrationsAsync())
            .ToList();

        var pendingMigrations = (await dbContext.Database
                .GetPendingMigrationsAsync())
            .ToList();

        Assert.NotEmpty(migrations);
        Assert.Equal(migrations, appliedMigrations);
        Assert.Empty(pendingMigrations);
    }

    [Fact]
    public async Task FreshDb_SeedsUsCommodityMarketData()
        
    {
        //arrange
        await using var arrangeDbContext = db.CreateDbContext();
        
        //act
        var markets = await arrangeDbContext.Markets
            .AsNoTracking()
            .Where(m => 
                m.Region == "US" &&
                m.MarketType == MarketType.Commodity &&
                m.ConnectedRealmId == null
                )
            .ToListAsync();
        
        
        //assert
        Assert.Single(markets);
        
        var market = markets[0];
        
        Assert.Equal(1, market.Id);
        Assert.Equal("US", market.Region);
        Assert.Equal(MarketType.Commodity, market.MarketType);
        Assert.Null(market.ConnectedRealmId);
        Assert.Equal("US Commodities", market.DisplayName);
    }
    [Fact]
    public async Task SeededMarket_DoesNotConflictWithGeneratedIds()
    {
        await using var dbContext = db.CreateDbContext();

        await dbContext.Database.OpenConnectionAsync();

        await using var command = dbContext.Database
            .GetDbConnection()
            .CreateCommand();

        command.CommandText =
            """
            INSERT INTO public."Markets"
                ("Region", "MarketType", "ConnectedRealmId", "DisplayName")
            VALUES
                ('EU', 1, NULL, 'EU Commodities')
            RETURNING "Id";
            """;

        var generatedId = (long)(await command.ExecuteScalarAsync())!;

        Assert.NotEqual(1, generatedId);
    }
}
