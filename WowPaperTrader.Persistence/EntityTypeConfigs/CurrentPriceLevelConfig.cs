using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence.EntityTypeConfigs;

public sealed class CurrentPriceLevelConfig: IEntityTypeConfiguration<CurrentPriceLevel>
{
    public void Configure(EntityTypeBuilder<CurrentPriceLevel> builder)
    {
        builder.HasKey(priceLevel => new
        {
            priceLevel.AuctionMarketId,
            priceLevel.ItemId,
            priceLevel.VariantKey,
            priceLevel.UnitPrice
        });

        builder.HasOne(priceLevel => priceLevel.AuctionMarketSnapshot)
            .WithMany(auctionMarketSnapshot => auctionMarketSnapshot.CurrentPriceLevels)
            .HasForeignKey(priceLevel => new
            {
                priceLevel.AuctionMarketId,
                priceLevel.ObservedAtUtc
            });
    }
}