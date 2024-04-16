#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Portal.Web.Mvc/Portal.Web.Mvc.csproj", "src/Portal.Web.Mvc/"]
COPY ["src/Portal.Web.Core/Portal.Web.Core.csproj", "src/Portal.Web.Core/"]
COPY ["src/Portal.Application/Portal.Application.csproj", "src/Portal.Application/"]
COPY ["src/Portal.Application.Shared/Portal.Application.Shared.csproj", "src/Portal.Application.Shared/"]
COPY ["src/Portal.Core.Shared/Portal.Core.Shared.csproj", "src/Portal.Core.Shared/"]
COPY ["src/Portal.Core/Portal.Core.csproj", "src/Portal.Core/"]
COPY ["src/Portal.EntityFrameworkCore/Portal.EntityFrameworkCore.csproj", "src/Portal.EntityFrameworkCore/"]
COPY ["src/Portal.GraphQL/Portal.GraphQL.csproj", "src/Portal.GraphQL/"]
RUN dotnet restore "src/Portal.Web.Mvc/Portal.Web.Mvc.csproj"
COPY . .
WORKDIR "/src/src/Portal.Web.Mvc"
RUN dotnet build "Portal.Web.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Portal.Web.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portal.Web.Mvc.dll"]
