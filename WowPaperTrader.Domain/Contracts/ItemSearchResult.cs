namespace WowPaperTrader.Domain.Contracts;

public sealed class ItemSearchResult
{
    public long ItemId { get; init; }
    public string Name { get; init; } = string.Empty;
}