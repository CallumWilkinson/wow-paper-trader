namespace WowPaperTrader.Application.Features.Read.ItemSearch;

public sealed class ItemSearchResponse
{
    public long ItemId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
}