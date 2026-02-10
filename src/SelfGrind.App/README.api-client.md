# API Client Generation Guide

This project uses Swagger (Swashbuckle CLI) and Kiota CLI to generate TypeScript API clients from the .NET backend.

## Prerequisites

- .NET SDK 10.0 or newer
- Node.js and npm

## 1. Install Swagger CLI (Swashbuckle)

This tool generates the OpenAPI (swagger) JSON from your .NET API project.

Open a terminal and run:

```
dotnet tool install --global Swashbuckle.AspNetCore.Cli
```

If you update Swashbuckle.AspNetCore in your project, update the CLI tool as well:

```
dotnet tool update --global Swashbuckle.AspNetCore.Cli
```

## 2. Install Kiota CLI

This tool generates the TypeScript client from the OpenAPI JSON.

```
dotnet tool install --global Microsoft.OpenApi.Kiota
```

If you update Kiota, use:

```
dotnet tool update --global Microsoft.OpenApi.Kiota
```

## 3. Add .NET Tools to PATH (if needed)

Ensure your PATH includes the .NET tools directory:

- Windows: `%USERPROFILE%\.dotnet\tools`
- Linux/macOS: `$HOME/.dotnet/tools`

## 4. Usage

### Build the .NET API project

```
dotnet build ../SelfGrind.API/SelfGrind.API.csproj
```

### Generate OpenAPI JSON

```
npm run build:api-file
```

This runs:

```
swagger tofile --output ./src/api/api.json ../SelfGrind.API/bin/Debug/net10.0/SelfGrind.API.dll v1
```

### Generate TypeScript API Client

```
npm run build:api-client
```

This runs:

```
kiota generate --language typescript --openapi ./src/api/api.json --class-name apiClient --output ./src/api/apiClient --clean-output
```

### Full API Client Build

To run both steps:

```
npm run build:api
```

---

If you encounter errors about missing commands, ensure the tools are installed and your PATH is set correctly. For further help, see the official docs:

- Swashbuckle CLI: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
- Kiota CLI: https://github.com/microsoft/kiota
