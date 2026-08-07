FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["WowPaperTrader.Api/WowPaperTrader.Api.csproj", "WowPaperTrader.Api/"]
COPY ["WowPaperTrader.Application/WowPaperTrader.Application.csproj", "WowPaperTrader.Application/"]
COPY ["WowPaperTrader.Infrastructure/WowPaperTrader.Infrastructure.csproj", "WowPaperTrader.Infrastructure/"]
COPY ["WowPaperTrader.Persistence/WowPaperTrader.Persistence.csproj", "WowPaperTrader.Persistence/"]

RUN dotnet restore "WowPaperTrader.Api/WowPaperTrader.Api.csproj"

COPY . .

RUN dotnet tool install \
    --global dotnet-ef \
    --version 10.0.9 

ENV PATH="${PATH}:/root/.dotnet/tools"

# These are build-only placeholder values and are not copied into the final image.
ENV ConnectionStrings__WowPaperTrader="Host=localhost;Port=5432;Database=wowpapertrader;Username=postgres_admin;Password=build-only"
ENV Cors__AllowedOrigins__0="http://localhost"

RUN dotnet ef migrations bundle \
        --project "/src/WowPaperTrader.Persistence/WowPaperTrader.Persistence.csproj" \
        --startup-project "/src/WowPaperTrader.Api/WowPaperTrader.Api.csproj" \
        --context ApplicationDbContext \
        --configuration Release \
        --output "/app/migrations/efbundle" \
        --force \
        --no-color

WORKDIR "/src/WowPaperTrader.Api"

RUN dotnet publish \
    "WowPaperTrader.Api.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /app/migrations/efbundle ./efbundle
USER $APP_UID
ENTRYPOINT ["dotnet", "WowPaperTrader.Api.dll"]