# SafeMugs — Agent Guide

## Project type

Intentionally vulnerable e-commerce store for security training, inspired by **OWASP Juice Shop**. This is an academic TG (undergraduate thesis) — a controlled laboratory environment, **not production software**.

**Do not fix vulnerabilities** — they are deliberately inserted for ethical hacking training.

## Sole objective

Create a **security training platform** where users can:
- Identify and exploit intentionally vulnerable flows
- Track progress via a gamified scoreboard
- Request hints 

## Theme

**SafeMugs** — a fictional personalized mugs store. The e-commerce theme is secondary to the security training goal. It exists to provide realistic attack surfaces.

## Vulnerability targets

| Category | Example surfaces |
|---|---|
| Broken Authentication | Login, session, weak passwords, user enumeration |
| Broken Access Control | Admin area, IDOR, privilege escalation |
| SQL Injection | Product search, filters, parameters |
| Cross-Site Scripting (XSS) | Reviews (stored), search (reflected) |
| Open Redirect | External links without destination validation |
| SSRF | Image upload, URL fetching |
| XXE | XML parsing, upload features |
| SSTI | Template-based message/note customization |
| IDOR | Order/cart/review belonging to other users |
| Integrity failures | Price/quantity tampering in cart |
| Data exposure | Inadequate authorization on sensitive endpoints |

All vulnerabilities must be linked to a **trackable challenge** on the scoreboard.

## Monorepo layout

| Directory | Tech | Entrypoint |
|-----------|------|------------|
| `backend/` | ASP.NET Core 9 + EF Core 9 + MySQL (Pomelo) | `backend/Program.cs` |
| `frontend/` | Angular 20.3.x + Angular Material 20 | `frontend/src/main.ts` |

## Commands

### Backend (from `backend/`)

```powershell
dotnet restore
dotnet ef database update    # requires MySQL running
dotnet run --launch-profile https   # serves at https://localhost:7224
```

Default DB: `server=localhost;port=3306;Database=Mugs;uid=root;password=root` (from `appsettings.Development.json`).

### Frontend (from `frontend/`)

```powershell
npm install
npx ng serve --configuration development   # serves at http://localhost:4200
npx ng test                                 # Jasmine/Karma
npx ng build
```

**Must** pass `--configuration development` (not the default). The proxy config (`src/proxy.conf.json`) routes `/api`, `/imagens`, `/notifications` to the backend.

### Run both

Terminal 1: `cd backend && dotnet run --launch-profile https`
Terminal 2: `cd frontend && npx ng serve --configuration development`

### Docker
The plan is to package the entire application into an image so that it can be run in Docker in the future.

## Framework quirks

- **Component prefix**: `sm` (e.g., `<sm-root>`, `<sm-footer>`)
- **Standalone components** only (no NgModules)
- **Change detection**: `OnPush` everywhere
- **State**: Signals (`input()`, `output()`, `computed()`, `update`/`set` — no `mutate`)
- **Templates**: Native control flow (`@if`, `@for`, `@switch`)
- **Auth**: JWT stored in cookie `safemugs.token`, attached via `HttpInterceptor`
- **Real-time**: SignalR hub at `/notifications`
- **Prettier** (embedded in `package.json`): `printWidth: 100`, `singleQuote: true`, HTML parser for `*.html`

## Testing

- No tests at all.

## Scoreboard mechanics

The scoreboard is the **central gamification hub** (like Juice Shop):
- Each vulnerability/challenge has its own completion state
- Challenges are unlocked when the user successfully exploits them
- System calculates percentage progress
- Hints are revealed incrementally on demand
- Challenges are organized by category

## Implementation directives

- Keep UI clean, functional, and simple — do not distract from the didactic goal
- Use Angular Material components consistently
- Every vulnerability must map to a trackable scoreboard challenge
- Prefer attack surfaces that are easy to explain and test
- Avoid complexity that doesn't serve the pedagogical objective
- Maintain coherence with the mugs e-commerce theme

## Existing instruction files (read these too)
- `.agents/skills/angular-developer/SKILL.md` — detailed Angular patterns

## MCP
When you need to search docs, use `context7` tools.

## Generated / seed data

- `backend/Data/ApplicationDbContext.cs` seeds users, products, challenges (10 desafios), comments, reviews
- EF Migrations in `backend/Migrations/` — use `dotnet ef database update` to apply
- Challenge hints in `DicaDesafio` table
- Passwords are MD5-hashed (intentionally weak)
