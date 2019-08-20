FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

COPY CSharpJWT/*.csproj ./CSharpJWT/

COPY Client1/*.csproj ./Client1/
RUN dotnet restore "./Client1/Client1.csproj" -s https://api.nuget.org/v3/index.json


COPY . .
WORKDIR /src/Client1
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Client1.dll"]