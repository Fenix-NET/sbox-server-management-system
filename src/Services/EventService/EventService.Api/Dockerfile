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
COPY ["src/Services/EventService/EventService.Api/EventService.Api.csproj", "src/Services/EventService/EventService.Api/"]
COPY ["src/Infrastructure/Infrastructure.Logging/Infrastructure.Logging.csproj", "src/Infrastructure/Infrastructure.Logging/"]
COPY ["src/Infrastructure/Infrastructure.Messaging/Infrastructure.Messaging.csproj", "src/Infrastructure/Infrastructure.Messaging/"]
COPY ["src/Infrastructure/Infrastructure.Monitoring/Infrastructure.Monitoring.csproj", "src/Infrastructure/Infrastructure.Monitoring/"]
COPY ["src/Services/EventService/EventService.Application/EventService.Application.csproj", "src/Services/EventService/EventService.Application/"]
COPY ["src/Services/EventService/EventService.Core/EventService.Core.csproj", "src/Services/EventService/EventService.Core/"]
COPY ["src/Services/EventService/EventService.Infrastructure/EventService.Infrastructure.csproj", "src/Services/EventService/EventService.Infrastructure/"]
RUN dotnet restore "./src/Services/EventService/EventService.Api/EventService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/EventService/EventService.Api"
RUN dotnet build "./EventService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventService.Api.dll"]