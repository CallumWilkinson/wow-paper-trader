using wow_paper_trader.Ingestor;

public sealed class CommodityAuction
{
    public long Id { get; private set; }

    //many to one relationship
    public long CommodityAuctionSnapshotId { get; private set; }

    //nagivation property
    public CommodityAuctionSnapshot CommodityAuctionSnapshot { get; private set; } = null!;

    public long ItemId { get; private set; }

    public long Quantity { get; private set; }

    public long UnitPrice { get; private set; }

    public string TimeLeft { get; private set; } = string.Empty;

    public CommodityAuction(long itemId, long quantity, long unitPrice, string timeLeft)
    {
        ItemId = itemId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TimeLeft = timeLeft;
    }


}
