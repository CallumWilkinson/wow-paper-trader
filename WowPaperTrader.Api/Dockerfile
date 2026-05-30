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

WORKDIR "/src/WowPaperTrader.Api"
RUN dotnet publish "WowPaperTrader.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WowPaperTrader.Api.dll"]