# wow-paper-trader

`wow-paper-trader` is a .NET solution for ingesting World of Warcraft commodity auction data from Blizzard's API and building the backend foundation for later analysis and paper-trading features.

The repository is currently centered on one implemented backend path:

- authenticate against Battle.net
- fetch the latest US commodity auction snapshot
- persist that snapshot to SQL Server
- query the latest snapshot for the current lowest unit price of an item

The broader product direction in [Requirements.md](./Requirements.md) is still larger than the codebase today. This README reflects the current implementation rather than the long-term plan.

## Current Status

Implemented today:

- background worker that ingests Blizzard US commodity auction snapshots once per hour
- SQL Server persistence with EF Core migrations
- ingestion run tracking with `Started`, `Finished`, `Failed`, and `Cancelled` states
- query layer for "lowest unit price for an item from the latest snapshot"
- persistence integration tests covering snapshot persistence and latest-snapshot price querying

Not implemented yet:

- public API endpoints for auction data
- write API endpoints
- paper-trading, portfolios, auth, or user accounts
- retention cleanup for old Blizzard data
- configurable ingestion interval

Two ASP.NET Core API projects exist, but both still expose the default template `weatherforecast` endpoint and OpenAPI in development.

## Solution Layout

- `wow-paper-trader.Ingestor`
  - worker service
  - configures EF Core, HTTP clients, and the ingestion loop
- `wow-paper-trader.Application.Write`
  - write-side use case and domain entities for ingestion
- `wow-paper-trader.Application.Read`
  - read-side use case and query contract for current lowest item price
- `wow-paper-trader.Infrastructure`
  - Blizzard OAuth client, commodity auction HTTP client, DTOs, and mapping
- `wow-paper-trader.Persistence`
  - `ApplicationDbContext`, EF migrations, repository, and Dapper query
- `wow-paper-trader.Api.Read`
  - read API host scaffold, not wired to the read use case yet
- `wow-paper-trader.Api.Write`
  - write API host scaffold, not wired to application services yet
- `wow-paper-trader.Persistence.Tests`
  - integration tests using in-memory SQLite
- `wow-paper-trader.Application.Read.Tests`
  - test project exists but currently has no discovered tests
- `wow-paper-trader.Application.Write.Tests`
  - test project exists but currently has no discovered tests

## Implemented Data Flow

1. `IngestionRunBackgroundService` starts an ingestion pass every hour.
2. `BattleNetAuthClient` requests an OAuth access token using Blizzard client credentials.
3. `CommodityAuctionClient` calls Blizzard's US commodities endpoint:
   - base URL: `https://us.api.blizzard.com/data/wow/`
   - endpoint suffix: `auctions/commodities?namespace=dynamic-us&locale=en_US`
4. `CommodityAuctionApiAdapter` maps the API response into application contracts.
5. `CommodityAuctionRepository` writes the snapshot and all auction rows in a database transaction.
6. `CurrentLowestUnitPriceQuery` can read the minimum `UnitPrice` for an `ItemId` from the most recent snapshot in the database.

## Data Stored

The initial migration creates three tables:

- `IngestionRuns`
  - lifecycle and error tracking for each ingestion attempt
- `CommodityAuctionSnapshots`
  - snapshot metadata such as fetch timestamp and source endpoint
- `CommodityAuctions`
  - raw commodity auction rows for a snapshot

Stored auction fields currently include:

- `ItemId`
- `Quantity`
- `UnitPrice`
- `TimeLeft`

## Local Setup

### Prerequisites

- .NET 10 SDK
- SQL Server instance accessible from your machine
- Blizzard API client credentials

### Configure Blizzard credentials

The ingestor reads:

- `Blizzard:ClientId`
- `Blizzard:ClientSecret`

Using user secrets:

```powershell
dotnet user-secrets set "Blizzard:ClientId" "<your-client-id>" --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
dotnet user-secrets set "Blizzard:ClientSecret" "<your-client-secret>" --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
```

### Configure the database connection

The ingestor reads `ConnectionStrings:WowPaperTrader`.

You can set it in `wow-paper-trader.Ingestor\appsettings.Development.json` or via an environment variable such as:

```powershell
$env:ConnectionStrings__WowPaperTrader="Server=localhost;Database=WowPaperTrader;Trusted_Connection=True;TrustServerCertificate=True;"
```

### Apply the database migration

If `dotnet-ef` is not installed:

```powershell
dotnet tool install --global dotnet-ef
```

Then update the database:

```powershell
dotnet ef database update --project .\wow-paper-trader.Persistence\wow-paper-trader.Persistence.csproj --startup-project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
```

## Running the Projects

Run the ingestor:

```powershell
dotnet run --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
```

Run the read API scaffold:

```powershell
dotnet run --project .\wow-paper-trader.Api.Read\wow-paper-trader.Api.Read.csproj
```

Default development URLs:

- `https://localhost:7033`
- `http://localhost:5091`

Run the write API scaffold:

```powershell
dotnet run --project .\wow-paper-trader.Api.Write\wow-paper-trader.Api.Write.csproj
```

Default development URLs:

- `https://localhost:7002`
- `http://localhost:5211`

## Tests

Run the full solution test suite:

```powershell
dotnet test wow-paper-trader.sln
```

Current observed result in this repository:

- `wow-paper-trader.Persistence.Tests`: 2 passing integration tests
- `wow-paper-trader.Application.Read.Tests`: project builds, no discovered tests
- `wow-paper-trader.Application.Write.Tests`: project builds, no discovered tests

## Notes and Gaps

- The ingestion interval is hard-coded to one hour in `IngestionRunBackgroundService`.
- The current implementation is US commodities only.
- The read and write APIs are not yet connected to the application and persistence layers.
- The 30-day Blizzard retention requirement described in `Requirements.md` is not yet enforced in code.

## Disclaimer

This is a personal educational project and is not affiliated with or endorsed by Blizzard Entertainment. World of Warcraft and Blizzard Entertainment are trademarks or registered trademarks of Blizzard Entertainment, Inc.

## License

No license file is present in the repository. All rights are reserved by default.
