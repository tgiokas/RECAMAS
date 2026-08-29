FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/RECAMAS.Api.csproj", "src/API/"]
COPY ["src/Application/RECAMAS.Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/RECAMAS.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Domain/RECAMAS.Domain.csproj", "src/Domain/"]

RUN dotnet restore "./src/API/RECAMAS.Api.csproj"
COPY . .
WORKDIR "/src/src/API"
RUN dotnet build "./RECAMAS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RECAMAS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RECAMAS.Api.dll"]