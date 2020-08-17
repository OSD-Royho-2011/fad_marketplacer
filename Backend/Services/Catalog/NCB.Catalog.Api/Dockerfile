#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Catalog/NCB.Catalog.Api/NCB.Catalog.Api.csproj", "Services/Catalog/NCB.Catalog.Api/"]
COPY ["Share/NCB.Core/NCB.Core.csproj", "Share/NCB.Core/"]
COPY ["Share/NCB.EventBus/NCB.EventBus.csproj", "Share/NCB.EventBus/"]
RUN dotnet restore "Services/Catalog/NCB.Catalog.Api/NCB.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/NCB.Catalog.Api"
RUN dotnet build "NCB.Catalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NCB.Catalog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NCB.Catalog.Api.dll"]
