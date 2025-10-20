FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServerMonitoringSystem.Collector/ServerMonitoringSystem.Collector.csproj", "ServerMonitoringSystem.Collector/"]
COPY ["ServerMonitoringSystem.RabbitMQ/ServerMonitoringSystem.RabbitMQ.csproj", "ServerMonitoringSystem.RabbitMQ/"]
RUN dotnet restore "ServerMonitoringSystem.Collector/ServerMonitoringSystem.Collector.csproj"
COPY . .
WORKDIR "/src/ServerMonitoringSystem.Collector"
RUN dotnet build "ServerMonitoringSystem.Collector.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ServerMonitoringSystem.Collector.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerMonitoringSystem.Collector.dll"]
