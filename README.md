# SafeMugs | Loja Vulnerável

Projeto de Trabalho de Graduação (TG) criado para simular uma loja virtual com vulnerabilidades de segurança para fins acadêmicos e educacionais.

## Integrantes

- Eunata Vinicius Oliveira Ferpa
- Gustavo Araujo Pereira
- Nicollas Marques Strumillo

## Visão Geral

O repositório contém:

- backend: API em ASP.NET Core com Entity Framework Core e MySQL
- frontend: aplicação Angular com Angular Material

## Stack e Versões (atuais do projeto)

### Backend

| Tecnologia | Versão |
|---|---|
| .NET / ASP.NET Core | net9.0 |
| Microsoft.AspNetCore.OpenApi | 9.0.15 |
| Microsoft.EntityFrameworkCore.Design | 9.0.0 |
| Pomelo.EntityFrameworkCore.MySql | 9.0.0 |
| Banco de dados | MySQL (conexão em `appsettings.Development.json`) |

### Frontend

| Tecnologia | Versão |
|---|---|
| Angular (core/common/router/forms etc.) | 20.3.x |
| Angular CLI | 20.3.5 |
| Angular Material | 20.2.14 |
| Angular CDK | 20.2.14 |
| TypeScript | 5.9.2 |
| RxJS | 7.8.x |
| Zone.js | 0.15.x |

## Pré-requisitos

- .NET SDK 9
- Node.js (recomendado: LTS compatível com Angular 20)
- Angular CLI 20 
- MySQL 8.0+

## Configuração do Banco (Backend)

A conexão de desenvolvimento está definida em `backend/appsettings.Development.json`:

```json
"ConnectionStrings": {
	"DefaultConnection": "server=localhost;port=3306;Database=Mugs;uid=root;password=root;"
}
```

Se necessário, ajuste host, usuário e senha para o seu ambiente local.

## Como Executar

### 1) Backend

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run --launch-profile https
```

### 2) Frontend

Em outro terminal:

```bash
cd frontend
npm install
npx ng serve --configuration development
```

## Observação

Sempre que fizer pull e perceber que uma nova migration foi adicionada, rode este comando para atualizar o banco de dados:

```bash
cd backend
dotnet ef database update
```

### Atribuição de créditos
Todas as imagens de produtos nesta aplicação foram tiradas do site: https://unsplash.com