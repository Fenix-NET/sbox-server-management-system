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
COPY ["src/Services/CharacterService/CharacterService.Api/CharacterService.Api.csproj", "src/Services/CharacterService/CharacterService.Api/"]
COPY ["src/Infrastructure/Infrastructure.Logging/Infrastructure.Logging.csproj", "src/Infrastructure/Infrastructure.Logging/"]
COPY ["src/Infrastructure/Infrastructure.Messaging/Infrastructure.Messaging.csproj", "src/Infrastructure/Infrastructure.Messaging/"]
COPY ["src/Infrastructure/Infrastructure.Monitoring/Infrastructure.Monitoring.csproj", "src/Infrastructure/Infrastructure.Monitoring/"]
COPY ["src/Services/CharacterService/CharacterService.Application/CharacterService.Application.csproj", "src/Services/CharacterService/CharacterService.Application/"]
COPY ["src/Services/CharacterService/CharacterService.Domain/CharacterService.Core.csproj", "src/Services/CharacterService/CharacterService.Domain/"]
COPY ["src/Services/CharacterService/CharacterService.Infrastructure/CharacterService.Infrastructure.csproj", "src/Services/CharacterService/CharacterService.Infrastructure/"]
RUN dotnet restore "./src/Services/CharacterService/CharacterService.Api/CharacterService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/CharacterService/CharacterService.Api"
RUN dotnet build "./CharacterService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CharacterService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CharacterService.Api.dll"]