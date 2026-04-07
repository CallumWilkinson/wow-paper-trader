using WowPaperTrader.Domain.Contracts;
using WowPaperTrader.Infrastructure.DTOs;

namespace WowPaperTrader.Infrastructure.ContractMappers;

public static class ItemMetaDataRecordMapper
{
    public static ItemMetaDataRecord MapToContract(
        ItemMetaDataResponseDto metadataDto,
        ItemMediaResponseDto mediaDto,
        DateTime lastFetchedUtc)
    {
        if (metadataDto == null || mediaDto == null) throw new ArgumentNullException(nameof(metadataDto));

        return new ItemMetaDataRecord
        {
            ItemId = metadataDto.Id,
            Name = metadataDto.Name,
            QualityType = metadataDto.Quality?.Type,
            QualityName = metadataDto.Quality?.Name,
            Level = metadataDto.Level,
            RequiredLevel = metadataDto.RequiredLevel,
            ItemClassId = metadataDto.ItemClass?.Id,
            ItemClassName = metadataDto.ItemClass?.Name,
            ItemSubclassId = metadataDto.ItemSubclass?.Id,
            ItemSubclassName = metadataDto.ItemSubclass?.Name,
            ProfessionId = metadataDto.PreviewItem?.Requirements?.Skill?.Profession?.Id,
            ProfessionName = metadataDto.PreviewItem?.Requirements?.Skill?.Profession?.Name,
            ProfessionSkillLevel = metadataDto.PreviewItem?.Requirements?.Skill?.Level,
            SkillDisplayString = metadataDto.PreviewItem?.Requirements?.Skill?.DisplayString,
            CraftingReagent = metadataDto.PreviewItem?.CraftingReagent,
            InventoryType = metadataDto.InventoryType?.Type,
            InventoryTypeName = metadataDto.InventoryType?.Name,
            PurchasePrice = metadataDto.PurchasePrice,
            SellPrice = metadataDto.SellPrice,
            MaxCount = metadataDto.MaxCount,
            IsEquippable = metadataDto.IsEquippable,
            IsStackable = metadataDto.IsStackable,
            PurchaseQuantity = metadataDto.PurchaseQuantity,
            ImageUrl = GetIconUrl(mediaDto),
            LastFetchedUtc = lastFetchedUtc
        };
    }

    private static string? GetIconUrl(ItemMediaResponseDto dto)
    {
        foreach (var asset in dto.Assets)
        {
            var isIcon = asset.Key == "icon";
            var hasValue = !string.IsNullOrWhiteSpace(asset.Value);

            if (isIcon && hasValue) return asset.Value;
        }

        return null;
    }
}