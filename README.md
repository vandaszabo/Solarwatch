# SolarwatchApp
A web application for getting sunrise and sunset times by city names.

## Database Setup with Entity Framework and MSSQL

### Prerequisites

Get an API Key from the following website: https://openweathermap.org/api/geocoding-api.
Set the apikey as Environmental variable named "ASPNETCORE_APIKEY".

Before you begin, ensure you have the following installed:

- Microsoft SQL Server
- [.NET SDK 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)

### Entity Framework Migrations

1. **Create Migrations:**
   - Open a terminal or command prompt.
   - Navigate to the project's backend directory. (SolarWatch)
   - Run the following commands to create migrations for the UsersContext and SolarWatchContext:
       - dotnet ef migrations add InitialCreate --context UsersContext
       - dotnet ef migrations add InitialCreate --context SolarWatchContext
2. **Apply the migrations to update the databases:**
   - Run the following commands to update your database:
       - dotnet ef database update --context UsersContext
       - dotnet ef database update --context SolarWatchContext
     
### Start the application

3. **Running the Project**
   - Start the backend server by running the following command:
       - dotnet run
   - Open a new terminal or command prompt.
   - Navigate to the project's frontend directory. (solarwatch-app)
   - Start the frontend application:
       - npm start

