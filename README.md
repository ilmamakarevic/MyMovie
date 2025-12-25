# 🎬 MyMovie

MyMovie is a full-stack web application for discovering, managing, and tracking movies and TV shows.  
The application integrates with The Movie Database (TMDB) for movies and TV shows data, and uses Firebase Authentication for user management.

The project is built following Clean Architecture principles on the backend and a modern React-based frontend.

## Features

- Browse popular, trending, and top-rated movies and TV shows
- View detailed information (overview, rating)
- User authentication (Register / Login) via Firebase
- Personal Watchlist (per authenticated user)
- Backend API with clean separation of concerns
- External API integration (TMDB)
- SQL Server database for persistent data storage

## Tech Stack

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Clean Architecture
  - MyMovie.Api
  - MyMovie.Application
  - MyMovie.Domain
  - MyMovie.Infrastructure

### Frontend
- React
- Firebase Authentication

### External Services
- TMDB API – movie & TV data
- Firebase – authentication

##  Environment Configuration
This project requires API keys for TMDB and Firebase.

### Prerequisites:
- .NET 8 
- SQL Server 
- TMDB API Key 
- Firebase API credentials

### TMDB
1. Create an account at https://www.themoviedb.org/ and generate an API key.
2. Navigate into server
3. The base API settings are stored in the project's configuration file. Ensure your appsettings.json contains the following:
```{
  "TMDB": {
    "BaseUrl": "https://api.themoviedb.org/3/"
  }
}
```

5. Open a terminal in the project's root directory and run: `dotnet user-secrets init`
   
6. Add your Key: Run the following command (replace YOUR_API_KEY_HERE with your actual key):
```
dotnet user-secrets set "TMDB:ApiKey" "YOUR_API_KEY_HERE"
```
### Firebase
1. Go to https://console.firebase.google.com/ and create a Firebase project and enable Email/Password Authentication.
2. Create a file named .env in the root of your frontend directory. Add your Firebase credentials there:
```REACT_APP_FIREBASE_API_KEY=your_api_key_here
REACT_APP_FIREBASE_AUTH_DOMAIN=your_project_id.firebaseapp.com
REACT_APP_FIREBASE_PROJECT_ID=your_project_id
REACT_APP_FIREBASE_STORAGE_BUCKET=your_project_id.appspot.com
REACT_APP_FIREBASE_MESSAGING_SENDER_ID=your_sender_id
REACT_APP_FIREBASE_APP_ID=your_app_id
```

### Install

1. Open your Terminal window (Command Prompt, PowerShell, bash, zsh, etc.) and clone this repository to any location on your PC:
```
git clone https://github.com/ilmamakarevic/MyMovie.git
```

2. Navigate into client directory and install required dependencies:
```
cd client
npm install
npm install react-icons
```

3. Navigate to server and apply migrations to create the schema:
```
database update --project MyMovie.Infrastructure --startup-project MyMovie.Api
```

### Run the application:
1. Before starting the API, your database must be running:

   a) Using Docker - If you have Docker installed, run this command to start a SQL Server container:
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!" -p 1433:1433 --name sql_server_container -d mcr.microsoft.com/mssql/server:2022-latest
```
   b) Local Installation - Ensure the SQL Server (MSSQLSERVER) service is running in your Windows Services.

3. Navigate to server and Api folder and start the app:
```
cd server/MyMovie.Api
dotnet restore
dotnet run
```
  
After this you should see that the application started on localhost:5081 and you can navigate to localhost:5081/swagger.

3. Open new terminal and navigate to frontend folder:
```
cd client
```
  
4. Run the application
```
npm start
```
  
After this you should see that the application started on localhost:3000. 


