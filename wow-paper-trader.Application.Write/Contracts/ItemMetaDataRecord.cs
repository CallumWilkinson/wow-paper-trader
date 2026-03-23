public sealed record ItemMetaDataRecord
{
    long ItemId;
    string Name;
    string SelfHref;
    string? QualityType;
    string? QualityName;
    int Level;
    int RequiredLevel;
    long? MediaId;
    string? MediaHref;
    int? ItemClassId;
    string? ItemClassName;
    int? ItemSubclassId;
    string? ItemSubclassName;
    string? InventoryType;
    string? InventoryTypeName;
    long? PurchasePrice;
    long? SellPrice;
    int? MaxCount;
    bool IsEquippable;
    bool IsStackable;
    int? PurchaseQuantity;
    string RawJson;
    DateTime LastFetchedUtc;
}