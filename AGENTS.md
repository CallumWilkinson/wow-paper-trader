# AGENTS.md

## Project context

This repository is **WowPaperTrader**, a World of Warcraft auction house analysis project built by Callum.

The goal is to build a portfolio-quality full-stack application that ingests World of Warcraft auction data, stores pricing history, and exposes searchable item pricing through a .NET backend and a React frontend.

The project demonstrates:

- C# and .NET backend development
- Layered architecture
- CQRS-style read and write separation
- SQL Server persistence
- EF Core write-side persistence
- Dapper read-side queries
- Background data ingestion
- External API integration with Blizzard APIs
- TypeScript React frontend development
- Charting and pricing UI
- Testing and maintainable engineering practices

This is not a throwaway demo. Treat it as a serious portfolio project where code quality, architecture, naming, tests, and commit boundaries matter.

## Current project priorities

Prioritize in this order:

1. Keep the application working
2. Make the smallest useful change
3. Preserve the existing architecture
4. Keep backend logic testable and covered by useful tests
5. Keep frontend code readable and easy to reason about
6. Avoid unnecessary rewrites
7. Avoid overengineering beyond the current feature

Do not optimize for cleverness, premature abstraction, large rewrites, or speculative future features.

The project is primarily for learning and job readiness, so prefer decisions that make the code easier to explain in an interview.

## Tech stack

Use the existing project stack.

Backend:

- C#
- .NET
- ASP.NET Core Web API
- Entity Framework Core
- Dapper
- SQL Server
- xUnit
- FluentAssertions

Frontend:

- React
- TypeScript
- Vite
- Material UI
- Axios
- React Query
- React Query DevTools
- Recharts, where charting is needed

General:

- Use PowerShell command syntax whenever suggesting terminal commands
- Do not use Bash commands
- Use the existing solution and project structure
- Do not introduce new libraries unless there is a clear reason and Callum agrees

Example commands:

```powershell
dotnet test
npm run build
npm run dev
```

Do not use Bash commands.

## Known repository structure

Assume the project follows this general structure:

```text
WowPaperTrader.Api/
WowPaperTrader.Application/
WowPaperTrader.Application.Tests/
WowPaperTrader.Infrastructure/
WowPaperTrader.Ingestor/
WowPaperTrader.Persistence/
WowPaperTrader.Persistence.Tests/
frontend/
WowPaperTrader.sln
README.md
Requirements.md
```

Use the actual repository structure after inspecting the repo. Do not assume paths blindly.

Expected responsibilities:

- `WowPaperTrader.Api` contains HTTP controllers, API startup, dependency injection, CORS, and request handling
- `WowPaperTrader.Application` contains application use cases, CQRS-style features, contracts, read models, commands, queries, and handlers
- `WowPaperTrader.Infrastructure` contains external API clients and adapters, especially Blizzard API integration
- `WowPaperTrader.Persistence` contains EF Core DbContext, repositories, migrations, and Dapper read services
- `WowPaperTrader.Ingestor` contains background ingestion logic
- `WowPaperTrader.Application.Tests` contains application-level unit tests
- `WowPaperTrader.Persistence.Tests` contains persistence and read-service integration tests
- `frontend` contains the TypeScript React Vite frontend

If the real structure differs, inspect it and follow the actual structure.

## Architecture expectations

Respect the layered architecture.

The dependency direction should stay clean:

- Application should not depend on Api, Infrastructure, Persistence, or frontend
- Api may depend on Application, Infrastructure, and Persistence
- Infrastructure may depend on Application contracts
- Persistence may depend on Application contracts and models where appropriate
- Ingestor may depend on Infrastructure and Persistence
- Frontend should call the backend API, not database or backend internals

Do not move code across layers without a clear architectural reason.

Do not place business rules in controllers.

Do not place SQL query logic in the frontend.

Do not place external Blizzard API details in Application unless they are represented behind an interface or contract.

Prefer this flow:

```text
Controller → QueryHandler or CommandHandler → Application contract → Persistence or Infrastructure implementation
```

For reads, prefer explicit read models and read services.

For writes, prefer command handlers, repositories, and EF Core where that already matches the project.

## CQRS and feature organization

Follow the existing CQRS-style conventions.

Expected Application feature structure:

```text
Features/
  Read/
    FeatureName/
  Write/
    FeatureName/
```

Keep read and write logic separate.

Use clear names such as:

- `MonthlyPriceQuantityQueryHandler`
- `MonthlyPriceQuantityReadService`
- `LowestPriceQueryHandler`
- `ItemSearchQueryHandler`
- `UpdateItemsCommandHandler`

Do not create generic service names like:

- `DataService`
- `Manager`
- `Helper`
- `Processor`

Prefer names that describe the domain responsibility.

## Backend coding guidelines

