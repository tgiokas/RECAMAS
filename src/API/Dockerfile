# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "./"]
COPY ["src/API/RECAMAS.Api.csproj", "src/API/"]
COPY ["src/Application/RECAMAS.Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/RECAMAS.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Domain/RECAMAS.Domain.csproj", "src/Domain/"]

# Cbs.* packages restore from the private cbs-github feed (see nuget.config),
# which refuses anonymous reads. The nuget_token secret is a GitHub PAT
# locally (read:packages, authorized for cosmos-business-systems if SSO is
# enforced) or the CI runner's own GITHUB_TOKEN — the BuildKit secret mount
# keeps it out of the final image (this RUN's own layer never ships; only
# /app/publish, copied in the `final` stage below, does). --store-password-
# in-clear-text is required here regardless — NuGet's encrypted credential
# storage is Windows/DPAPI-only, so a plaintext nuget.config inside this
# intermediate layer is unavoidable on Linux, not a mistake in this approach.
RUN --mount=type=secret,id=nuget_token \
    dotnet nuget update source cbs-github \
        --username tgiokas \
        --password "$(cat /run/secrets/nuget_token)" \
        --store-password-in-clear-text \
        --configfile ./nuget.config && \
    dotnet restore "./src/API/RECAMAS.Api.csproj"
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