# MyMovie

## 1. Project Overview
**MyMovie** is a web application made with .NET 8 (Clean Architecture) and React for frontend.
The goal of the project is to collect and store movie data from the TMDB API in a SQL Server database.

**Technologies:**
- .NET 8
- C#  
- EF Core 
- SQL Server (Docker)
- TMDB API
- Clean Architecture (API, Application, Infrastructure, Domain)

---

## 2. Architecture
- MyMovie.API → Endpoints / Controllers
- MyMovie.Application → Business logic / Services / DTOs
- MyMovie.Domain → Entities / Interfaces
- MyMovie.Infrastructure → Database access / Repositories / External API services

## 3. Database
SQL Server is running on Docker
DbContext: MoviesAppContext
Connection: appsettings.json + Program.cs

### Migrations
dotnet ef migrations add InitialCreate -p MyMovie.Infrastructure -s MyMovie.Api
dotnet ef database update -p MyMovie.Infrastructure -s MyMovie.Api

## How to Run

Prerequisites:
- Docker (SQL Server)
- .NET 8 SDK
- TMDB API Key

### How to get TMDB API Key
- Go to TMDB Developers
- Create an account
- Go to: Settings → API → Create API Key

### Steps:
Start SQL Server in Docker
Configure appsettings.json connection string

### Run migrations:
dotnet ef database update -p MyMovie.Infrastructure -s MyMovie.Api

### Run API:
dotnet run --project MyMovie.Api
