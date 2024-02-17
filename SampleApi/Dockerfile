#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SampleApp/SampleApi.csproj", "SampleApp/"]
COPY ["SampleApi.Application/SampleApi.Application.csproj", "SampleApi.Application/"]
COPY ["SampleApi.Domain/SampleApi.Domain.csproj", "SampleApi.Domain/"]
COPY ["SampleApi.Database/SampleApi.Database.csproj", "SampleApi.Database/"]
RUN dotnet restore "SampleApp/SampleApi.csproj"
COPY . .
WORKDIR "/src/SampleApp"
RUN dotnet build "SampleApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleApi.dll"]