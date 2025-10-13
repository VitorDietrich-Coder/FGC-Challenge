# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar projetos e restaurar dependências
COPY ["src/FGC.Api/FGC.Api.csproj", "src/FGC.Api/"]
COPY ["src/FGC.Application/FGC.Application.csproj", "src/FGC.Application/"]
COPY ["src/FGC.Domain/FGC.Domain.csproj", "src/FGC.Domain/"]
COPY ["src/FGC.Infra/FGC.Infra.csproj", "src/FGC.Infra/"]

RUN dotnet restore "./src/FGC.Api/FGC.Api.csproj"

# Copiar todo o código e buildar
COPY . .
WORKDIR "/src/src/FGC.Api"
RUN dotnet build "./FGC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar para produção
RUN dotnet publish "./FGC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Migrator - responsável por aplicar migrations no banco
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrator
WORKDIR /src
COPY --from=build /src /src

WORKDIR /src/src/FGC.Api

# Instala dotnet-ef globalmente
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

# Comando para rodar migrations (você pode chamar esse container separadamente)
ENTRYPOINT ["dotnet", "ef", "database", "update", "--no-build", "--project", "FGC.Api.csproj"]

# Final runtime image - para rodar a aplicação
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FGC.Api.dll"]

FROM prom/prometheus
COPY prometheus.yml /etc/prometheus/prometheus.yml
