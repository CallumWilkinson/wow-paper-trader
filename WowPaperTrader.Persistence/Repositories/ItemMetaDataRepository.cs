using Microsoft.Extensions.Logging;
using WowPaperTrader.Domain.Interfaces;
using WowPaperTrader.Domain.ResponseTypes;
using WowPaperTrader.Persistence.EntityMappers;

namespace WowPaperTrader.Persistence.Repositories;

public class ItemMetaDataRepository : IItemMetaDataRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ItemMetaDataRepository> _logger;

    public ItemMetaDataRepository(ApplicationDbContext dbContext, ILogger<ItemMetaDataRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SaveItemMetaDataAsync(List<ItemMetaDataRecordResponse> itemMetaDataRecords,
        CancellationToken cancellationToken)
    {
        var startingAdd = DateTime.UtcNow;

        _logger.LogInformation("Adding to DbContext at {Time}", startingAdd);

        foreach (var record in itemMetaDataRecords)
        {
            var itemMetaDataEntity = ItemMetaDataMapper.MapToEntity(record);
            _dbContext.ItemMetaData.Add(itemMetaDataEntity);
        }

        _logger.LogInformation("DbContext Add took {Seconds} Seconds", (DateTime.UtcNow - startingAdd).TotalSeconds);

        var startingSaveToDb = DateTime.UtcNow;

        _logger.LogInformation("Starting SQL Write at {Time}", startingSaveToDb);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("SQL Write took {Seconds} Seconds", (DateTime.UtcNow - startingSaveToDb).TotalSeconds);
    }
}