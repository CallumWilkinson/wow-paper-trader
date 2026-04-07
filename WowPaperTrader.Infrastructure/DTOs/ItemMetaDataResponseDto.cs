using System.Text.Json.Serialization;

namespace WowPaperTrader.Infrastructure.DTOs;

public sealed class ItemMetaDataResponseDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("quality")]
    public BlizzardTypeNameDto? Quality { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("required_level")]
    public int RequiredLevel { get; set; }

    [JsonPropertyName("item_class")]
    public BlizzardItemClassDto? ItemClass { get; set; }

    [JsonPropertyName("item_subclass")]
    public BlizzardItemSubclassDto? ItemSubclass { get; set; }

    [JsonPropertyName("inventory_type")]
    public BlizzardTypeNameDto? InventoryType { get; set; }

    [JsonPropertyName("purchase_price")]
    public long? PurchasePrice { get; set; }

    [JsonPropertyName("sell_price")]
    public long? SellPrice { get; set; }

    [JsonPropertyName("max_count")]
    public int? MaxCount { get; set; }

    [JsonPropertyName("is_equippable")]
    public bool IsEquippable { get; set; }

    [JsonPropertyName("is_stackable")]
    public bool IsStackable { get; set; }

    [JsonPropertyName("purchase_quantity")]
    public int? PurchaseQuantity { get; set; }

    [JsonPropertyName("preview_item")]
    public BlizzardPreviewItemDto? PreviewItem { get; set; }
}

public sealed class BlizzardPreviewItemDto
{
    [JsonPropertyName("requirements")]
    public BlizzardRequirementsDto? Requirements { get; set; }

    [JsonPropertyName("crafting_reagent")]
    public string? CraftingReagent { get; set; }
}

public sealed class BlizzardRequirementsDto
{
    [JsonPropertyName("skill")]
    public BlizzardSkillDto? Skill { get; set; }
}

public sealed class BlizzardSkillDto
{
    [JsonPropertyName("profession")]
    public BlizzardProfessionDto? Profession { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [JsonPropertyName("display_string")]
    public string? DisplayString { get; set; }
}

public sealed class BlizzardProfessionDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }
}

public sealed class BlizzardTypeNameDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public sealed class BlizzardItemClassDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }
}

public sealed class BlizzardItemSubclassDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }
}