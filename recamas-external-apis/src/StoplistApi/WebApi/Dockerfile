FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all projects
COPY ["RecamasInterfaces.sln", "./"]
COPY ["src/Shared/Domain/Shared.Domain.csproj", "src/Shared/Domain/"]
COPY ["src/Shared/Web/Shared.Web.csproj", "src/Shared/Web/"]
COPY ["src/StoplistApi/Domain/StoplistApi.Domain.csproj", "src/StoplistApi/Domain/"]
COPY ["src/StoplistApi/Application/StoplistApi.Application.csproj", "src/StoplistApi/Application/"]
COPY ["src/StoplistApi/Infrastructure/StoplistApi.Infrastructure.csproj", "src/StoplistApi/Infrastructure/"]
COPY ["src/StoplistApi/WebApi/StoplistApi.WebApi.csproj", "src/StoplistApi/WebApi/"]
RUN dotnet restore "src/StoplistApi/WebApi/StoplistApi.WebApi.csproj"

COPY . .
WORKDIR "/src/src/StoplistApi/WebApi"
RUN dotnet build "StoplistApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StoplistApi.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoplistApi.WebApi.dll"]
