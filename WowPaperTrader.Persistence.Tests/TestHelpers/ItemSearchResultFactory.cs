using WowPaperTrader.Application.Features.Read.ItemSearch;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

public static class ItemSearchResultFactory
{
    public static List<ItemSearchResponse> CreateExpectedLinenResults()
    {
        return new List<ItemSearchResponse>
        {
            // Exact match
            new() { ItemId = 2589, Name = "Linen Cloth", ImageUrl = "fake/url" },

            // Starts with "linen"
            new() { ItemId = 1251, Name = "Linen Bandage", ImageUrl = "fake/url" },

            // Contains "linen"
            new() { ItemId = 10001, Name = "Heavy Linen Pants", ImageUrl = "fake/url" },
            new() { ItemId = 10002, Name = "Fine Linen Shirt",  ImageUrl = "fake/url" },
            new() { ItemId = 10003, Name = "linen robe", ImageUrl = "fake/url"}
        };
    }
}