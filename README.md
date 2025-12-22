# MyMovie

## 1. Project Overview
**MyMovie** je backend aplikacija izrađena u .NET 8 koristeći **Clean Architecture**.  
Cilj projekta je prikupljanje i čuvanje podataka o filmovima iz **TMDB API-ja** u SQL Server bazu.  

**Tehnologije:**
- .NET 8
- C#  
- EF Core (Code First)  
- SQL Server (Docker)
- TMDB API
- Clean Architecture (API, Application, Infrastructure, Domain)

---

## 2. Architecture

Projekt koristi **Clean Architecture** sa sljedećim slojevima:
- MyMovie.API → Endpoints / Controllers
- MyMovie.Application → Business logic / Services / DTOs
- MyMovie.Domain → Entities / Interfaces
- MyMovie.Infrastructure → Database access / Repositories / External API services

## 3. Database

SQL Server je pokrenut u Dockeru
DbContext: MoviesAppContext
Konekcija: appsettings.json + Program.cs

### Migracije
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
