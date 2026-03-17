# wow-paper-trader

`wow-paper-trader` is a .NET 10 solution for ingesting World of Warcraft commodity auction data from Blizzard's API, persisting full snapshots to SQL Server, and exposing a small read API over the latest stored snapshot.

The long-term product direction in [Requirements.md](./Requirements.md) is much larger than the codebase today. This README is intentionally scoped to the repository as it exists on March 17, 2026.

## Current Project State

Implemented:

- worker service that starts an ingestion run immediately on startup, then repeats every hour
- Battle.net client-credentials OAuth flow for Blizzard API access
- ingestion of the US commodities snapshot from Blizzard's WoW data API
- SQL Server persistence via EF Core for ingestion runs, snapshots, and auction rows
- transactional snapshot writes to avoid partial auction persistence
- read API endpoint for "lowest unit price for an item from the latest stored snapshot"
- integration tests for snapshot persistence and latest-snapshot querying

Partially implemented:

- `wow-paper-trader.Api.Read` is wired to the read use case and database
- `wow-paper-trader.Api.Write` still contains the default template `weatherforecast` endpoint only

Not implemented yet:

- automated 30-day retention cleanup
- configurable ingestion interval
- historical price APIs beyond "latest snapshot lowest unit price"
- item search, aggregated analytics, or charting APIs
- user accounts, auth, portfolios, or paper-trading features
- retry/backoff handling for Blizzard API failures or rate limits

## Solution Layout

- `wow-paper-trader.Ingestor`
  - worker host
  - configures EF Core, HTTP clients, and the hourly ingestion loop
- `wow-paper-trader.Application.Write`
  - ingestion use case, write-side entities, and contracts
- `wow-paper-trader.Application.Read`
  - read-side use case and query contract for current lowest unit price
- `wow-paper-trader.Infrastructure`
  - Blizzard auth client, commodity auction client, DTOs, and contract mapping
- `wow-paper-trader.Persistence`
  - EF Core `ApplicationDbContext`, migration, repository, and Dapper query
- `wow-paper-trader.Api.Read`
  - ASP.NET Core API exposing the current lowest unit price endpoint
- `wow-paper-trader.Api.Write`
  - ASP.NET Core API scaffold with the default template endpoint
- `wow-paper-trader.Persistence.Tests`
  - integration tests using in-memory SQLite
- `wow-paper-trader.Application.Read.Tests`
  - test project scaffold; currently no test source files
- `wow-paper-trader.Application.Write.Tests`
  - test project scaffold; currently no test source files

## Implemented Data Flow

### Ingestion

1. `IngestionRunBackgroundService` creates a scope and executes `IngestionRunUseCase.RunOnceAsync()`.
2. `BattleNetAuthClient` requests or reuses an OAuth access token from `https://oauth.battle.net/token`.
3. `CommodityAuctionClient` calls Blizzard's US commodities endpoint using:
   - base URL: `https://us.api.blizzard.com/data/wow/`
   - endpoint suffix: `auctions/commodities?namespace=dynamic-us&locale=en_US`
4. `CommodityAuctionApiAdapter` maps the DTO response into application contracts.
5. `CommodityAuctionRepository` writes the ingestion run and snapshot data to SQL Server in a transaction.
6. On success, the ingestion run transitions to `Finished`. On errors, it is marked `Failed`. If shutdown happens during persistence, it is marked `Cancelled`.

### Read Path

1. `GET /api/commodities/{itemId}/current-lowest-unit-price` hits `CommoditiesController`.
2. `GetCurrentLowestUnitPriceByItemIdUseCase` validates `itemId`.
3. `CurrentLowestUnitPriceQuery` uses Dapper against the application database.
4. The query returns the minimum `UnitPrice` for that `ItemId` from the most recent stored snapshot only.

## API Surface

### Read API

Project: `wow-paper-trader.Api.Read`

Implemented endpoint:

- `GET /api/commodities/{itemId}/current-lowest-unit-price`
  - `400 Bad Request` if `itemId <= 0`
  - `404 Not Found` if the latest snapshot does not contain that item
  - `200 OK` response body:

```json
{
  "itemId": 2770,
  "unitPrice": 80,
  "priceTakenAtUtc": "2026-03-12T09:00:00Z"
}
```

Development-only tooling:

- Swagger is enabled in development

### Write API

Project: `wow-paper-trader.Api.Write`

Current state:

- still the default template host
- exposes `GET /weatherforecast`
- maps the development OpenAPI document
- does not yet register application, persistence, or Blizzard integration services

## Data Stored

The initial EF Core migration creates three tables:

- `IngestionRuns`
  - `StartedAtUtc`
  - `LastUpdatedAtUtc`
  - `FinishedAtUtc`
  - `Status`
  - `ErrorMessage`
  - `ErrorStack`
- `CommodityAuctionSnapshots`
  - `IngestionRunId`
  - `FetchedAtUtc`
  - `ApiEndPoint`
- `CommodityAuctions`
  - `CommodityAuctionSnapshotId`
  - `ItemId`
  - `Quantity`
  - `UnitPrice`
  - `TimeLeft`

Important scope notes:

- the current implementation stores commodity auction snapshots only
- commodities are region-wide data; the current schema does not store realm-specific auction information
- the schema does not yet store item names, historical aggregates, portfolios, or user data

## Local Setup

### Prerequisites

- .NET 10 SDK
- SQL Server instance accessible from your machine
- Blizzard API client credentials

### Configure Blizzard credentials

The ingestor requires:

- `Blizzard:ClientId`
- `Blizzard:ClientSecret`

Example using user secrets:

```powershell
dotnet user-secrets set "Blizzard:ClientId" "<your-client-id>" --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
dotnet user-secrets set "Blizzard:ClientSecret" "<your-client-secret>" --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
```

### Configure the database connection

Both the ingestor and the read API require `ConnectionStrings:WowPaperTrader`.

Example PowerShell session:

```powershell
$env:ConnectionStrings__WowPaperTrader="Server=localhost;Database=WowPaperTrader;Trusted_Connection=True;TrustServerCertificate=True;"
```

Development convenience files also exist at:

- `wow-paper-trader.Ingestor/appsettings.Development.json`
- `wow-paper-trader.Api.Read/appsettings.Development.json`

### Apply the database migration

If `dotnet-ef` is not installed:

```powershell
dotnet tool install --global dotnet-ef
```

Apply the current migration:

```powershell
dotnet ef database update --project .\wow-paper-trader.Persistence\wow-paper-trader.Persistence.csproj --startup-project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
```

## Running the Projects

Run the ingestor:

```powershell
dotnet run --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
```

Behavior:

- one ingestion run starts immediately when the worker launches
- subsequent runs happen every hour

Run the read API:

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

Current verified result on March 17, 2026:

- `wow-paper-trader.Persistence.Tests`: 2 passing integration tests
- `wow-paper-trader.Application.Read.Tests`: builds, but no tests are discovered
- `wow-paper-trader.Application.Write.Tests`: builds, but no tests are discovered

## Known Gaps

- retention cleanup is not automated; a manual helper script exists at `manual_delete_data_past_30_days_sql_script.txt`
- the ingestion interval is hard-coded to one hour in `IngestionRunBackgroundService`
- the current Blizzard integration is fixed to the US commodities endpoint
- the read API only answers against the latest stored snapshot, not arbitrary history
- the write API is still scaffold-level

## Disclaimer

This is a personal educational project and is not affiliated with or endorsed by Blizzard Entertainment. World of Warcraft and Blizzard Entertainment are trademarks or registered trademarks of Blizzard Entertainment, Inc.

## License

No license file is present in the repository. All rights are reserved by default.
