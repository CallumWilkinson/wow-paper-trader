using WowPaperTrader.Application.Read.Contracts;
using WowPaperTrader.Application.Read.Entities;

namespace WowPaperTrader.Persistence.EntityMappers;

public static class ItemMetaDataMapper
{
    public static ItemMetaData MapToEntity(ItemMetaDataRecord record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        return new ItemMetaData
        {
            ItemId = record.ItemId,
            Name = record.Name,

            QualityType = record.QualityType,
            QualityName = record.QualityName,

            Level = record.Level,
            RequiredLevel = record.RequiredLevel,

            ItemClassId = record.ItemClassId,
            ItemClassName = record.ItemClassName,

            ItemSubclassId = record.ItemSubclassId,
            ItemSubclassName = record.ItemSubclassName,

            ProfessionId = record.ProfessionId,
            ProfessionName = record.ProfessionName,
            ProfessionSkillLevel = record.ProfessionSkillLevel,
            SkillDisplayString = record.SkillDisplayString,

            CraftingReagent = record.CraftingReagent,

            InventoryType = record.InventoryType,
            InventoryTypeName = record.InventoryTypeName,

            PurchasePrice = record.PurchasePrice,
            SellPrice = record.SellPrice,

            MaxCount = record.MaxCount,

            IsEquippable = record.IsEquippable,
            IsStackable = record.IsStackable,

            PurchaseQuantity = record.PurchaseQuantity,

            ImageUrl = record.ImageUrl,

            LastFetchedUtc = record.LastFetchedUtc
        };
    }
}