# Luftborn Store

ASP.NET Core API + Angular frontend — CRUD for Categories and Products with Clean Architecture.

## Tech Stack

- **Backend:** .NET 10, ASP.NET Core, EF Core, SQL Server
- **Frontend:** Angular 21, TypeScript, SCSS
- **Patterns:** Clean Architecture, Repository, Unit of Work, Specification, Result pattern, Idempotency

## Solution Structure

```
Luftborn.CodeTest/
├── API/                 # ASP.NET Core Web API
├── Application/         # Services, DTOs, Mappings
├── Core/                # Entities, Interfaces, Specifications
├── Infrastructure/      # EF Core, Repositories, Data
├── client/              # Angular SPA
└── README.md
```

## Prerequisites

- .NET 10 SDK
- Node.js 18+
- SQL Server or LocalDB (for API)

## Run Locally

### 1. API

```bash
cd API
dotnet run
# HTTPS: https://localhost:7124
# HTTP:  http://localhost:5207
```

### 2. Client (Angular)

```bash
cd client
npm install
npm start
# http://localhost:4200
```

Ensure the API is running so the client proxy (`/api` → API) works.

### 3. Database

- Connection string in `API/appsettings.json` (DefaultConnection).
- Migrations run on startup. For design-time:  
  `dotnet ef migrations add <Name> -p Infrastructure -s API`

## API Overview

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/categories | List categories |
| GET | /api/categories/{id} | Get category by id |
| POST | /api/categories | Create category |
| PUT | /api/categories/{id} | Update category |
| DELETE | /api/categories/{id} | Delete category |
| GET | /api/products | List products (pagination, filter by category) |
| GET | /api/products/{id} | Get product by id |
| POST | /api/products | Create product |
| PUT | /api/products/{id} | Update product |
| DELETE | /api/products/{id} | Delete product |

All responses use a consistent **Result pattern** (`success`, `data`, `message`, `errors`).  
**Idempotency:** Send header `Idempotency-Key` (e.g. UUID) on POST/PUT/PATCH to avoid duplicate operations on retry.

## Postman

Import `Luftborn.API.postman_collection.json` and set `baseUrl` (e.g. `https://localhost:7124`).

## License

MIT
