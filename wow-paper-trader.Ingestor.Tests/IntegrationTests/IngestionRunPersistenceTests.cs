using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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

        //id for row in db (one ingestion run) so we can reload the entity in the assertion below
        long runId;

        await using (var dbContext = _db.CreateDbContext())
        {
            var run = new IngestionRun();
            run.TransitionTo(IngestionRunStatus.TokenRequested, DateTime.UtcNow);

            dbContext.IngestionRuns.Add(run);
            await dbContext.SaveChangesAsync();

            runId = run.Id;
        }

        await using (var dbContext = _db.CreateDbContext())
        {
            var loaded = await dbContext.IngestionRuns.SingleAsync(x => x.Id == runId);
            Assert.Equal(IngestionRunStatus.TokenRequested, loaded.Status);

        }
    }
}