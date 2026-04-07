using System.Text.Json.Serialization;

namespace WowPaperTrader.Infrastructure.DTOs;

public sealed class ItemMediaResponseDto
{
    [JsonPropertyName("_links")] public BlizzardLinksDto Links { get; set; } = new();

    [JsonPropertyName("assets")] public List<BlizzardItemMediaAssetDto> Assets { get; set; } = new();

    [JsonPropertyName("id")] public long Id { get; set; }
}

public sealed class BlizzardLinksDto
{
    [JsonPropertyName("self")] public BlizzardHrefDto Self { get; set; } = new();
}

public sealed class BlizzardHrefDto
{
    [JsonPropertyName("href")] public string Href { get; set; } = string.Empty;
}

public sealed class BlizzardItemMediaAssetDto
{
    [JsonPropertyName("key")] public string Key { get; set; } = string.Empty;

    [JsonPropertyName("value")] public string Value { get; set; } = string.Empty;

    [JsonPropertyName("file_data_id")] public int FileDataId { get; set; }
}