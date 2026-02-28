# WowPaperTrader

WowPaperTrader is a work-in-progress web application that ingests World of Warcraft Auction House data and allows users to explore item price history and simulate gold-making strategies using a paper-trading system.

The project is inspired by real-world stock trading platforms, but applied to the World of Warcraft economy. Users will eventually be able to give themselves a starting amount of gold, buy and sell items using historical auction data, and evaluate strategies without risking real in-game gold.

This repository is being built incrementally with a focus on clean architecture, test-driven development, maintainability, and real-world backend patterns.

---

## Project Goals

- Ingest Auction House snapshot data from the Blizzard World of Warcraft API
- Store historical auction data in a relational database for querying
- Allow searching item price history by server and item
- Support multiple World of Warcraft versions in the future (Retail, Classic, etc.)
- Provide a paper-trading system where users can simulate buying and selling items
- Practice real-world backend and full-stack development patterns

---

## Architecture Overview

The solution is structured as a multi-project .NET solution to separate responsibilities.

### Current Projects

- **wow-paper-trader.Api**
  - ASP.NET Core Web API
  - Currently a starter API (template endpoints + OpenAPI in Development)
  - Planned: endpoints for querying auction data, items, realms, and user portfolios
  - Planned: authentication and authorization (ASP.NET Identity + JWT)

- **wow-paper-trader.Ingestor**
  - Background worker service
  - Pulls commodity auction snapshots from the Blizzard WoW API (US region config by default)
  - Persists snapshots into SQL Server via Entity Framework Core
  - Runs on a loop (currently fixed at 1 hour)

- **wow-paper-trader.Ingestor.Tests**
  - xUnit integration tests for ingestion
  - Uses SQLite in-memory for database assertions

## Tech Stack (Planned / In Progress)

### Backend

- .NET (ASP.NET Core Web API)
- Background Workers
- Entity Framework Core
- SQL Server
- Unit Test Focus
- JWT & .NET Identity

### Frontend (Planned)

- React
- TypeScript
- Material UI
- Axios
- React Router
- Playwright E2E testing

### Tooling

- VS Code
- Git & GitHub
- PowerShell
- Swagger / OpenAPI

### Planned Additions

- Frontend (React + TypeScript)
- Paper-trading domain (portfolios, trades, performance)
- Additional data ingestion (realm auctions, item metadata, etc.)

---

## Quickstart (Local Dev)

### Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB or SQL Express is fine)
- Blizzard API credentials (Client ID + Client Secret)

### Configure Secrets (Ingestor)

The ingestor reads credentials from configuration keys:

- `Blizzard:ClientId`
- `Blizzard:ClientSecret`

Using user-secrets:

```powershell
dotnet user-secrets set "Blizzard:ClientId" "<your-client-id>" --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
dotnet user-secrets set "Blizzard:ClientSecret" "<your-client-secret>" --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
```

### Configure Database (Ingestor)

Set `ConnectionStrings:WowPaperTrader` (for example in `wow-paper-trader.Ingestor/appsettings.Development.json`, or via an env var like `ConnectionStrings__WowPaperTrader`).

Run migrations:

```powershell
dotnet tool install --global dotnet-ef
dotnet ef database update --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
```

### Run

```powershell
dotnet run --project .\wow-paper-trader.Ingestor\wow-paper-trader.Ingestor.csproj
dotnet run --project .\wow-paper-trader.Api\wow-paper-trader.Api.csproj
```

Run tests:

```powershell
dotnet test
```

---

## Data Model (Current)

The ingestor currently writes:

- `IngestionRuns` (status + errors)
- `CommodityAuctionSnapshots` (fetched time + endpoint)
- `CommodityAuctions` (item id, quantity, unit price, time left)

---

## Data Model Philosophy

Blizzard's Auction House APIs are snapshot-style (not historical queries). This project builds history by:

1. Periodically ingesting snapshots
2. Storing auction data with timestamps
3. Querying historical prices from the local database

---

## Current Status

**Active development**

- API: currently implementing API to search current lowest auction price given an item ID using current data ingestor derived database

This project is intended to evolve over time and act as a portfolio centerpiece.

---

## Disclaimer

This project is a personal, educational project and is not affiliated with or endorsed by Blizzard Entertainment.
World of Warcraft and Blizzard Entertainment are trademarks or registered trademarks of Blizzard Entertainment, Inc.

---

## License

No license has been applied yet. All rights reserved by default.
