namespace WowPaperTrader.Ingestor;

public static class RunAuctionsAndMetadata
{
    public static async Task<int> RunAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        var auctionJob = serviceProvider.GetRequiredService<AuctionDataIngestionJob>();
        
        var auctionExitCode = await auctionJob.RunAsync(stoppingToken);
        
        if (auctionExitCode != 0) return auctionExitCode;
        
        var metadataJob = serviceProvider.GetRequiredService<UpdateItemMetadataJob>();
        
        return await metadataJob.RunAsync(stoppingToken);
    }
}