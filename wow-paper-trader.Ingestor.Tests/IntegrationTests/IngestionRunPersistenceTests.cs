using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace wow_paper_trader.Ingestor.Tests;

public sealed class IngestionRunPersistenceTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public IngestionRunPersistenceTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task CanPersistAndReloadIngestionRun()
    {
        //arrange
        await using var arrangeDbContext = await _db.CreateArrangeDbContextAsync();
        var run = new IngestionRun();
        run.TransitionTo(IngestionRunStatus.TokenRequested, DateTime.UtcNow);
        long runId = run.Id;

        //act
        arrangeDbContext.IngestionRuns.Add(run);
        await arrangeDbContext.SaveChangesAsync();


        //assert
        await using var assertDbContext = _db.CreateAssertDbContext();
        var loaded = await assertDbContext.IngestionRuns.SingleAsync();
        Assert.Equal(IngestionRunStatus.TokenRequested, loaded.Status);


    }
}