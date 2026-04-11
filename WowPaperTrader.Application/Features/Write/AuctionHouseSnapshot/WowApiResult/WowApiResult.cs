namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.WowApiResult;

public sealed record WowApiResult<T>(
    T Payload,
    DateTime DataReturnedAtUtc,
    string Endpoint
);

public sealed record AuctionSnapshot(
    List<AuctionSnapshotRow> Auctions
);

public sealed record AuctionSnapshotRow(
    long ItemId,
    long Quantity,
    long UnitPrice,
    string TimeLeft
);