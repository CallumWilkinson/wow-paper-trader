using FluentAssertions;
using WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;
using WowPaperTrader.Persistence.ReadServices;
using WowPaperTrader.Persistence.Tests.TestFixtures;
using WowPaperTrader.Persistence.Tests.TestHelpers;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.QueryTests;

[Collection("SqlServer Database")]
public sealed class MonthlyPriceQuantityReadServiceTests(SqlServerTestDbFixture db)
{
    [Fact]
    public async Task
        GetAsync_ShouldReturn_AllPriceQuantityHistoryFromLast30Days_Given_ItemID_2770()
    {
        //arrange
        await using (var arrangeDbContext = await db.CreateArrangeDbContextAsync())
        {
            var olderTime = DateTime.UtcNow.AddDays(-2);
            await CommodityAuctionSnapshotTestFactory.AddOlderSnapshotToDbAsync(arrangeDbContext, olderTime);

            var newerTime = DateTime.UtcNow.AddDays(-1);
            await CommodityAuctionSnapshotTestFactory.AddNewerSnapshotToDbAsync(arrangeDbContext, newerTime);
        }
        
        const long copperOreItemId = 2770;
        
        //act
        MonthlyPriceQuantityResponse result;
        
        await using (var assertDbContext = db.CreateAssertDbContext())
        {
            
            var readService = new MonthlyPriceQuantityReadService(assertDbContext);
            
            result = await readService.GetAsync(copperOreItemId, CancellationToken.None);
           
        }
        
        //assert
        result.Should().NotBeNull();
        result.ItemId.Should().Be(copperOreItemId);
        result.PriceQuantityResponses.Should().HaveCount(2);
        result.PriceQuantityResponses[0].LowestUnitPrice.Should().Be(80);
        result.PriceQuantityResponses[0].TotalQuantityPosted.Should().Be(15);
        result.PriceQuantityResponses[1].LowestUnitPrice.Should().Be(50);
        result.PriceQuantityResponses[1].TotalQuantityPosted.Should().Be(10);
    }
}