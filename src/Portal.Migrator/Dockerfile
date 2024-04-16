#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Portal.Migrator/Portal.Migrator.csproj", "src/Portal.Migrator/"]
COPY ["src/Portal.EntityFrameworkCore/Portal.EntityFrameworkCore.csproj", "src/Portal.EntityFrameworkCore/"]
COPY ["src/Portal.Core/Portal.Core.csproj", "src/Portal.Core/"]
COPY ["src/Portal.Core.Shared/Portal.Core.Shared.csproj", "src/Portal.Core.Shared/"]
RUN dotnet restore "src/Portal.Migrator/Portal.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Portal.Migrator"
RUN dotnet build "Portal.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Portal.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portal.Migrator.dll", "-s"]
