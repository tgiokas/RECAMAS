FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all projects
COPY ["RecamasInterfaces.sln", "./"]
COPY ["src/Shared/Domain/Shared.Domain.csproj", "src/Shared/Domain/"]
COPY ["src/Shared/Web/Shared.Web.csproj", "src/Shared/Web/"]
COPY ["src/CassApi/Domain/CassApi.Domain.csproj", "src/CassApi/Domain/"]
COPY ["src/CassApi/Application/CassApi.Application.csproj", "src/CassApi/Application/"]
COPY ["src/CassApi/Infrastructure/CassApi.Infrastructure.csproj", "src/CassApi/Infrastructure/"]
COPY ["src/CassApi/WebApi/CassApi.WebApi.csproj", "src/CassApi/WebApi/"]
RUN dotnet restore "src/CassApi/WebApi/CassApi.WebApi.csproj"

COPY . .
WORKDIR "/src/src/CassApi/WebApi"
RUN dotnet build "CassApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CassApi.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CassApi.WebApi.dll"]
