# BUILD
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_VER
WORKDIR /src
COPY ./ModCore ./
RUN dotnet restore
RUN dotnet publish -c Release -o out /p:VersionPrefix=${BUILD_VER}

# RUNNER IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build /src/out .
WORKDIR /config

ENTRYPOINT ["dotnet", "/app/ModCore.dll"]