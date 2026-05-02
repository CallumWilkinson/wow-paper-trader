# AGENTS.md

## Project context

This folder contains the **frontend only** for **WowPaperTrader**, a World of Warcraft auction house analysis application.

The frontend is responsible for presenting auction house data from the backend API in a clear, useful, and portfolio-quality interface.

The frontend should help users:

- Search for World of Warcraft items
- Select an item from search results
- View item metadata
- View latest lowest unit price
- View price and quantity history
- Understand price data through readable cards, labels, tooltips, and graphs

This file is intentionally scoped to the frontend folder only.

## Frontend-only boundary

Only work inside this frontend project folder.

Do not edit files outside this folder.

Do not modify:

- Backend API projects
- Application layer projects
- Infrastructure projects
- Persistence projects
- Ingestor projects
- Backend test projects
- Database migrations
- Solution files outside this frontend folder
- Root-level backend configuration
- Backend `Program.cs`
- Backend controllers
- Backend DTOs
- Backend query handlers
- Backend read services
- Backend repositories

If a frontend task appears to require a backend change, stop and explain what backend change may be needed. Do not make that backend change.

Acceptable frontend changes include:

- React components
- React pages
- React hooks
- TypeScript frontend types
- Frontend API client files
- Frontend utility functions
- Frontend styling
- Frontend assets
- Frontend package configuration
- Frontend tests, only when appropriate or requested
- Frontend documentation inside this folder

The goal is to keep this agent safely contained to frontend work.

## Current frontend priorities

Prioritize in this order:

1. Keep the frontend building successfully
2. Make the smallest useful change
3. Preserve existing component and hook structure
4. Keep UI behaviour easy to understand
5. Keep API boundaries explicit
6. Keep formatting logic reusable and testable
7. Avoid unnecessary dependencies
8. Avoid unrelated cleanup

Do not optimize for cleverness, animation polish, complex design systems, or speculative features.

The frontend is part of a job-ready portfolio project, so the code should be easy for Callum to explain in an interview.

## Tech stack

Use the existing frontend stack.

Assume the frontend uses:

- React
- TypeScript
- Vite
- Material UI
- Axios
- React Query
- React Query DevTools
- Recharts, where charting is needed
- ES modules

Use TypeScript React for components.

Use PowerShell command syntax whenever suggesting terminal commands.

Example:

```powershell
npm install
npm run dev
npm run build
```

Do not use Bash commands.

## Expected frontend structure

Inspect the actual folder structure before editing.

A typical structure may look like:

```text
src/
  api/
  assets/
  components/
  features/
  hooks/
  pages/
  providers/
  types/
  utils/
  App.tsx
  main.tsx
```

Follow the actual existing structure. Do not create a new structure unless there is a clear reason.

Expected responsibilities:

- `api/` contains Axios client setup and API request functions
- `components/` contains reusable UI components
- `features/` contains feature-specific components, hooks, and types
- `hooks/` contains shared hooks
- `pages/` contains page-level components
- `providers/` contains app-level providers such as React Query setup
- `types/` contains shared TypeScript types
- `utils/` contains formatting and transformation helpers
- `assets/` contains local images and static assets

Do not place large API logic inside presentational components.

Do not place large JSX components inside data or utility files.

## Frontend architecture expectations

Keep frontend responsibilities clear.

Prefer this flow:

```text
Page → Feature component → Hook → API client → Backend API
```

Use React Query for server state.

Use component state for local UI state.

Do not add global state libraries unless Callum explicitly asks.

Do not bypass the API client layer by calling Axios directly inside deep presentational components.

Keep formatting functions separate from rendering when practical.

Keep chart data transformation explicit and easy to test.

## API boundary rules

The frontend consumes backend API responses. It does not own backend contracts.

When working with API data:

- Define clear TypeScript types for response shapes
- Keep API calls in dedicated API files or feature API files
- Keep React Query hooks separate from presentational components
- Use `enabled` on queries when required data is missing
- Use stable, descriptive query keys
- Handle loading states
- Handle error states
- Avoid assuming optional backend values are always present

If an API response shape seems wrong, do not silently patch around it. Explain the mismatch.

If the backend appears to return data in an awkward order, decide whether the frontend can safely transform it. If the correct fix belongs in the backend, explain that instead of editing backend files.

## React Query guidelines

Use React Query for server state.

Use clear query keys.

Prefer query keys like:

```ts
["items", "search", searchTerm][("items", "metadata", selectedItemId)][
  ("items", "priceQuantity", itemId)
];
```

Use `enabled` to prevent invalid requests.

Example:

```ts
const shouldFetchItem = selectedItemId !== null;

const query = useQuery({
  queryKey: ["items", "metadata", selectedItemId],
  queryFn: () => getItemMetadata(selectedItemId),
  enabled: shouldFetchItem,
});
```

Avoid making API requests with empty search terms, `null` IDs, or placeholder values.

Do not use React Query for purely local UI state.

## Axios guidelines

Use the existing Axios setup.

