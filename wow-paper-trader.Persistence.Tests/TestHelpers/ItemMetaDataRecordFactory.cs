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
}