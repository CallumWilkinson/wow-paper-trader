using System.Text.Json.Serialization;

public sealed class CommodityAuctionsResponseDto
{
    [JsonPropertyName("auctions")]
    public required List<CommodityAuctionDto> CommodityAuctions { get; init; }
}

public sealed class CommodityAuctionDto
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("item")]
    public required CommodityAuctionItemDto Item { get; init; }

    [JsonPropertyName("quanity")]
    public int Quanity { get; init; }

    [JsonPropertyName("unit_price")]
    public int UnitPrice { get; init; }

    [JsonPropertyName("time_left")]
    public required string TimeLeft { get; init; }
}

public sealed class CommodityAuctionItemDto
{
    [JsonPropertyName("id")]
    public long Id { get; init; }
}