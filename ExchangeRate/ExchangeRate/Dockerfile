#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder

WORKDIR /src
COPY ["ExchangeRate/ExchangeRate.csproj", "ExchangeRate/"]
RUN dotnet restore "ExchangeRate/ExchangeRate.csproj" -v diag
COPY . .

COPY ./ExchangeRate .
RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:5.0-buster-slim AS base

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "ExchangeRate.dll"]