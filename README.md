# WowPaperTrader

WowPaperTrader is a work-in-progress web application that ingests World of Warcraft Auction House snapshot data and allows users to explore item price history and simulate gold-making strategies using a paper-trading system.

The project is inspired by real-world stock trading platforms, but applied to the World of Warcraft economy. Users will eventually be able to give themselves a starting amount of gold, buy and sell items using historical auction data, and evaluate strategies without risking real in-game gold.

This repository represents an early foundation and is being built incrementally with a strong focus on clean architecture, test driven development, maintainability, and real-world backend patterns.

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

The solution is structured as a multi-project .NET solution to clearly separate responsibilities.

### Current Projects

- **WowPaperTrader.Api**

  - ASP.NET Core Web API
  - Will expose endpoints for querying auction data, items, servers, and user portfolios
  - Will handle authentication and authorization (planned: ASP.NET Identity + JWT)

- **WowPaperTrader.Ingestor**
  - Background worker service
  - Responsible for pulling Auction House snapshot data from the Blizzard API
  - Persists snapshot data into the database
  - Decoupled from the API to allow independent scheduling and scaling

### Planned Additions

- **Domain layer**
  - Core business models and rules
- **Infrastructure layer**
  - Database access (Entity Framework Core)
  - External API integrations
- **Frontend**
  - React + TypeScript client application (planned)
  - Material UI
  - Consumes the API and provides the user interface

---

## Data Model Philosophy

Blizzard’s Auction House API provides snapshot-style data rather than historical queries.  
This project builds history by:

1. Periodically ingesting snapshots per server
2. Storing auction data with timestamps
3. Querying historical prices from the local database

All Blizzard API data usage will comply with Blizzard’s Developer API Terms, including data retention limits.

---

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
- Playwrite E2E testing

### Tooling

- VS Code
- Git & GitHub
- PowerShell
- Swagger / OpenAPI

---

## Current Status

🚧 **Early development**  
The repository currently contains solution and project scaffolding.  
Core ingestion logic, database schema, and API endpoints are under active development.

This project is intended to evolve over time and act as a portfolio centerpiece.

---

## Disclaimer

This project is a personal, educational project and is not affiliated with or endorsed by Blizzard Entertainment.  
World of Warcraft and Blizzard Entertainment are trademarks or registered trademarks of Blizzard Entertainment, Inc.

---

## License

No license has been applied yet. All rights reserved by default.