Write C# code for readability first.

Use:

- Clear method names
- Explicit control flow
- Early returns where useful
- Small classes with one responsibility
- Constructor injection
- Interfaces at architectural boundaries
- Cancellation tokens where existing code uses them
- `async` and `await` for asynchronous work
- Guard clauses for invalid inputs
- Constants for repeated magic values

Avoid:

- Large methods
- Hidden side effects
- Static helper dumping grounds
- Overly generic abstractions
- Clever LINQ that is hard to read
- Silent exception swallowing
- Mixing API, application, persistence, and infrastructure concerns

Prefer explicit code over compact code.

Do not rewrite working code just to make it look different.

## Frontend coding guidelines

Use TypeScript React for the WowPaperTrader frontend.

Use:

- Functional React components
- Simple props
- Small components
- Custom hooks for API/query logic
- React Query for server state
- Axios where the project already uses it
- Material UI for UI elements
- Recharts for graphing where appropriate
- Utility functions for formatting prices and dates

Avoid:

- Large all-in-one components
- Business logic buried in JSX
- Unnecessary global state
- Overly clever TypeScript types
- Ternary operators
- Hidden date or currency assumptions
- Direct API calls inside deeply nested presentational components

Prefer this:

```tsx
if (selectedItemId === null) {
  return null;
}

return <SelectedItemCard itemId={selectedItemId} />;
```

Avoid this:

```tsx
return selectedItemId ? <SelectedItemCard itemId={selectedItemId} /> : null;
```

Keep frontend data flow easy to explain.

## Data and database expectations

The project stores World of Warcraft auction and item metadata.

Known concepts include:

- `CommodityAuctionSnapshot`
- `CommodityAuction`
- `IngestionRun`
- `ItemMetaData`
- hourly snapshots
- 30-day price and quantity history
- lowest unit price
- total quantity posted
- item search by name
- Blizzard API metadata and media/image URLs

Be careful with database changes.

Before changing schema:

1. Inspect the entity
2. Inspect the DbContext
3. Inspect existing migrations
4. Understand how the field is used
5. Make the smallest schema change possible
6. Add or update tests where useful
7. Provide the exact migration command, but do not run destructive commands without permission

Use PowerShell examples for EF Core commands:

```powershell
dotnet ef migrations add AddExampleField --project .\WowPaperTrader.Persistence\ --startup-project .\WowPaperTrader.Api\
```

Do not delete migrations or reset the database unless Callum explicitly asks.

Do not casually suggest dropping production-like data.

## SQL and read service guidelines

Dapper read services should stay explicit and readable.

Use SQL when it makes the read model simpler or more efficient.

When writing SQL:

- Use clear aliases
- Use readable CTEs where they improve clarity
- Avoid deeply nested queries if a CTE is clearer
- Keep parameters explicit
- Avoid string interpolation for SQL values
- Keep ordering intentional
- Be careful with UTC date filtering
- Be careful with newest-first vs oldest-first ordering for graphs

For graph data, prefer returning data in the order the UI needs unless there is a clear reason not to.

For 30-day hourly data, preserve the expected full time range where the UI needs a stable graph shape.

## External API integration guidelines

Blizzard API details belong in Infrastructure.

Application code should depend on contracts or adapters, not raw HTTP details.

When touching Blizzard API integration:

- Inspect the existing client or adapter first
- Preserve token handling
- Preserve namespace and locale conventions unless the task requires changing them
- Handle 404s or missing metadata explicitly
- Avoid hiding failed item metadata fetches
- Keep logging useful but not noisy
- Do not hardcode secrets

Never place API keys, client secrets, tokens, or credentials in code.

## Testing expectations

Unlike the small portfolio website project, WowPaperTrader should use tests for meaningful backend and data logic.

When changing backend logic, add or update tests where practical.

Prioritize tests for:

- Application handlers
- Domain or application logic
- Price formatting or calculation logic
- Read service query behavior
- Repository behavior
- Ingestion behavior
- Edge cases around missing data
- Date filtering
- 30-day graph data generation
- Lowest price calculations
- Item search ranking

Use:

- xUnit
- FluentAssertions
- Existing test fixtures
- Real instances over mocks where practical

Avoid over-mocking.

Do not add broad, brittle tests that only assert implementation details.

Do not rewrite the whole test setup unless the current setup blocks the task.

For frontend, add tests only if Callum asks or if the project already has an established frontend test setup for the area being changed.

Frontend code should still be written in a testable state:

- Keep formatting functions separate
- Keep components small
- Keep API hooks separate from presentation
- Keep conditional rendering simple
- Avoid browser-only side effects in pure utilities

## Test and build commands

After backend changes, run the smallest relevant check first.

Examples:

```powershell
dotnet test
```

For a specific test project:

```powershell
dotnet test .\WowPaperTrader.Application.Tests\
```

