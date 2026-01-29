namespace wow_paper_trader.Ingestor.Tests;

using System;
using Xunit;



public sealed class IngestionRunTests
{
    [Fact]
    public void TransitionTo_UpdatesStatusAndLastUpdated()
    {
        var run = new IngestionRun();
        var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        run.TransitionTo(IngestionRunStatus.TokenRequested, now);

        Assert.Equal(IngestionRunStatus.TokenRequested, run.Status);
        Assert.Equal(now, run.LastUpdatedAtUtc);
    }

    [Fact]
    public void TransitionTo_WhenFinished_SetsFinishedAtUtc()
    {
        var run = new IngestionRun();
        var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        run.TransitionTo(IngestionRunStatus.Finished, now);

        Assert.Equal(IngestionRunStatus.Finished, run.Status);
        Assert.Equal(now, run.LastUpdatedAtUtc);
        Assert.Equal(now, run.FinishedAtUtc);

    }

    [Fact]
    public void TransitionTo_WhenFailed_SetsFinishedAtUtc()
    {
        var run = new IngestionRun();
        var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        run.TransitionTo(IngestionRunStatus.Failed, now);

        Assert.Equal(IngestionRunStatus.Failed, run.Status);
        Assert.Equal(now, run.LastUpdatedAtUtc);
        Assert.Equal(now, run.FinishedAtUtc);
    }

    [Fact]
    public void MarkFailed_SetsFailedAndStoresErrorInfo()
    {
        var run = new IngestionRun();
        var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var ex = new InvalidOperationException("Boom");

        run.MarkFailed(ex, now);

        Assert.Equal(IngestionRunStatus.Failed, run.Status);
        Assert.Equal(now, run.FinishedAtUtc);
        Assert.NotNull(run.ErrorMessage);
        Assert.Contains("Boom", run.ErrorMessage!, StringComparison.Ordinal);
        Assert.NotNull(run.ErrorStack);
        Assert.Contains("InvalidOperationException", run.ErrorStack!, StringComparison.Ordinal);

    }
}