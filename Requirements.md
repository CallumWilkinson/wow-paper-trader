# wow-paper-trader — Project Requirements Specification

## 1. Project Overview

wow-paper-trader is a web-based analytics and simulation platform for the World of Warcraft Auction House.  
It ingests official Blizzard Auction House snapshot data, stores it in a compliant local database, and exposes tools for historical price analysis and simulated “paper trading” using virtual gold.

The project is designed as a portfolio-grade, job-ready system, demonstrating backend engineering, data ingestion, authentication, persistence, and frontend integration using a modern C# + React stack.

---

## 2. High-Level Goals

- Ingest and persist WoW Auction House snapshot data in compliance with Blizzard API terms
- Build historical price views that do not exist in Blizzard’s live API
- Allow users to simulate gold-making strategies with fake portfolios
- Demonstrate real-world backend architecture, data modeling, auth, and APIs
- Serve as a centerpiece junior developer project over 2–3 months

---

## 3. System Architecture Requirements

### 3.1 Backend Architecture

- ASP.NET Core Web API (C#)
- Layered architecture:
  - API layer (controllers)
  - Application/services layer
  - Domain models
  - Infrastructure (Blizzard API, database)
- Clear separation between:
  - External data ingestion
  - Internal business logic
  - User-facing APIs

### 3.2 Frontend Architecture

- React with TypeScript
- Communicates exclusively with backend APIs
- No direct calls from frontend to Blizzard APIs
- Token-based authentication via JWT

### 3.3 Data Storage

- SQL Server
- Managed via Entity Framework Core
- Explicit schemas for:
  - Auction snapshots
  - Aggregated historical prices
  - Users and portfolios
  - Trades and transactions

---

## 4. Blizzard API Integration Requirements

### 4.1 API Usage

- Use Blizzard Developer APIs only
- Support WoW Retail initially
- Start with a single realm:
  - Server: Area 52
- Future extensibility for:
  - Multiple realms
  - Classic / SoD / Anniversary variants

### 4.2 Data Retention Compliance

- Enforce a maximum 30-day data retention window
- Automatically delete or expire auction data older than 30 days
- Retention enforcement must occur server-side

### 4.3 Snapshot Handling

- Auction House endpoint is treated as a full snapshot
- Each snapshot ingestion must:
  - Be timestamped
  - Be associated with a realm and game version
- No mutation of raw snapshot data after ingestion

---

## 5. Data Ingestion Service Requirements

### 5.1 Ingestion Method

- Background worker service (ASP.NET Worker Service or hosted background task)
- Periodically pulls Auction House snapshots
- Interval must be configurable

### 5.2 Ingestion Responsibilities

- Fetch auction data from Blizzard API
- Validate response integrity
- Persist raw auction records
- Persist derived aggregates (optional, but preferred)

### 5.3 Fault Tolerance

- Graceful handling of:
  - API downtime
  - Rate limits
  - Network failures
- Logging of ingestion failures
- No partial snapshot writes

---

## 6. Data Modeling Requirements

### 6.1 Auction Data

- Store:
  - Item ID
  - Buyout price
  - Quantity
  - Time left
  - Snapshot timestamp
  - Realm and region
- Support multiple auctions per item per snapshot

### 6.2 Historical Price Aggregation

- Derive time-series data:
  - Min price
  - Max price
  - Average price
  - Median price (optional)
- Aggregation intervals:
  - Per snapshot
  - Daily rollups (optional)

---

## 7. User System Requirements

### 7.1 Authentication

- ASP.NET Identity
- JWT-based authentication
- Secure password hashing
- Refresh-token or re-login strategy (implementation choice documented)

### 7.2 User Accounts

- Register
- Login
- Logout
- Authenticated API access only for portfolio features

---

## 8. Paper Trading Requirements

### 8.1 Virtual Portfolio

- Each user has:
  - Starting gold balance (configurable)
  - Portfolio of items
- No interaction with real WoW accounts

### 8.2 Trading Rules

- Users can:
  - “Buy” items at historical prices
  - “Sell” items at historical prices
- Trades must:
  - Reference a specific snapshot or time
  - Adjust gold and item balances accordingly

### 8.3 Trade Validation

- Cannot spend more gold than available
- Cannot sell items not owned
- All trades must be persisted

---

## 9. API Requirements

### 9.1 Public APIs

- Item search
- Current snapshot prices
- Historical price data

### 9.2 Authenticated APIs

- Portfolio overview
- Trade execution (buy/sell)
- Trade history
- Portfolio performance metrics

### 9.3 API Design

- RESTful conventions
- Clear HTTP status codes
- Consistent error response format
- Swagger/OpenAPI documentation enabled

---

## 10. Frontend Feature Requirements

### 10.1 Core UI Features

- Item search by name or ID
- Realm selection (single realm initially)
- Historical price charts
- Snapshot-based price views

### 10.2 User Features

- Register / login flow
- Portfolio dashboard
- Trade execution UI
- Trade history table

### 10.3 UX Requirements

- Clear separation between:
  - Real auction data
  - Simulated trading
- Explicit disclaimers that no real gold is involved

---

## 11. Non-Functional Requirements

### 11.1 Performance

- Ingestion must not block API requests
- Read-heavy APIs optimized with indexing

### 11.2 Security

- JWT validation on protected endpoints
- No Blizzard credentials exposed to frontend
- Environment-based secrets management

### 11.3 Maintainability

- Clean, readable code
- Explicit naming
- No hidden “magic” behavior
- Logging at ingestion and trade layers

---

## 12. Development & Tooling Requirements

- VS Code on Windows 11
- PowerShell for CLI usage
- SSMS for database inspection
- Swagger for API exploration
- GitHub for source control
- Consistent project casing: `wow-paper-trader`

---

## 13. Portfolio & Presentation Requirements

- Clean README explaining:
  - Architecture
  - Data flow
  - Compliance decisions
- Clear explanation of Blizzard data retention handling
- Screenshots or diagrams (optional)
- Code structured to demonstrate junior-to-mid backend competence

---

## 14. Future Extension Requirements (Out of Scope for MVP)

- Multiple realms
- Cross-realm comparisons
- Multi-version WoW support
- Strategy backtesting
- Alerts or signals
- Leaderboards for simulated performance

---

## 15. Success Criteria

The project is considered successful if it:

- Fully complies with Blizzard API terms
- Demonstrates end-to-end data ingestion and querying
- Supports authenticated users and paper trading
- Is understandable, extensible, and interview-ready
