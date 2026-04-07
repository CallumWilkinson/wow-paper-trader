using Microsoft.Extensions.Logging.Abstractions;
using WowPaperTrader.Persistence.Repositories;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

public static class CommodityAuctionSnapshotTestFactory
{
    public async static Task AddOlderSnapshotToDbAsync(ApplicationDbContext arrangeDbContext, DateTime olderTime)
    {
        var logger = NullLogger<CommodityAuctionRepository>.Instance;

        var repo = new CommodityAuctionRepository(arrangeDbContext, logger);

        var olderRun = await repo.CreateIngestionRunAsync(CancellationToken.None);

        var result = WowApiResultFactory.CreateOlderApiResult(olderTime);

        await repo.SaveSnapshotAsync(
            olderRun,
            result,
            CancellationToken.None
        );

    }

    public async static Task AddNewerSnapshotToDbAsync(ApplicationDbContext arrangeDbContext, DateTime newerTime)
    {
        var logger = NullLogger<CommodityAuctionRepository>.Instance;

        var repo = new CommodityAuctionRepository(arrangeDbContext, logger);

        var newerRun = await repo.CreateIngestionRunAsync(CancellationToken.None);

        var result = WowApiResultFactory.CreateNewerApiResult(newerTime);

        await repo.SaveSnapshotAsync(
            newerRun,
            result,
            CancellationToken.None
        );
    }

}