using WowPaperTrader.Application.Read.Contracts;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

public static class ItemMetaDataRecordFactory
{

    public static List<ItemMetaDataRecord> CreateRecordsList()
    {
        var fakeItems = new List<ItemMetaDataRecord>
        {
            new ItemMetaDataRecord
            {
                ItemId = 2770,
                Name = "Copper Ore",
                QualityType = "COMMON",
                QualityName = "Common",
                Level = 10,
                RequiredLevel = 0,
                ItemClassId = 7,
                ItemClassName = "Trade Goods",
                ItemSubclassId = 7,
                ItemSubclassName = "Metal & Stone",
                ProfessionId = 186,
                ProfessionName = "Mining",
                ProfessionSkillLevel = 1,
                SkillDisplayString = "Mining (1)",
                CraftingReagent = "Blacksmithing, Engineering",
                InventoryType = null,
                InventoryTypeName = null,
                PurchasePrice = null,
                SellPrice = 25,
                MaxCount = 1000,
                IsEquippable = false,
                IsStackable = true,
                PurchaseQuantity = null,
                ImageUrl = "fake/url/to/image",
                LastFetchedUtc = DateTime.UtcNow
            },

            new ItemMetaDataRecord
            {
                ItemId = 765,
                Name = "Silverleaf",
                QualityType = "COMMON",
                QualityName = "Common",
                Level = 5,
                RequiredLevel = 0,
                ItemClassId = 7,
                ItemClassName = "Trade Goods",
                ItemSubclassId = 9,
                ItemSubclassName = "Herb",
                ProfessionId = 182,
                ProfessionName = "Herbalism",
                ProfessionSkillLevel = 1,
                SkillDisplayString = "Herbalism (1)",
                CraftingReagent = "Alchemy",
                InventoryType = null,
                InventoryTypeName = null,
                PurchasePrice = null,
                SellPrice = 15,
                MaxCount = 1000,
                IsEquippable = false,
                IsStackable = true,
                PurchaseQuantity = null,
                ImageUrl = "fake/url/to/image",
                LastFetchedUtc = DateTime.UtcNow
            },

            new ItemMetaDataRecord
            {
                ItemId = 2447,
                Name = "Peacebloom",
                QualityType = "COMMON",
                QualityName = "Common",
                Level = 5,
                RequiredLevel = 0,
                ItemClassId = 7,
                ItemClassName = "Trade Goods",
                ItemSubclassId = 9,
                ItemSubclassName = "Herb",
                ProfessionId = 182,
                ProfessionName = "Herbalism",
                ProfessionSkillLevel = 1,
                SkillDisplayString = "Herbalism (1)",
                CraftingReagent = "Alchemy",
                InventoryType = null,
                InventoryTypeName = null,
                PurchasePrice = null,
                SellPrice = 15,
                MaxCount = 1000,
                IsEquippable = false,
                IsStackable = true,
                PurchaseQuantity = null,
                ImageUrl = "fake/url/to/image",
                LastFetchedUtc = DateTime.UtcNow
            },

            new ItemMetaDataRecord
            {
                ItemId = 2589,
                Name = "Linen Cloth",
                QualityType = "COMMON",
                QualityName = "Common",
                Level = 10,
                RequiredLevel = 0,
                ItemClassId = 7,
                ItemClassName = "Trade Goods",
                ItemSubclassId = 5,
                ItemSubclassName = "Cloth",
                ProfessionId = 197,
                ProfessionName = "Tailoring",
                ProfessionSkillLevel = 1,
                SkillDisplayString = "Tailoring (1)",
                CraftingReagent = "Tailoring",
                InventoryType = null,
                InventoryTypeName = null,
                PurchasePrice = null,
                SellPrice = 13,
                MaxCount = 1000,
                IsEquippable = false,
                IsStackable = true,
                PurchaseQuantity = null,
                ImageUrl = "fake/url/to/image",
                LastFetchedUtc = DateTime.UtcNow
            },

            new ItemMetaDataRecord
            {
                ItemId = 2318,
                Name = "Light Leather",
                QualityType = "COMMON",
                QualityName = "Common",
                Level = 10,
                RequiredLevel = 0,
                ItemClassId = 7,
                ItemClassName = "Trade Goods",
                ItemSubclassId = 6,
                ItemSubclassName = "Leather",
                ProfessionId = 165,
                ProfessionName = "Leatherworking",
                ProfessionSkillLevel = 1,
                SkillDisplayString = "Leatherworking (1)",
                CraftingReagent = "Leatherworking",
                InventoryType = null,
                InventoryTypeName = null,
                PurchasePrice = null,
                SellPrice = 20,
                MaxCount = 1000,
                IsEquippable = false,
                IsStackable = true,
                PurchaseQuantity = null,
                ImageUrl = "fake/url/to/image",
                LastFetchedUtc = DateTime.UtcNow
            }
        };

        return fakeItems;
    }

    public static List<ItemMetaDataRecord> CreateLinenSearchTestRecords()
    {
        return new List<ItemMetaDataRecord>
        {
            new ItemMetaDataRecord
            {
                ItemId = 2589,
                Name = "Linen Cloth",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Trade Goods",
                ItemSubclassName = "Cloth",
                IsStackable = true,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },
            new ItemMetaDataRecord
            {
                ItemId = 2996,
                Name = "Bolt of Linen Cloth",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Trade Goods",
                ItemSubclassName = "Cloth",
                IsStackable = true,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },
            new ItemMetaDataRecord
            {
                ItemId = 1251,
                Name = "Linen Bandage",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Consumable",
                ItemSubclassName = "Bandage",
                IsStackable = true,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },
            new ItemMetaDataRecord
            {
                ItemId = 2581,
                Name = "Heavy Linen Bandage",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Consumable",
                ItemSubclassName = "Bandage",
                IsStackable = true,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },
            new ItemMetaDataRecord
            {
                ItemId = 10001,
                Name = "Heavy Linen Pants",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Armor",
                ItemSubclassName = "Cloth",
                IsStackable = false,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },
            new ItemMetaDataRecord
            {
                ItemId = 10002,
                Name = "Fine Linen Shirt",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Armor",
                ItemSubclassName = "Cloth",
                IsStackable = false,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },
            new ItemMetaDataRecord
            {
                ItemId = 10003,
                Name = "linen robe", // case variation
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Armor",
                ItemSubclassName = "Cloth",
                IsStackable = false,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },

            // Noise (should NOT match "linen")
            new ItemMetaDataRecord
            {
                ItemId = 2770,
                Name = "Copper Ore",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Trade Goods",
                ItemSubclassName = "Metal & Stone",
                IsStackable = true,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },
            new ItemMetaDataRecord
            {
                ItemId = 765,
                Name = "Silverleaf",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Trade Goods",
                ItemSubclassName = "Herb",
                IsStackable = true,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            },
            new ItemMetaDataRecord
            {
                ItemId = 2447,
                Name = "Peacebloom",
                QualityType = "COMMON",
                QualityName = "Common",
                ItemClassName = "Trade Goods",
                ItemSubclassName = "Herb",
                IsStackable = true,
                ImageUrl = "fake/url",
                LastFetchedUtc = DateTime.UtcNow
            }
        };
    }
}