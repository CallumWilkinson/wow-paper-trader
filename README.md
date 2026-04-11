# WowPaperTrader

`WowPaperTrader` is a .NET 10 solution for collecting World of Warcraft commodity auction data from Blizzard's API, storing auction snapshots in SQL Server, enriching known item ids with Blizzard item metadata, and exposing a small REST API for item search, metadata, and current lowest auction price queries.

The long-term product direction in [Requirements.md](./Requirements.md) is larger than the codebase today. This README describes the repository as it exists on April 11, 2026.

## Current Project State

Implemented:

- hourly background ingestion of the US commodity auction snapshot
- Battle.net client-credentials OAuth flow with token reuse
- Blizzard API clients for commodity auctions, item metadata, and item media
- SQL Server persistence with EF Core migrations
- transactional snapshot writes for auction ingestion
- item metadata storage, including icon image URLs from the media endpoint
- CQRS-style application layer with explicit command/query contracts and handlers
- Dapper-based read services for query-oriented API responses
- REST API under `/api/v1/items`
- persistence integration tests using SQLite fixtures

Partially implemented:

- item metadata refresh is exposed as a manual API command endpoint, not automated
- read APIs query the latest auction snapshot only
- CQRS boundaries are present in code, but handler registration is still manual in host projects

Not implemented yet:

- automated 30-day retention cleanup
- configurable ingestion interval
- historical price APIs beyond latest-snapshot reads
- realm-specific auction data
- user accounts, authentication, portfolios, or paper-trading workflows
- retry/backoff policies for Blizzard API failures or rate limits

## Solution Layout

- `WowPaperTrader.Api`
  - ASP.NET Core REST API
  - registers query handlers, read services, metadata update command handler, EF Core, Swagger, and Blizzard item clients
  - exposes all current HTTP endpoints through `ItemsController`
- `WowPaperTrader.Ingestor`
  - worker service
  - starts an auction ingestion command immediately, then repeats every hour
  - registers EF Core, the auction ingestion command handler, the Blizzard auction adapter, and repository
- `WowPaperTrader.Application`
  - application contracts and use cases
  - contains `ICommand`, `ICommandHandler<TCommand>`, `IQuery<TResponse>`, and `IQueryHandler<TQuery, TResponse>`
  - organizes features into `Features/Read` and `Features/Write`
- `WowPaperTrader.Infrastructure`
  - Blizzard OAuth and API HTTP clients
  - adapter implementations for application-facing API contracts
  - DTO-to-application contract mappers
- `WowPaperTrader.Persistence`
  - EF Core `ApplicationDbContext`
  - migrations, write repositories, entity mappers, and Dapper read services
- `WowPaperTrader.Application.Tests`
  - application test project scaffold
- `WowPaperTrader.Persistence.Tests`
  - integration tests for repositories, read services, and schema behavior

## Dependency Graph

Project references:

```text
WowPaperTrader.Api
  -> WowPaperTrader.Application
  -> WowPaperTrader.Infrastructure
  -> WowPaperTrader.Persistence

WowPaperTrader.Ingestor
  -> WowPaperTrader.Infrastructure
  -> WowPaperTrader.Persistence

WowPaperTrader.Infrastructure
  -> WowPaperTrader.Application

WowPaperTrader.Persistence
  -> WowPaperTrader.Application

WowPaperTrader.Application.Tests
  -> WowPaperTrader.Application

WowPaperTrader.Persistence.Tests
  -> WowPaperTrader.Persistence
```

Architectural direction:

```text
Hosts
  Api / Ingestor
    -> Application
       -> contracts for reads, writes, and external services
    -> Infrastructure
       -> Blizzard HTTP adapters implementing application contracts
    -> Persistence
       -> SQL Server repositories and read services implementing application contracts
```

`Application` is the center of the solution. It owns feature contracts and use-case orchestration, while `Infrastructure` and `Persistence` depend inward on those contracts. The host projects compose the concrete implementations.

## CQRS Focus

The solution is intentionally organized around commands and queries:

- queries implement `IQuery<TResponse>` and are handled by `IQueryHandler<TQuery, TResponse>`
- commands implement `ICommand` and are handled by `ICommandHandler<TCommand>`
- read features live under `WowPaperTrader.Application/Features/Read`
- write features live under `WowPaperTrader.Application/Features/Write`
- read services are application contracts implemented in `WowPaperTrader.Persistence/ReadServices`
- write repositories are application contracts implemented in `WowPaperTrader.Persistence/Repositories`

Current query features:

- `ItemSearchQuery`
  - searches `ItemMetaData` by name and returns the top five matches
- `GetMetadataQuery`
  - returns item metadata joined with latest-snapshot lowest price information
