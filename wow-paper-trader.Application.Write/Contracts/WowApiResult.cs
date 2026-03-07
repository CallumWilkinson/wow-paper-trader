public sealed record WowApiResult<T>
(
    T DtoPayload,
    DateTime DataReturnedAtUtc,
    string Endpoint
);

public sealed record AuctionSnapshot(
    IReadOnlyList<AuctionSnapshotRow> Auctions
);

public sealed record AuctionSnapshotRow(
    long ItemId,
    long Quantity,
    long UnitPrice,
    string TimeLeft
);