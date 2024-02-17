#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SampeApp.Database.Migrations/SampeApp.Database.Migrations.csproj", "SampeApp.Database.Migrations/"]
RUN dotnet restore "SampeApp.Database.Migrations/SampeApp.Database.Migrations.csproj"
COPY . .
WORKDIR "/src/SampeApp.Database.Migrations"
RUN dotnet build "SampeApp.Database.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampeApp.Database.Migrations.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampeApp.Database.Migrations.dll"]