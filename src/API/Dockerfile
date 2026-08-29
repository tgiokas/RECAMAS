FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/API/RECAMAS.Api.csproj", "src/API/"]
COPY ["src/Application/RECAMAS.Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/RECAMAS.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Domain/RECAMAS.Domain.csproj", "src/Domain/"]
RUN dotnet restore "src/API/RECAMAS.Api.csproj"

COPY . .
RUN dotnet publish "src/API/RECAMAS.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY errors.json /app/errors.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "RECAMAS.Api.dll"]