Do not create duplicate Axios clients unless there is a clear reason.

Keep base URL configuration centralized.

Use Vite environment variables where the project already does.

For example:

```ts
const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;
```

Do not hardcode production URLs into components.

Do not store secrets in frontend code.

Frontend environment variables are public once built.

## Component guidelines

Use functional React components.

Keep components focused on one job.

Prefer simple props and explicit types.

Use descriptive names.

Prefer this:

```tsx
interface SelectedItemCardProps {
  itemId: number;
}

export default function SelectedItemCard({ itemId }: SelectedItemCardProps) {
  return <Card>{itemId}</Card>;
}
```

Avoid vague component names like:

- `DataCard`
- `InfoBox`
- `ThingList`
- `TempComponent`

Use Material UI where it helps build quickly and consistently.

Prefer readable `sx` styling over large custom CSS files unless the existing project already uses CSS modules or plain CSS for that area.

Do not create a complex theme architecture unless Callum explicitly asks.

## TypeScript guidelines

Use TypeScript clearly, not cleverly.

Use:

- Explicit interfaces for props
- Clear response types
- Simple union types where useful
- `number | null` for selected IDs that may not exist
- `string | undefined` for optional strings
- Type guards where they improve clarity

Avoid:

- `any`
- Type assertions unless there is a clear reason
- Complex generics for simple UI code
- Overly broad types
- Non-null assertions unless unavoidable

If a value can be missing, handle that explicitly.

Prefer this:

```ts
if (item.imageUrl === undefined) {
  return fallbackImageUrl;
}

return item.imageUrl;
```

Avoid this:

```ts
return item.imageUrl!;
```

## Control flow style

Prioritize readability over compactness.

Use explicit `if` statements.

Avoid ternary operators.

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

Use early returns when they reduce nesting.

Avoid clever one-liners.

## Styling guidelines

The UI should feel:

- Clean
- Practical
- Responsive
- Developer-focused
- Easy to scan
- Suitable for a portfolio demo

Prioritize clarity over decoration.

For Material UI:

- Use existing theme conventions if present
- Use `Box`, `Stack`, `Card`, `Typography`, `Button`, `TextField`, and similar components where appropriate
- Keep `sx` readable
- Avoid giant `sx` objects inside large components
- Extract repeated styles only when repetition becomes annoying

Do not over-polish visual design before the core behaviour works.

## Search UI expectations

The search experience should be simple and predictable.

When working on item search:

- Debounce user input where the existing code does this
- Do not search for empty strings
- Show loading state when useful
- Show no-results state when useful
- Hide dropdown results after item selection where appropriate
- Keep keyboard and mouse interaction understandable
- Avoid surprising state resets

Search results should be easy to scan and should include useful item context where available.

## Price formatting expectations

World of Warcraft prices are usually stored as copper.

Use or preserve a utility function to format unit price into gold, silver, and copper.

For example:

```text
1g 23s 45c
```

Keep price formatting logic outside JSX where practical.

Handle zero copper clearly.

Handle invalid values explicitly.

Do not duplicate price formatting logic across components.

## Date and time formatting expectations

Treat API timestamps as UTC unless the code or API contract clearly says otherwise.

When formatting dates:

- Normalize UTC strings carefully
- Avoid accidental local-time parsing bugs
- Keep formatting utilities separate
- Make graph tooltip dates readable
- Include time when the chart or tooltip needs it

Do not bury date parsing logic in chart JSX if a utility function would be clearer.

## Graphing expectations

Use Recharts where the project already uses it for charts.

For price and quantity graphs:

- Prefer oldest-to-newest order on the x-axis
- Label price clearly
- Label quantity clearly
- Ensure tooltip names are not swapped
- Format unit price as gold, silver, and copper
- Format timestamps into readable local time
- Keep missing data behaviour intentional
- Avoid confusing quantity values with price values

If the backend returns newest-first data and the chart needs oldest-first data, transform it clearly in the frontend or explain why the backend should change.

## Error handling and loading states

Handle frontend states explicitly.

Use:

- Loading states
- Error states
- Empty states
- Guard clauses for optional data
- Fallback images where appropriate
- Helpful variable names

Do not silently swallow errors.

Do not show misleading values when data is missing.

If a backend endpoint fails, show a reasonable frontend error state and explain the likely cause in the final summary.

## Testing expectations

Frontend tests are optional unless Callum explicitly asks or the project already has a useful frontend test setup for the code being changed.

If adding or changing pure utility logic, tests may be useful.

Good frontend test targets:

- `formatUnitPrice`
- `formatLocalDate`
- Chart data transformation helpers
- Search filtering helpers, if any
- Small deterministic utilities

Avoid writing brittle tests for Material UI layout details.

Avoid adding a whole new test framework unless Callum asks.

If tests already exist and a change affects tested logic, update the relevant tests.

## Build and check commands

After frontend changes, run the smallest relevant check.

Usually run:

```powershell
npm run build
```

If tests exist and are relevant, run:

```powershell
npm test
```

or the existing test command from `package.json`.

