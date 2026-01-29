using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public sealed class IngestionRunPersistenceTests
{
    [Fact]
    public async Task CanPersistAndReloadIngestionRun()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        //EF Core setup
        var options = new DbContextOptionsBuilder<IngestorDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var db = new IngestorDbContext(options))
        {
            //create schema (translates model into SQL) and creates tables
            await db.Database.EnsureCreatedAsync();
        }

        //id for row
        var runId = Guid.Empty;

        await using (var db = new IngestorDbContext(options))
        {
            var run = new IngestionRun();
            run.TransitionTo(IngestionRunStatus.TokenRequested, DateTime.UtcNow);

            db.IngestionRuns.Add(run);
            await db.SaveChangesAsync();

            runId = run.Id;
        }

        await using (var db = new IngestorDbContext(options))
        {
            var loaded = await db.IngestionRuns.SingleAsync(x => x.Id == runId);
            Assert.Equal(IngestionRunStatus.TokenRequested, loaded.Status);

        }
    }
}