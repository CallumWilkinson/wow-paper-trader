namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.ApiResponse;

public sealed record WowApiResponse<T>(
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