- `LowestPriceQuery`
  - returns the lowest unit price for an item from the latest stored snapshot

Current command features:

- `PostAuctionDataCommand`
  - fetches the Blizzard commodity auction snapshot and persists it
  - executed by `WowPaperTrader.Ingestor`
- `UpdateItemsCommand`
  - finds item ids that appear in auction data but do not have metadata yet
  - fetches metadata and media from Blizzard
  - persists new metadata records
  - currently triggered manually through the API

Persistence follows the CQRS split pragmatically:

- write paths use EF Core repositories because they create and persist aggregate-shaped data
- read paths use Dapper and SQL tailored to the response shape
- the metadata update command uses a read service to discover missing metadata ids, then writes through a repository

## Runtime Data Flow

### Auction Ingestion

1. `AuctionDataBackgroundService` creates a scoped service provider.
2. The worker resolves `PostAuctionDataCommandHandler`.
3. The handler creates an `IngestionRun`.
4. `CommodityAuctionApiAdapter` requests an OAuth token through `BattleNetAuthClient`.
5. `CommodityAuctionClient` calls:

```text
GET https://us.api.blizzard.com/data/wow/auctions/commodities?namespace=dynamic-us&locale=en_US
```

6. Infrastructure maps Blizzard DTOs to application contracts.
7. `CommodityAuctionRepository` writes the snapshot and auction rows in a database transaction.
8. The ingestion run is marked `Finished`, `Failed`, or `Cancelled`.

### Metadata Update

1. `POST /api/v1/items` reaches `ItemsController.UpdateItemMetaData`.
2. The controller dispatches `UpdateItemsCommand`.
3. `ItemIdsWithoutMetadataReadService` finds distinct auction item ids missing from `ItemMetaData`.
4. `ItemMetadataApiAdapter` fetches item metadata and media from Blizzard:

```text
GET item/{itemId}?namespace=static-us&locale=en_US
GET media/item/{itemId}?namespace=static-us&locale=en_US
```

5. Infrastructure maps the DTOs into `ItemMetadataRecord`.
6. `ItemMetadataRepository` saves the metadata rows through EF Core.

### Read Path

1. A client calls a `GET /api/v1/items...` endpoint.
2. `ItemsController` validates route or query parameters.
3. The controller constructs an application query.
4. The matching query handler validates the query and calls its read-service contract.
5. The persistence read service executes Dapper SQL against the application database.
6. The controller returns `200 OK`, `400 Bad Request`, or `404 Not Found` depending on the request and result.

## REST API Design

The API is versioned at the route level with `/api/v1`. Item-related resources are grouped under `/api/v1/items`.

Development tooling:

- Swagger/OpenAPI is enabled in development
- default development URLs are `https://localhost:7033` and `http://localhost:5091`

### Search Items

```http
GET /api/v1/items?itemName=thorium
```

Behavior:

- returns `400 Bad Request` when `itemName` is empty or whitespace
- returns `200 OK` with up to five matches

Example response:

```json
[
  {
    "itemId": 10620,
    "name": "Thorium Ore"
  }
]
```

### Get Item Metadata

```http
GET /api/v1/items/{itemId}
```

Behavior:

- returns `400 Bad Request` when `itemId <= 0`
- returns `404 Not Found` when the item is not present in the latest auction snapshot result
- returns `200 OK` with item metadata and latest lowest-price information

Example response:

```json
{
  "itemId": 10620,
  "unitPrice": 19000,
  "priceTakenAtUtc": "2026-04-11T08:00:00Z",
  "name": "Thorium Ore",
  "qualityType": "COMMON",
  "qualityName": "Common",
  "level": 40,
  "requiredLevel": 0,
  "itemClassId": 7,
  "itemClassName": "Tradeskill",
  "itemSubclassId": 7,
  "itemSubclassName": "Metal & Stone",
  "imageUrl": "https://render.worldofwarcraft.com/us/icons/56/inv_ore_thorium_02.jpg",
  "metadataLastFetchedUtc": "2026-04-11T08:05:00Z"
}
```

### Get Lowest Auction Price

```http
GET /api/v1/items/{itemId}/auctions/lowest
```

Behavior:

- returns `400 Bad Request` when `itemId <= 0`
- returns `404 Not Found` when the latest stored snapshot does not contain that item
- returns `200 OK` with the minimum `UnitPrice` from the latest stored snapshot

Example response:

```json
{
  "itemId": 10620,
  "unitPrice": 19000,
  "priceTakenAtUtc": "2026-04-11T08:00:00Z"
}
```

### Update Missing Item Metadata

```http
POST /api/v1/items
```

Behavior:

