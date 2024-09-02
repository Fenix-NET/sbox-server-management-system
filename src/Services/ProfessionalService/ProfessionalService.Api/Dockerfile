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
COPY ["src/Services/ProfessionalService/ProfessionalService.Api/ProfessionalService.Api.csproj", "src/Services/ProfessionalService/ProfessionalService.Api/"]
COPY ["src/Infrastructure/Infrastructure.Logging/Infrastructure.Logging.csproj", "src/Infrastructure/Infrastructure.Logging/"]
COPY ["src/Infrastructure/Infrastructure.Messaging/Infrastructure.Messaging.csproj", "src/Infrastructure/Infrastructure.Messaging/"]
COPY ["src/Infrastructure/Infrastructure.Monitoring/Infrastructure.Monitoring.csproj", "src/Infrastructure/Infrastructure.Monitoring/"]
COPY ["src/Services/ProfessionalService/ProfessionalService.Application/ProfessionalService.Application.csproj", "src/Services/ProfessionalService/ProfessionalService.Application/"]
COPY ["src/Services/ProfessionalService/ProfessionalService.Core/ProfessionalService.Core.csproj", "src/Services/ProfessionalService/ProfessionalService.Core/"]
COPY ["src/Services/ProfessionalService/ProfessionalService.Infrastructure/ProfessionalService.Infrastructure.csproj", "src/Services/ProfessionalService/ProfessionalService.Infrastructure/"]
RUN dotnet restore "./src/Services/ProfessionalService/ProfessionalService.Api/ProfessionalService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/ProfessionalService/ProfessionalService.Api"
RUN dotnet build "./ProfessionalService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProfessionalService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProfessionalService.Api.dll"]