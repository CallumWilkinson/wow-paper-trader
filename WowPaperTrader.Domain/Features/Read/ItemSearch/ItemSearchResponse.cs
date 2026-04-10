namespace WowPaperTrader.Domain.Features.ItemSearch;

public sealed class ItemSearchResponse
{
    public long ItemId { get; init; }
    public string Name { get; init; } = string.Empty;
}