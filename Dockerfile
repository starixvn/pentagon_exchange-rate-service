FROM hub.lumigame.com/base/dotnet/core/sdk:9.0-alpine-25h2 AS builder

WORKDIR /src
COPY /src/ExchangeRate/ExchangeRate.Service.csproj ExchangeRate/ExchangeRate.Service.csproj

WORKDIR /src/ExchangeRate
RUN dotnet restore -v diag

COPY /src/ExchangeRate .
RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM hub.lumigame.com/base/dotnet/core/aspnet:9.0-alpine-25h2 AS base

WORKDIR /app
RUN adduser app_user --disabled-password
USER app_user
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "ExchangeRate.Service.dll"]
