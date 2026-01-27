namespace wow_paper_trader.Ingestor.Tests;

using System;
using Xunit;
using wow_paper_trader.Ingestor;


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

    }

    [Fact]
    public void TransitionTo_WhenFailed_SetsFinishedAtUtc()
    {

    }

    [Fact]
    public void MarkFailed_SetsFailedAndStoresErrorInfo()
    {

    }

    [Fact]
    public void MarkFailed_TruncatesErrorFields()
    {

    }
}