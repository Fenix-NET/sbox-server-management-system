# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/InventoryService/InventoryService.Api/InventoryService.Api.csproj", "src/Services/InventoryService/InventoryService.Api/"]
COPY ["src/Infrastructure/Infrastructure.Logging/Infrastructure.Logging.csproj", "src/Infrastructure/Infrastructure.Logging/"]
COPY ["src/Infrastructure/Infrastructure.Messaging/Infrastructure.Messaging.csproj", "src/Infrastructure/Infrastructure.Messaging/"]
COPY ["src/Infrastructure/Infrastructure.Monitoring/Infrastructure.Monitoring.csproj", "src/Infrastructure/Infrastructure.Monitoring/"]
COPY ["src/Services/InventoryService/InventoryService.Application/InventoryService.Application.csproj", "src/Services/InventoryService/InventoryService.Application/"]
COPY ["src/Services/InventoryService/InventoryService.Core/InventoryService.Core.csproj", "src/Services/InventoryService/InventoryService.Core/"]
COPY ["src/Services/InventoryService/InventoryService.Infrastructure/InventoryService.Infrastructure.csproj", "src/Services/InventoryService/InventoryService.Infrastructure/"]
RUN dotnet restore "./src/Services/InventoryService/InventoryService.Api/InventoryService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/InventoryService/InventoryService.Api"
RUN dotnet build "./InventoryService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InventoryService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryService.Api.dll"]