```powershell
dotnet test .\WowPaperTrader.Persistence.Tests\
```

After frontend changes, run:

```powershell
npm run build
```

Run commands from the correct directory.

If the frontend lives in a `frontend` folder, say so explicitly:

```powershell
cd .\frontend
npm run build
```

If a check cannot be run, say why clearly.

Do not pretend a command passed if it was not run.

## Development workflow

Before changing code:

1. Inspect the existing repository structure
2. Read the relevant files before editing
3. Identify the smallest useful change
4. Explain the intended change briefly if the task is not trivial
5. Preserve existing working code
6. Avoid large rewrites unless the current structure clearly blocks progress

While changing code:

1. Make one logical change at a time
2. Keep added or changed files limited where possible
3. Avoid unrelated cleanup
4. Avoid changing naming conventions unless there is a clear reason
5. Keep the application working after each change
6. Prefer practical progress over theoretical perfection

After changing code:

1. Run the relevant local check
2. Fix compile errors
3. Fix TypeScript errors
4. Fix obvious runtime errors
5. Summarize what changed
6. Provide a short proposed commit message
7. Pause for review

Do not continue to the next major task until Callum has reviewed the result or explicitly says to continue.

## Review loop

Work in a back-and-forth review loop.

After each logical change, output:

1. Files changed
2. What changed
3. Why it changed
4. Any trade-offs or assumptions
5. Checks run
6. Suggested commit message
7. Suggested files to stage, if relevant
8. Anything Callum should manually check

Then stop and wait for feedback.

Do not push ahead through multiple unrelated changes without review.

If Callum gives feedback, revise the current solution before starting a new task.

## Git workflow

Never commit code.

Never run:

```powershell
git commit
git push
```

Callum will handle commits manually.

You may suggest commands like:

```powershell
git add .\WowPaperTrader.Application\Features\Read\MonthlyPriceQuantity\MonthlyPriceQuantityQueryHandler.cs
```

Only stage files automatically if Callum explicitly asks you to stage them.

Always provide a short, copy-pasteable Conventional Commit message after each logical change.

Prefer small commit messages.

Use this format:

```text
feat: add monthly price quantity endpoint
```

Use a body only if the change needs explanation.

Commit types:

- `feat:` for new features
- `fix:` for bug fixes
- `refactor:` for code reorganization without behaviour changes
- `test:` for test additions or test changes
- `docs:` for documentation updates
- `chore:` for setup or config changes
- `style:` for visual-only changes

Each proposed commit should represent one logical change.

Do not bundle unrelated changes into one commit.

Each commit should leave the codebase in a working state.

## Commit boundary expectations

After suggesting a commit message, explain briefly why the change belongs in one commit.

If the work should be split, suggest the split.

Good commit boundary examples:

- One commit for adding a query handler
- One commit for adding a Dapper read service
- One commit for adding an API endpoint
- One commit for wiring dependency injection
- One commit for adding integration tests
- One commit for adding a frontend hook
- One commit for adding a chart component
- One commit for visual polish
- One commit for documentation updates

Avoid giant commits that mix backend API changes, database schema changes, frontend components, styling, and documentation.

If one feature needs multiple layers, ask whether Callum wants it split or complete the smallest backend slice first.

## Scope boundaries

Do not add these unless explicitly requested:

- Authentication
- User accounts
- Payment features
- Complex deployment automation
- Large state management libraries
- GraphQL
- New database engines
- New frontend frameworks
- Background job frameworks
- Message queues
- Caching layers
- Docker setup
- Kubernetes setup
- Cloud hosting changes
- Major architecture rewrites

If a task seems to require one of these, explain the trade-off and suggest the smallest alternative first.

## Clean code principles

Write code for humans first.

Use:

- Descriptive names
- Small classes
- Small functions
- Clear file responsibilities
- Explicit control flow
- Guard clauses and early returns
- Constants instead of magic strings or numbers
- Consistent formatting
- Simple data shapes
- Clear seams between API, application, infrastructure, persistence, and UI

Avoid:

- Ternary operators
- Clever one-liners
- Deeply nested logic
- Vague names like `data`, `temp`, `foo`, `result2`, or `x`
- Copy-pasted logic
- Unnecessary abstraction
- Mixed concerns
- Hidden dependencies
- CommonJS syntax in frontend code
- Silent error handling
- Large files that do too many things

Prefer clarity over performance unless performance is the actual problem being solved.

## Commenting rules

Only write comments when the reason, intent, or assumption is not obvious from the code.

Do not comment what the code is doing.

Prefer clearer names and smaller functions over explanatory comments.

Use comments for:

- Non-obvious decisions
- Assumptions
- Edge cases
- Workarounds
- Gotchas
- Future review notes

Use these tags responsibly:

