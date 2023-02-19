FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env 
WORKDIR /build

# Copy everything
COPY src/ .
COPY MerpBot.csproj .
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet build

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:7.0-bullseye-slim
WORKDIR /app
COPY --from=build-env /build/bin/Debug/net7.0 .
ENTRYPOINT ["dotnet", "MerpBot.dll"]