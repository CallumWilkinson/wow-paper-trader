using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

public sealed class IngestionRunUseCaseTests
{
    [Fact]
    public async Task RunOnceAsync_WhenApiReturnsSnapshot_SavesSnapshotAndDoesNotMarkFailure()
    {
        var repository = new FakeCommodityAuctionRepository();
        var apiAdapter = new FakeCommodityAuctionApiAdapter();

        var useCase = new IngestionRunUseCase(
            NullLogger<IngestionRunUseCase>.Instance,
            apiAdapter,
            repository);

        await useCase.RunOnceAsync(CancellationToken.None);

        Assert.Equal(1, repository.CreateIngestionRunAsyncCallCount);
        Assert.Equal(1, repository.SaveSnapshotAsyncCallCount);
        Assert.Equal(0, repository.MarkRunCancelledAsyncCallCount);
        Assert.Equal(0, repository.MarkRunFailedAsyncCallCount);
    }

    [Fact]
    public async Task RunOnceAsync_WhenOperationIsCancelled_MarksRunAsCancelled()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var repository = new FakeCommodityAuctionRepository();
        var apiAdapter = new FakeCommodityAuctionApiAdapter
        {
            ExceptionToThrow = new OperationCanceledException(cancellationTokenSource.Token)
        };

        var useCase = new IngestionRunUseCase(
            NullLogger<IngestionRunUseCase>.Instance,
            apiAdapter,
            repository);

        await useCase.RunOnceAsync(cancellationTokenSource.Token);

        Assert.Equal(1, repository.CreateIngestionRunAsyncCallCount);
        Assert.Equal(0, repository.SaveSnapshotAsyncCallCount);
        Assert.Equal(1, repository.MarkRunCancelledAsyncCallCount);
        Assert.Equal(0, repository.MarkRunFailedAsyncCallCount);
    }

    [Fact]
    public async Task RunOnceAsync_WhenApiThrowsUnexpectedException_MarksRunAsFailed()
    {
        var repository = new FakeCommodityAuctionRepository();
        var expectedException = new InvalidOperationException("Boom");

        var apiAdapter = new FakeCommodityAuctionApiAdapter
        {
            ExceptionToThrow = expectedException
        };

        var useCase = new IngestionRunUseCase(
            NullLogger<IngestionRunUseCase>.Instance,
            apiAdapter,
            repository);

        await useCase.RunOnceAsync(CancellationToken.None);

        Assert.Equal(1, repository.CreateIngestionRunAsyncCallCount);
        Assert.Equal(0, repository.SaveSnapshotAsyncCallCount);
        Assert.Equal(0, repository.MarkRunCancelledAsyncCallCount);
        Assert.Equal(1, repository.MarkRunFailedAsyncCallCount);
        Assert.Same(expectedException, repository.MarkRunFailedAsyncException);
    }

    private sealed class FakeCommodityAuctionApiAdapter : ICommodityAuctionApiAdapter
    {
        public Exception? ExceptionToThrow { get; set; }

        public CommodityAuctionSnapshot SnapshotToReturn { get; set; } = default!;

        public Task<CommodityAuctionSnapshot> GetCommodityAuctionsSnapshotAsync(
            CancellationToken cancellationToken)
        {
            if (ExceptionToThrow is not null)
            {
                throw ExceptionToThrow;
            }

            return Task.FromResult(SnapshotToReturn);
        }
    }

    private sealed class FakeCommodityAuctionRepository : ICommodityAuctionRepository
    {
        public int CreateIngestionRunAsyncCallCount { get; private set; }

        public int SaveSnapshotAsyncCallCount { get; private set; }

        public int MarkRunFailedAsyncCallCount { get; private set; }

        public int MarkRunCancelledAsyncCallCount { get; private set; }

        public Exception? MarkRunFailedAsyncException { get; private set; }

        private readonly IngestionRun _runToReturn = default!;

        public Task<IngestionRun> CreateIngestionRunAsync(CancellationToken cancellationToken)
        {
            CreateIngestionRunAsyncCallCount++;
            return Task.FromResult(_runToReturn);
        }

        public Task SaveSnapshotAsync(
            IngestionRun run,
            CommodityAuctionSnapshot snapshot,
            CancellationToken cancellationToken)
        {
            SaveSnapshotAsyncCallCount++;
            return Task.CompletedTask;
        }

        public Task MarkRunFailedAsync(IngestionRun run, Exception exception)
        {
            MarkRunFailedAsyncCallCount++;
            MarkRunFailedAsyncException = exception;
            return Task.CompletedTask;
        }

        public Task MarkRunCancelledAsync(IngestionRun run)
        {
            MarkRunCancelledAsyncCallCount++;
            return Task.CompletedTask;
        }
    }
}