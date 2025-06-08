FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./

WORKDIR /app/CashFlow.Api

RUN dotnet restore

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY --from=build- /app/out .

ENTRYPOINT ["dotnet", "CashFlow.Api.dll"]

   
