using WowPaperTrader.Domain.Features.Write.UpdateItems;

namespace WowPaperTrader.Persistence.EntityMappers;

public static class ItemMetaDataMapper
{
    public static ItemMetaData MapToEntity(ItemMetaDataRecordResponse recordResponse)
    {
        if (recordResponse == null) throw new ArgumentNullException(nameof(recordResponse));

        return new ItemMetaData
        {
            ItemId = recordResponse.ItemId,
            Name = recordResponse.Name,

            QualityType = recordResponse.QualityType,
            QualityName = recordResponse.QualityName,

            Level = recordResponse.Level,
            RequiredLevel = recordResponse.RequiredLevel,

            ItemClassId = recordResponse.ItemClassId,
            ItemClassName = recordResponse.ItemClassName,

            ItemSubclassId = recordResponse.ItemSubclassId,
            ItemSubclassName = recordResponse.ItemSubclassName,

            ProfessionId = recordResponse.ProfessionId,
            ProfessionName = recordResponse.ProfessionName,
            ProfessionSkillLevel = recordResponse.ProfessionSkillLevel,
            SkillDisplayString = recordResponse.SkillDisplayString,

            CraftingReagent = recordResponse.CraftingReagent,

            InventoryType = recordResponse.InventoryType,
            InventoryTypeName = recordResponse.InventoryTypeName,

            PurchasePrice = recordResponse.PurchasePrice,
            SellPrice = recordResponse.SellPrice,

            MaxCount = recordResponse.MaxCount,

            IsEquippable = recordResponse.IsEquippable,
            IsStackable = recordResponse.IsStackable,

            PurchaseQuantity = recordResponse.PurchaseQuantity,

            ImageUrl = recordResponse.ImageUrl,

            LastFetchedUtc = recordResponse.LastFetchedUtc
        };
    }
}