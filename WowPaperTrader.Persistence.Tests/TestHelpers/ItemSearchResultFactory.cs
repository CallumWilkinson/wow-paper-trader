using WowPaperTrader.Application.Features.Read.ItemSearch;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

public static class ItemSearchResultFactory
{
    public static List<ItemSearchResponse> CreateExpectedLinenResults()
    {
        return new List<ItemSearchResponse>
        {
            // Exact match
            new() { ItemId = 2589, Name = "Linen Cloth" },

            // Starts with "linen"
            new() { ItemId = 1251, Name = "Linen Bandage" },

            // Contains "linen"
            new() { ItemId = 10001, Name = "Heavy Linen Pants" },
            new() { ItemId = 10002, Name = "Fine Linen Shirt" },
            new() { ItemId = 10003, Name = "linen robe" }
        };
    }
}