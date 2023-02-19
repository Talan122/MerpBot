FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim-arm64v8 AS build-env 
WORKDIR /build

# Copy everything
COPY src/ .
COPY MerpBot.csproj .
# Restore as distinct layers
RUN dotnet restore -r linux-arm64
# Build and publish a release
RUN dotnet publish -c Release -o /out -r linux-arm64

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:7.0-bullseye-slim-arm64v8
WORKDIR /app
COPY --from=build-env /out .
RUN chmod +x ./MerpBot
ENTRYPOINT ["./MerpBot"]