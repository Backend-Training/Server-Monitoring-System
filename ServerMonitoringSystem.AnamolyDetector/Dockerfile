FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServerMonitoringSystem.AnamolyDetector/ServerMonitoringSystem.AnamolyDetector.csproj", "ServerMonitoringSystem.AnamolyDetector/"]
RUN dotnet restore "ServerMonitoringSystem.AnamolyDetector/ServerMonitoringSystem.AnamolyDetector.csproj"
COPY . .
WORKDIR "/src/ServerMonitoringSystem.AnamolyDetector"
RUN dotnet build "./ServerMonitoringSystem.AnamolyDetector.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ServerMonitoringSystem.AnamolyDetector.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerMonitoringSystem.AnamolyDetector.dll"]
