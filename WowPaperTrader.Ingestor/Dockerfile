FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["WowPaperTrader.Ingestor/WowPaperTrader.Ingestor.csproj", "WowPaperTrader.Ingestor/"]
COPY ["WowPaperTrader.Application/WowPaperTrader.Application.csproj", "WowPaperTrader.Application/"]
COPY ["WowPaperTrader.Infrastructure/WowPaperTrader.Infrastructure.csproj", "WowPaperTrader.Infrastructure/"]
COPY ["WowPaperTrader.Persistence/WowPaperTrader.Persistence.csproj", "WowPaperTrader.Persistence/"]

RUN dotnet restore "WowPaperTrader.Ingestor/WowPaperTrader.Ingestor.csproj"

COPY . .

WORKDIR "/src/WowPaperTrader.Ingestor"

RUN dotnet publish \
    "WowPaperTrader.Ingestor.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=build /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "WowPaperTrader.Ingestor.dll"]