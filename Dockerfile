FROM hub.lumigame.com/base/dotnet/core/sdk:5.0.102-ca-patch-buster-slim-2 AS builder

WORKDIR /src
COPY /src/ExchangeRate/ExchangeRate.Service.csproj ExchangeRate/ExchangeRate.Service.csproj

WORKDIR /src/ExchangeRate
RUN dotnet restore -v diag

COPY /src/ExchangeRate .
RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM hub.lumigame.com/base/dotnet/core/aspnet:5.0-alpine AS base

#RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
#RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app
RUN adduser app_user --disabled-password
USER app_user
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "ExchangeRate.Service.dll"]
