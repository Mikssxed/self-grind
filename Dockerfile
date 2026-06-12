# syntax=docker/dockerfile:1

# ---- Stage 1: build the Vue SPA ----
# Vite is configured (vite.config.ts) to output into ../SelfGrind.API/wwwroot, so we keep the same
# folder layout here and let the build write into /src/SelfGrind.API/wwwroot.
FROM node:22 AS frontend
WORKDIR /src/SelfGrind.App

# Install deps first for better layer caching
COPY src/SelfGrind.App/package.json src/SelfGrind.App/package-lock.json ./
RUN npm ci

# Build the SPA. The GA4 measurement id is a build-time value baked into the bundle (not secret).
COPY src/SelfGrind.App/ ./
ARG VITE_GA_MEASUREMENT_ID=""
ENV VITE_GA_MEASUREMENT_ID=$VITE_GA_MEASUREMENT_ID
RUN npm run build   # -> /src/SelfGrind.API/wwwroot

# ---- Stage 2: publish the .NET API (which will serve the SPA) ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend
WORKDIR /src

# Restore against the project graph first for layer caching
COPY src/SelfGrind.Domain/*.csproj src/SelfGrind.Domain/
COPY src/SelfGrind.Application/*.csproj src/SelfGrind.Application/
COPY src/SelfGrind.Infrastructure/*.csproj src/SelfGrind.Infrastructure/
COPY src/SelfGrind.API/*.csproj src/SelfGrind.API/
RUN dotnet restore src/SelfGrind.API/SelfGrind.API.csproj

# Copy the rest of the source, then overlay the built SPA so it is not overwritten
COPY src/ src/
COPY --from=frontend /src/SelfGrind.API/wwwroot src/SelfGrind.API/wwwroot

RUN dotnet publish src/SelfGrind.API/SelfGrind.API.csproj -c Release -o /publish --no-restore

# ---- Stage 3: runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=backend /publish ./

# Azure Container Apps routes to this port; ASPNETCORE_ENVIRONMENT defaults to Production here.
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SelfGrind.API.dll"]
