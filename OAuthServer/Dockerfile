FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["OAuthServer/OAuthServer.csproj", "OAuthServer/"]
COPY ["CSharpJWT/CSharpJWT.csproj", "CSharpJWT/"]
RUN dotnet restore "OAuthServer/OAuthServer.csproj"
COPY . .
WORKDIR "/src/OAuthServer"
RUN dotnet build "OAuthServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OAuthServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OAuthServer.dll"]
