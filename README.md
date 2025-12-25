# 🎬 MyMovie

MyMovie is a full-stack web application for discovering, managing, and tracking movies and TV shows.  
The application integrates with **The Movie Database (TMDB)** for movie and TV data and uses **Firebase Authentication** for user management.

The project is built as a learning and portfolio application, following **Clean Architecture principles** on the backend and a modern **React-based frontend**.

## Features

- Browse popular, trending, and top-rated movies and TV shows
- View detailed information (overview, rating, release date, etc.)
- User authentication (Register / Login) via Firebase
- Personal Watchlist (per authenticated user)
- Backend API with clean separation of concerns
- External API integration (TMDB)
- SQL Server database for persistent data storage

## Tech Stack

### Backend
- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **Clean Architecture**
  - MyMovie.Api
  - MyMovie.Application
  - MyMovie.Domain
  - MyMovie.Infrastructure

### Frontend
- **React**
- **React Router**
- **Firebase Authentication**
- **Fetch / Axios for API calls**

### External Services
- **TMDB API** – movie & TV data
- **Firebase** – authentication

##  Environment Configuration
This project requires API keys for **TMDB** and **Firebase**.

### TMDB
Create an account at https://www.themoviedb.org/ and generate an API key.

### Firebase
Create a Firebase project and enable **Email/Password Authentication**.


