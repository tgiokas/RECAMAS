FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all projects
COPY ["RecamasInterfaces.sln", "./"]
COPY ["src/Shared/Domain/Shared.Domain.csproj", "src/Shared/Domain/"]
COPY ["src/Shared/Web/Shared.Web.csproj", "src/Shared/Web/"]
COPY ["src/ArrivalsApi/Domain/ArrivalsApi.Domain.csproj", "src/ArrivalsApi/Domain/"]
COPY ["src/ArrivalsApi/Application/ArrivalsApi.Application.csproj", "src/ArrivalsApi/Application/"]
COPY ["src/ArrivalsApi/Infrastructure/ArrivalsApi.Infrastructure.csproj", "src/ArrivalsApi/Infrastructure/"]
COPY ["src/ArrivalsApi/WebApi/ArrivalsApi.WebApi.csproj", "src/ArrivalsApi/WebApi/"]
RUN dotnet restore "src/ArrivalsApi/WebApi/ArrivalsApi.WebApi.csproj"

COPY . .
WORKDIR "/src/src/ArrivalsApi/WebApi"
RUN dotnet build "ArrivalsApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArrivalsApi.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArrivalsApi.WebApi.dll"]
