FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all projects
COPY ["RecamasInterfaces.sln", "./"]
COPY ["src/Shared/Domain/Shared.Domain.csproj", "src/Shared/Domain/"]
COPY ["src/Shared/Web/Shared.Web.csproj", "src/Shared/Web/"]
COPY ["src/ArsApi/Domain/ArsApi.Domain.csproj", "src/ArsApi/Domain/"]
COPY ["src/ArsApi/Application/ArsApi.Application.csproj", "src/ArsApi/Application/"]
COPY ["src/ArsApi/Infrastructure/ArsApi.Infrastructure.csproj", "src/ArsApi/Infrastructure/"]
COPY ["src/ArsApi/WebApi/ArsApi.WebApi.csproj", "src/ArsApi/WebApi/"]
RUN dotnet restore "src/ArsApi/WebApi/ArsApi.WebApi.csproj"

COPY . .
WORKDIR "/src/src/ArsApi/WebApi"
RUN dotnet build "ArsApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArsApi.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArsApi.WebApi.dll"]