```csharp
// TODO: Add retry handling after the ingestion flow is stable.
// FIXME: Replace temporary query once duplicate item names are handled properly.
// HACK: Keeps the graph range stable until missing snapshot handling is moved server-side.
```

Do not write comments that may quickly become false.

Delete outdated comments.

Do not add AI-generated signature comments.

Do not add:

```text
Generated with Claude Code
Co-Authored-By: Claude
```

## Error handling and debugging

Handle errors explicitly.

For backend code:

- Validate inputs at API boundaries
- Return appropriate responses from controllers
- Avoid hiding failed ingestion problems
- Keep exception handling meaningful
- Preserve stack traces where relevant
- Do not catch exceptions just to ignore them

For frontend code:

- Show useful loading states
- Show useful error states
- Guard optional values
- Avoid crashing on missing image URLs or missing price data
- Keep formatting functions defensive

If something seems structurally wrong, flag it instead of working around it silently.

## Refactoring expectations

Refactor only with a clear purpose.

Do not move code just to tidy it up.

If code is unclear:

1. Identify the issue
2. Explain why it may be a problem
3. Suggest a small fix
4. Wait for review if the change is not clearly necessary

If you find old or questionable code, do not delete it unless it is clearly unused and safe to remove.

If unsure, flag it for review.

Use `// legacy:` comments only when needed to mark old code that should not be removed yet.

Always question design flaws and assumptions.

Do not blindly follow a poor structure if a small improvement would make the code clearer.

## Large task handling

If a task is too large to safely complete in one pass, split it into smaller steps.

Use this format:

```text
This task is too large to do safely in one pass. I recommend splitting it into:

1. Step one
2. Step two
3. Step three
```

Then complete only the first useful step and pause for review.

Do not silently truncate work.

Do not pretend that a large task is fully complete if only part of it was handled.

At the end of a multi-step task, summarize what was completed and what remains.

## Feature implementation guidance

When adding a backend feature, prefer this order:

1. Define or inspect the response/request model in Application
2. Add or update the query or command handler
3. Add or update the read service or repository contract
4. Implement the Persistence or Infrastructure adapter
5. Add or update tests
6. Wire dependency injection
7. Add or update the API endpoint
8. Run relevant tests

When adding a frontend feature, prefer this order:

1. Define or inspect the TypeScript type
2. Add or update the API client function
3. Add or update the React Query hook
4. Add or update the presentational component
5. Wire the component into the page
6. Add simple loading and error states
7. Run the production build

Keep each step small enough to review.

## UI and graphing guidance

The frontend should be clear, useful, and portfolio-ready.

Prioritize:

- Search clarity
- Readable item cards
- Correct price formatting
- Correct date and time formatting
- Clear graph labels
- Stable chart behavior
- Mobile usability
- Simple loading and error states

For price display:

- Unit price is stored as copper
- Format copper into gold, silver, and copper where useful
- Keep price formatting in a utility function

For date display:

- Treat API timestamps as UTC unless proven otherwise
- Convert to local time in the frontend for display
- Be explicit when normalizing timestamps

For charts:

- Prefer oldest-to-newest order on the x-axis
- Keep tooltip labels readable
- Label price and quantity clearly
- Avoid confusing price values with quantity values
- Keep missing data behavior intentional

## Documentation expectations

Update documentation when a change affects:

- Project setup
- Architecture
- API routes
- Environment variables
- Database migrations
- Test commands
- Ingestion behavior
- Frontend run instructions

Do not rewrite documentation unrelated to the change.

Keep documentation honest and specific.

## Manual review checklist

Before considering a feature complete, check:

- Backend compiles
- Relevant tests pass
- Frontend builds, if touched
- API route matches the intended resource
- Dependency injection is wired correctly
- SQL query returns data in the expected order
- UTC handling is correct
- Error states are handled
- No secrets are committed
- No unrelated files changed
- Suggested commit message is provided
- Commit boundary is clear

## Final response format after a code change

After making a code change, respond with:

```text
Files changed:
- path/to/file

What changed:
- ...

Why:
- ...

Checks:
- dotnet test passed
```

If checks were not run, say:

```text
Checks:
- Not run. Reason: ...
```

Then include:

```text
Suggested commit message:
feat: add monthly price quantity endpoint
```

Then include:

```text
Commit boundary:
This should be one commit because ...
```

Then include:

```text
Suggested files to stage:
- path/to/file
```

Then pause for Callum's review.

Do not keep making changes after this unless Callum says to continue.

## Important working style

Callum wants small, reviewable changes.

Do not rush through large automated edits.

Do not commit.

Do not push.

Do not silently stage files.

Do not hide uncertainty.

Read before editing.

Prefer one clean, working change over five half-finished changes.

The best outcome is code Callum understands, trusts, and can explain in an interview.
