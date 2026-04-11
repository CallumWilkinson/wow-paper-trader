using Microsoft.Extensions.Logging;
using WowPaperTrader.Domain.Features.Write.UpdateItems;
using WowPaperTrader.Persistence.EntityMappers;

namespace WowPaperTrader.Persistence.Repositories;

public class ItemMetadataRepository : IItemMetadataRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ItemMetadataRepository> _logger;

    public ItemMetadataRepository(ApplicationDbContext dbContext, ILogger<ItemMetadataRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SaveItemMetaDataAsync(List<ItemMetadataRecord> itemMetaDataRecords,
        CancellationToken cancellationToken)
    {
        var startingAdd = DateTime.UtcNow;

        _logger.LogInformation("Adding to DbContext at {Time}", startingAdd);

        foreach (var record in itemMetaDataRecords)
        {
            var itemMetaDataEntity = ItemMetadataMapper.MapToEntity(record);
            _dbContext.ItemMetaData.Add(itemMetaDataEntity);
        }

        _logger.LogInformation("DbContext Add took {Seconds} Seconds", (DateTime.UtcNow - startingAdd).TotalSeconds);

        var startingSaveToDb = DateTime.UtcNow;

        _logger.LogInformation("Starting SQL Write at {Time}", startingSaveToDb);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("SQL Write took {Seconds} Seconds", (DateTime.UtcNow - startingSaveToDb).TotalSeconds);
    }
}