- triggers `UpdateItemsCommand`
- fetches metadata only for item ids that have auction rows but no existing metadata row
- returns `200 OK` after the command completes

This endpoint is currently an operational command exposed through the item collection. It is useful during development, but it is not yet a background job or admin-only maintenance route.

## Data Stored

The EF Core migrations currently define these tables:

- `IngestionRuns`
  - tracks ingestion lifecycle, status, finish time, and error details
- `CommodityAuctionSnapshots`
  - stores one row per fetched commodity snapshot
  - links to `IngestionRuns`
  - stores `FetchedAtUtc` and the Blizzard API endpoint used
- `CommodityAuctions`
  - stores auction rows for each snapshot
  - stores `ItemId`, `Quantity`, `UnitPrice`, and `TimeLeft`
- `ItemMetaData`
  - stores Blizzard item metadata for item ids observed in auction data
  - includes item name, quality, item class, subclass, profession requirement fields, prices, stack/equip flags, purchase quantity, image URL, and metadata fetch time

Important scope notes:

- auction data is commodity-only and region-wide
- the schema does not store realm-specific auction information
- read APIs are built around the latest commodity snapshot
- metadata is stored separately from auction rows and may lag behind newly ingested auction item ids until the metadata update command runs

## Local Setup

### Prerequisites

- .NET 10 SDK
- SQL Server instance accessible from your machine
- Blizzard API client credentials

### Configure Blizzard Credentials

Both the ingestor and the API use the same user-secrets id. The Blizzard integration requires:

- `Blizzard:ClientId`
- `Blizzard:ClientSecret`

Example:

```powershell
dotnet user-secrets set "Blizzard:ClientId" "<your-client-id>" --project .\WowPaperTrader.Api\WowPaperTrader.Api.csproj
dotnet user-secrets set "Blizzard:ClientSecret" "<your-client-secret>" --project .\WowPaperTrader.Api\WowPaperTrader.Api.csproj
```

### Configure Database Connection

The API and ingestor require `ConnectionStrings:WowPaperTrader`.

Example PowerShell session:

```powershell
$env:ConnectionStrings__WowPaperTrader="Server=localhost;Database=WowPaperTrader;Trusted_Connection=True;TrustServerCertificate=True;"
```

Development settings files also exist at:

- `WowPaperTrader.Api/appsettings.Development.json`
- `WowPaperTrader.Ingestor/appsettings.Development.json`

### Configure Blizzard API Base URL

Both hosts read `WowApi:BaseUrl`. The default in `appsettings.json` is:

```json
{
  "WowApi": {
    "BaseUrl": "https://us.api.blizzard.com/data/wow/"
  }
}
```

### Apply Database Migrations

If `dotnet-ef` is not installed:

```powershell
dotnet tool install --global dotnet-ef
```

Apply the current migrations:

```powershell
dotnet ef database update --project .\WowPaperTrader.Persistence\WowPaperTrader.Persistence.csproj --startup-project .\WowPaperTrader.Ingestor\WowPaperTrader.Ingestor.csproj
```

## Running the Projects

Run the ingestor:

```powershell
dotnet run --project .\WowPaperTrader.Ingestor\WowPaperTrader.Ingestor.csproj
```

Behavior:

- one ingestion run starts immediately when the worker launches
- subsequent runs happen every hour
- graceful shutdown timeout is configured to 10 minutes

Run the API:

```powershell
dotnet run --project .\WowPaperTrader.Api\WowPaperTrader.Api.csproj
```

Default development URLs:

- `https://localhost:7033`
- `http://localhost:5091`

## Tests

Run the full solution test suite:

```powershell
dotnet test .\WowPaperTrader.sln
```

Current test structure:

- `WowPaperTrader.Persistence.Tests`
  - repository integration tests
  - Dapper read-service tests
  - schema-focused tests
- `WowPaperTrader.Application.Tests`
  - project scaffold currently contains no source test files

## Known Gaps

- retention cleanup is not automated; a manual helper script exists at `manual_delete_data_past_30_days_sql_script.txt`
- the ingestion interval is hard-coded to one hour in `AuctionDataBackgroundService`
- Blizzard API calls are fixed to the US region and `en_US` locale
- metadata update is manual and synchronous through the API
- no API authentication or authorization is configured
- read APIs do not expose historical price ranges or aggregates
- item metadata does not currently enforce a unique database constraint on `ItemId`

## Disclaimer

This is a personal educational project and is not affiliated with or endorsed by Blizzard Entertainment. World of Warcraft and Blizzard Entertainment are trademarks or registered trademarks of Blizzard Entertainment, Inc.

## License

No license file is present in the repository. All rights are reserved by default.
