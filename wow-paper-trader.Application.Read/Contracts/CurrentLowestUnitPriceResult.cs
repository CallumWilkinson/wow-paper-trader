public sealed record CurrentLowestUnitPriceResult
(
    long ItemId,
    long UnitPrice,
    DateTime PriceTakenAtUtc
);