namespace WowPaperTrader.Domain.Features.Write.UpdateItems;

public sealed class ItemMetaData
{
    public long Id { get; set; } // Primary key

    public long ItemId { get; set; }
    public string Name { get; set; } = string.Empty;

    public string? QualityType { get; set; }        
    public string? QualityName { get; set; }

    public int Level { get; set; }
    public int RequiredLevel { get; set; }

    public int? ItemClassId { get; set; }
    public string? ItemClassName { get; set; }

    public int? ItemSubclassId { get; set; }
    public string? ItemSubclassName { get; set; }

    public int? ProfessionId { get; set; }
    public string? ProfessionName { get; set; }
    public int? ProfessionSkillLevel { get; set; }
    public string? SkillDisplayString { get; set; }

    public string? CraftingReagent { get; set; }

    public string? InventoryType { get; set; }
    public string? InventoryTypeName { get; set; }

    public long? PurchasePrice { get; set; }
    public long? SellPrice { get; set; }

    public int? MaxCount { get; set; }

    public bool IsEquippable { get; set; }
    public bool IsStackable { get; set; }

    public int? PurchaseQuantity { get; set; }

    public string? ImageUrl { get; init; }

    public DateTime LastFetchedUtc { get; set; }
}