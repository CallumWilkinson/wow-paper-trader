using Microsoft.Extensions.Logging;

public sealed class IngestionRunUseCase
{
    private readonly ILogger<IngestionRunUseCase> _logger;
    private readonly ICommodityAuctionApiAdapter _auctionApiAdapter;
    private readonly ICommodityAuctionRepository _repository;

    public IngestionRunUseCase(
        ILogger<IngestionRunUseCase> logger,
        ICommodityAuctionApiAdapter auctionApiAdapter,
        ICommodityAuctionRepository repository)
    {
        _logger = logger;
        _auctionApiAdapter = auctionApiAdapter;
        _repository = repository;
    }

    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        var run = await _repository.CreateIngestionRunAsync(cancellationToken);

        try
        {
            var result = await _auctionApiAdapter.GetCommodityAuctionsSnapshotAsync(cancellationToken);

            await _repository.SaveSnapshotAsync(run, result, cancellationToken);

            _logger.LogInformation("IngestionRun UseCase completed successfully.");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("IngestionRun UseCase Cancelled");
        }
        catch
        {
            _logger.LogInformation("IngestionRun UseCase Failed");
        }
    }
}