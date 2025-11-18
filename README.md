# SmartBudget API

Advanced Budget Management API built with .NET 9 and Clean Architecture.

## Technologies

- .NET 9
- Entity Framework Core
- SQL Server
- MediatR (CQRS)
- FluentValidation
- JWT Authentication
- Swagger/OpenAPI

## Architecture

- **Domain Layer** - Entities, Value Objects, Interfaces
- **Application Layer** - CQRS Commands/Queries, DTOs, Validators
- **Infrastructure Layer** - EF Core, Repositories, Services
- **API Layer** - Controllers, Middleware

## Getting Started

1. Update connection string in `appsettings.json`
2. Run migrations: `dotnet ef database update --project SmartBudgetAPI.Infrastructure --startup-project SmartBudgetAPI`
3. Run the API: `dotnet run --project SmartBudgetAPI`
4. Open Swagger UI: `https://localhost:5001`

## Features

- User Authentication (JWT)
- Budget Management
- Transaction Tracking
- Category Management
- Budget Alerts

## License

MIT

