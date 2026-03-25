public static class ItemMetaDataRecordMapper
{
    public static ItemMetaDataRecord MapToContract(
        ItemMetaDataResponseDto dto,
        DateTime lastFetchedUtc)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new ItemMetaDataRecord
        {
            ItemId = dto.Id,
            Name = dto.Name,
            QualityType = dto.Quality?.Type,
            QualityName = dto.Quality?.Name,
            Level = dto.Level,
            RequiredLevel = dto.RequiredLevel,
            ItemClassId = dto.ItemClass?.Id,
            ItemClassName = dto.ItemClass?.Name,
            ItemSubclassId = dto.ItemSubclass?.Id,
            ItemSubclassName = dto.ItemSubclass?.Name,
            ProfessionId = dto.PreviewItem?.Requirements?.Skill?.Profession?.Id,
            ProfessionName = dto.PreviewItem?.Requirements?.Skill?.Profession?.Name,
            ProfessionSkillLevel = dto.PreviewItem?.Requirements?.Skill?.Level,
            SkillDisplayString = dto.PreviewItem?.Requirements?.Skill?.DisplayString,
            CraftingReagent = dto.PreviewItem?.CraftingReagent,
            InventoryType = dto.InventoryType?.Type,
            InventoryTypeName = dto.InventoryType?.Name,
            PurchasePrice = dto.PurchasePrice,
            SellPrice = dto.SellPrice,
            MaxCount = dto.MaxCount,
            IsEquippable = dto.IsEquippable,
            IsStackable = dto.IsStackable,
            PurchaseQuantity = dto.PurchaseQuantity,
            LastFetchedUtc = lastFetchedUtc
        };
    }
}