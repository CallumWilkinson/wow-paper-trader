using WowPaperTrader.Domain.Contracts;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

public static class ItemSearchResultFactory
{
    public static List<ItemSearchResult> CreateExpectedLinenResults()
    {
        return new List<ItemSearchResult>
        {
            // Exact match
            new ItemSearchResult { ItemId = 2589, Name = "Linen Cloth" },

            // Starts with "linen"
            new ItemSearchResult { ItemId = 1251, Name = "Linen Bandage" },

            // Contains "linen"
            new ItemSearchResult { ItemId = 10001, Name = "Heavy Linen Pants" },
            new ItemSearchResult { ItemId = 10002, Name = "Fine Linen Shirt" },
            new ItemSearchResult { ItemId = 10003, Name = "linen robe" }
        };
    }
}