namespace WowPaperTrader.Application.Read.Contracts;

public sealed class ItemMetadataAndPriceResult
{
    public long ItemId { get; init; }
    public long UnitPrice { get; init; }
    public DateTime PriceTakenAtUtc { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? QualityType { get; init; }
    public string? QualityName { get; init; }
    public int Level { get; init; }
    public int RequiredLevel { get; init; }
    public int? ItemClassId { get; init; }
    public string? ItemClassName { get; init; }
    public int? ItemSubclassId { get; init; }
    public string? ItemSubclassName { get; init; }
    public int? ProfessionId { get; init; }
    public string? ProfessionName { get; init; }
    public int? ProfessionSkillLevel { get; init; }
    public string? SkillDisplayString { get; init; }
    public string? CraftingReagent { get; init; }
    public string? InventoryType { get; init; }
    public string? InventoryTypeName { get; init; }
    public long? PurchasePrice { get; init; }
    public long? SellPrice { get; init; }
    public int? MaxCount { get; init; }
    public bool IsEquippable { get; init; }
    public bool IsStackable { get; init; }
    public int? PurchaseQuantity { get; init; }
    public string? ImageUrl { get; init; }
    public DateTime MetadataLastFetchedUtc { get; init; }
}