# Stage 1: Build the Angular app
FROM node:latest as build-client
WORKDIR /app
COPY azcoreweb.client/package*.json ./
RUN npm install
COPY azcoreweb.client .
RUN npm run build

# Stage 2: Build the .NET API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-api
WORKDIR /src
COPY AzCoreWeb.Server .
RUN dotnet restore AzCoreWeb.Server.csproj
#COPY . .
RUN dotnet publish AzCoreWeb.Server.csproj -c Release -o /app/publish

# Stage 3: Create the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-api /app/publish .
COPY --from=build-client /app/dist/azcoreweb.client/browser ./wwwroot
#ENV ASPNETCORE_URLS=http://localhost:5000
ENTRYPOINT ["dotnet", "AzCoreWeb.Server.dll"]