FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder

WORKDIR /src
COPY /ExchangeRate/src/ExchangeRate/ExchangeRate.csproj ExchangeRate/ExchangeRate.csproj

WORKDIR /src/ExchangeRate
RUN dotnet restore -v diag

COPY /ExchangeRate/src/ExchangeRate .
RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:5.0-buster-slim AS base

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "ExchangeRate.dll"]