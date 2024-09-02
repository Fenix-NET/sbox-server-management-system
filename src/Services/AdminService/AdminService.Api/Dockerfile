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
COPY ["src/Services/AdminService/AdminService.Api/AdminService.Api.csproj", "src/Services/AdminService/AdminService.Api/"]
COPY ["src/Infrastructure/Infrastructure.Logging/Infrastructure.Logging.csproj", "src/Infrastructure/Infrastructure.Logging/"]
COPY ["src/Infrastructure/Infrastructure.Messaging/Infrastructure.Messaging.csproj", "src/Infrastructure/Infrastructure.Messaging/"]
COPY ["src/Infrastructure/Infrastructure.Monitoring/Infrastructure.Monitoring.csproj", "src/Infrastructure/Infrastructure.Monitoring/"]
COPY ["src/Services/AdminService/AdminService.Application/AdminService.Application.csproj", "src/Services/AdminService/AdminService.Application/"]
COPY ["src/Services/AdminService/AdminService.Domain/AdminService.Core.csproj", "src/Services/AdminService/AdminService.Domain/"]
COPY ["src/Services/AdminService/AdminService.Infrastructure/AdminService.Infrastructure.csproj", "src/Services/AdminService/AdminService.Infrastructure/"]
RUN dotnet restore "./src/Services/AdminService/AdminService.Api/AdminService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/AdminService/AdminService.Api"
RUN dotnet build "./AdminService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AdminService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminService.Api.dll"]