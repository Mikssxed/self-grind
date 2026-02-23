# SelfGrind

A task management application built with .NET 10 and Vue 3.

## Prerequisites (What to Install on a Clean Computer)

### Required Software

1. **.NET 10 SDK**
   - Download from: https://dotnet.microsoft.com/download/dotnet/10.0
   - Verify installation: `dotnet --version`

2. **SQL Server LocalDB** (comes with Visual Studio or standalone)
   - **Option A**: Install Visual Studio 2022 (Community Edition is free)
     - During installation, select "Data storage and processing" workload
   - **Option B**: Download standalone SQL Server Express LocalDB
     - Download from: https://aka.ms/sslocaldbinstaller
   - Verify installation: `sqllocaldb info`

3. **Node.js** (version 18 or higher)
   - Download from: https://nodejs.org/
   - Verify installation: `node --version` and `npm --version`

### Optional Tools

- **Visual Studio 2022** or **JetBrains Rider** (recommended for backend development)
- **VS Code** (recommended for frontend development)
- **SQL Server Management Studio (SSMS)** for database management

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd SelfGrind
```

### 2. Backend Setup

#### Restore Dependencies

```bash
cd src/SelfGrind.API
dotnet restore
```

#### Configure Database Connection (Optional)

The app uses SQL Server LocalDB by default. The connection string is in `appsettings.Development.json`:

```json
"ConnectionStrings": {
  "SelfGrindDb": "Server=(localdb)\\mssqllocaldb;Database=SelfGrindDb;Trusted_Connection=True;"
}
```

**Note**: The database will be created automatically on first run!

#### Run the Backend

```bash
dotnet run
```

The API will start at:

- HTTPS: `https://localhost:5081`
- HTTP: `http://localhost:5080`
- Swagger UI: `https://localhost:5081/swagger`

### 3. Frontend Setup

#### Navigate to Frontend Directory

```bash
cd src/SelfGrind.App
```

#### Install Dependencies

```bash
npm install
```

#### Run the Frontend

```bash
npm run dev
```

The app will start at: `http://localhost:5173`

## Project Structure

```
SelfGrind/
├── src/
│   ├── SelfGrind.API/          # Web API (Entry point)
│   ├── SelfGrind.Application/  # Business logic & CQRS handlers
│   ├── SelfGrind.Domain/       # Domain entities & interfaces
│   ├── SelfGrind.Infrastructure/ # Database & external services
│   └── SelfGrind.App/          # Vue 3 frontend
└── SelfGrind.sln               # Solution file
```

## Architecture

- **Backend**: Clean Architecture with CQRS pattern (MediatR)
- **Frontend**: Vue 3 with Composition API, TypeScript, Vite
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT tokens with ASP.NET Identity
- **API Client**: Generated with Microsoft Kiota

## Common Commands

### Backend

```bash
# Run the API in development mode
cd src/SelfGrind.API
dotnet run

# Run with watch (auto-restart on changes)
dotnet watch run

# Build the solution
dotnet build

# Create a new migration
dotnet ef migrations add MigrationName --project src/SelfGrind.Infrastructure --startup-project src/SelfGrind.API

# Update database manually (not needed - auto-migrates on startup)
dotnet ef database update --project src/SelfGrind.Infrastructure --startup-project src/SelfGrind.API
```

### Frontend

```bash
cd src/SelfGrind.App

# Start development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Regenerate API client (after backend changes)
npm run build:api
```

## Development Workflow

1. **Start the backend first**: `cd src/SelfGrind.API && dotnet run`
2. **Then start the frontend**: `cd src/SelfGrind.App && npm run dev`
3. **Access the app**: Open `http://localhost:5173` in your browser

The database will be created and migrated automatically on the first run!

## Troubleshooting

### Database Issues

- **LocalDB not found**: Install SQL Server Express LocalDB
- **Migration errors**: The app auto-migrates on startup, but you can manually run:
  ```bash
  dotnet ef database update --project src/SelfGrind.Infrastructure --startup-project src/SelfGrind.API
  ```

### Backend Issues

- **Port already in use**: Check `Properties/launchSettings.json` to change ports
- **JWT errors**: Ensure `JwtSettings` are configured in `appsettings.Development.json`

### Frontend Issues

- **API connection errors**: Ensure backend is running on `http://localhost:5081`
- **Module not found**: Run `npm install` again
- **Port 5173 in use**: Vite will automatically try the next available port

## Configuration Files

### Backend Configuration

- `appsettings.json` - Production settings
- `appsettings.Development.json` - Development settings (connection strings, JWT secrets)

### Frontend Configuration

- `vite.config.ts` - Vite configuration with proxy to backend API

## Contributing

1. Create a feature branch
2. Make your changes
3. Test both backend and frontend
4. Submit a pull request

## License

[Add your license information here]
