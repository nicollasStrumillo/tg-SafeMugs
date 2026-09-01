# SafeMugs | Loja Vulnerável

Projeto de Trabalho de Graduação (TG) — loja virtual **intencionalmente vulnerável** para ensino de segurança ofensiva.

## Integrantes

- Eunata Vinicius Oliveira Ferpa
- Gustavo Araujo Pereira
- Nicollas Marques Strumillo

## Visão Geral

- `backend/` — API ASP.NET Core + EF Core + MySQL (Pomelo)
- `frontend/` — Angular + Angular Material (SPA)

## Stack e Versões

### Backend (`backend/backend.csproj`)

| Tecnologia | Versão |
|---|---|
| .NET / ASP.NET Core | `net9.0` (SDK `9.0`) |
| `Microsoft.AspNetCore.OpenApi` | `9.0.15` |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | `9.0.0` |
| `Microsoft.EntityFrameworkCore.Design` | `9.0.0` |
| `Pomelo.EntityFrameworkCore.MySql` | `9.0.0` |
| `Hashids.net` | `1.7.0` |
| Banco de dados | MySQL `8.4` (imagem `mysql:8.4` no Docker; `8.0+` local) |

### Frontend (`frontend/package.json`)

| Tecnologia | Versão |
|---|---|
| Angular (core/common/router/forms/compiler/animations) | `20.3.x` (`^20.3.0`, `20.3.18` resolvido) |
| Angular CLI / Angular Build | `20.3.5` (`^20.3.5`) |
| Angular Material / CDK | `20.2.14` |
| TypeScript | `5.9.2` (`~5.9.2`) |
| RxJS | `7.8.x` (`~7.8.0`) |
| Zone.js | `0.15.x` (`~0.15.0`) |
| `@microsoft/signalr` | `^10.0.0` |
| `highlight.js` | `^11.11.1` |

> Imagens Docker: `mcr.microsoft.com/dotnet/sdk:9.0` + `aspnet:9.0`, `node:22-alpine`, `nginx:1.27-alpine`.

## Pré-requisitos

| Modo | O que precisa |
|---|---|
| **Desenvolvimento** | .NET SDK `9.0`, Node.js `22` (ou `^20.19`/`>=24` para Angular 20), MySQL `8.0+` rodando local |
| **Docker** | Docker Desktop (Engine + Compose v2) — Node/.NET/MySQL **não** precisam estar instalados |

## Como Rodar — Desenvolvimento (sem Docker)

> Backend em `https://localhost:7224` + `http://localhost:5242` (`launchSettings` profile `https`). Frontend em `http://localhost:4200` com proxy para `/api`, `/imagens` e `/notifications`.

```powershell
# 1. clone
git clone <url-do-repo> SafeMugs
cd SafeMugs

# 2. backend — em um terminal
cd backend
dotnet restore
dotnet ef database update          
dotnet run --launch-profile https  

# 3. frontend — em outro terminal
cd frontend
npm install
npx ng serve --configuration development 
```

Acesse `http://localhost:4200`. Conexão dev em `backend/appsettings.Development.json`: `server=localhost;port=3306;Database=Mugs;uid=root;password=root;`.

Após `git pull` com nova migration: `cd backend && dotnet ef database update`.

## Como Rodar — Docker (do zero)

> Sobe 3 containers: `db` (MySQL 8.4), `backend` (`8080`) e `frontend` (nginx em `4200` proxies `/api` e `/notifications` para `backend:8080`). Build aplica migrations e seed automaticamente (`AutoRecover`).

```powershell
git clone <url-do-repo> SafeMugs
cd SafeMugs

docker compose up --build -d      

# parar
docker compose down            
```

Acesse:

- Frontend: `http://localhost:4200`
- Backend: `http://localhost:8080` (`/api`, `/imagens`, `/notifications`)
- MySQL: interno `db:3306` (externo não exposto; backend usa `server=db` via `ConnectionStrings__DefaultConnection`)

Não precisa `dotnet ef database update` nem `npm install` no Docker — o backend roda `MigrateAsync()` no boot.

## Atribuição de créditos

Todas as imagens de produtos foram retiradas de https://unsplash.com