Always inspect `package.json` before assuming available scripts.

If a check cannot be run, say why clearly.

Do not pretend a command passed if it was not run.

## Package and dependency rules

Do not add dependencies unless there is a clear reason.

Before adding a package:

1. Check whether the existing stack can solve the problem
2. Explain why the package is needed
3. Prefer small, well-known packages
4. Avoid adding packages for trivial utilities
5. Wait for Callum's approval if the dependency is not obviously necessary

Do not run major package upgrades unless explicitly requested.

Do not casually change lockfiles unless the task requires dependency changes.

## Development workflow

Before changing code:

1. Inspect the existing frontend folder structure
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
5. Keep the frontend building after each change
6. Prefer practical progress over theoretical perfection

After changing code:

1. Run the relevant local check
2. Fix TypeScript errors
3. Fix obvious runtime errors
4. Summarize what changed
5. Provide a short proposed commit message
6. Pause for review

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
git add .\src\features\items\components\ItemSearchBar.tsx
```

Only stage files automatically if Callum explicitly asks you to stage them.

Always provide a short, copy-pasteable Conventional Commit message after each logical change.

Prefer small commit messages.

Use this format:

```text
feat: add item price history graph
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

Each proposed commit should represent one logical frontend change.

Do not bundle unrelated changes into one commit.

Each commit should leave the frontend in a working state.

## Commit boundary expectations

After suggesting a commit message, explain briefly why the change belongs in one commit.

If the work should be split, suggest the split.

Good frontend commit boundary examples:

- One commit for adding a frontend API function
- One commit for adding a React Query hook
- One commit for adding a presentational component
- One commit for wiring a component into a page
- One commit for fixing tooltip formatting
- One commit for adding a utility function and tests
- One commit for visual polish
- One commit for documentation updates

Avoid commits that mix unrelated features, styling, dependency changes, and refactors.

## Scope boundaries

Do not add these unless explicitly requested:

- New backend endpoints
- Backend code changes
- Database changes
- Authentication
- User accounts
- Payment features
- Global state libraries
- New frontend frameworks
- Complex routing
- Animation libraries
- Major theme rewrites
- Large design systems
- New test frameworks
- Deployment configuration changes outside the frontend folder

If a task seems to require one of these, explain the trade-off and suggest the smallest frontend-only alternative first.

## Clean code principles

Write code for humans first.

Use:

- Descriptive names
- Small components
- Small functions
- Clear file responsibilities
- Explicit control flow
- Guard clauses and early returns
- Constants instead of magic strings or numbers
- Consistent formatting
- Simple data shapes
- Clear seams between API, hooks, components, pages, and utilities

Avoid:

- Ternary operators
- Clever one-liners
- Deeply nested logic
- Vague names like `data`, `temp`, `foo`, `result2`, or `x`
- Copy-pasted logic
- Unnecessary abstraction
- Mixed concerns
- Hidden dependencies
- CommonJS syntax
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

```ts
// TODO: Add keyboard selection after the mouse selection flow is stable.
// FIXME: Replace temporary empty state once the API returns richer metadata.
// HACK: Keeps chart spacing stable until missing snapshot handling is moved server-side.
```

Do not write comments that may quickly become false.

Delete outdated comments.

Do not add AI-generated signature comments.

Do not add:

```text
Generated with Claude Code
Co-Authored-By: Claude
```

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

## Frontend feature implementation guidance

When adding a frontend feature, prefer this order:

1. Inspect the existing feature folder
2. Define or inspect the TypeScript type
3. Add or update the API client function
4. Add or update the React Query hook
5. Add or update the presentational component
6. Wire the component into the page
7. Add simple loading, error, and empty states
8. Run the production build
9. Suggest a small commit message
10. Pause for review

Keep each step small enough to review.

If the feature spans multiple layers, split it into small frontend-only commits.

## Manual review checklist

Before considering a frontend change complete, check:

- The frontend builds
- TypeScript errors are fixed
- UI still loads locally
- API URLs are still centralized
- Query keys are sensible
- Loading states work
- Error states work
- Empty states work
- Date formatting is correct
- Price formatting is correct
- Chart labels and tooltips are correct
- Mobile layout is not obviously broken
- No backend files were changed
- No unrelated files were changed
- No secrets are present
- Suggested commit message is provided
- Commit boundary is clear

## Final response format after a frontend code change

After making a frontend code change, respond with:

```text
Files changed:
- path/to/file

What changed:
- ...

Why:
- ...

Checks:
- npm run build passed
```

If checks were not run, say:

```text
Checks:
- Not run. Reason: ...
```

Then include:

```text
Suggested commit message:
feat: add item price history graph
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

Callum wants small, reviewable frontend changes.

Do not rush through large automated edits.

Do not edit backend files.

Do not commit.

Do not push.

Do not silently stage files.

Do not hide uncertainty.

Read before editing.

Prefer one clean, working frontend change over five half-finished changes.

The best outcome is frontend code Callum understands, trusts, and can explain in an interview.
