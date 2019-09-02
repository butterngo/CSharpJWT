FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["samples/Client1/Client1.csproj", "samples/Client1/"]
COPY ["src/CSharpJWT.Client/CSharpJWT.Client.csproj", "src/CSharpJWT.Client/"]
RUN dotnet restore "samples/Client1/Client1.csproj"
COPY . .
WORKDIR "/src/samples/Client1"
RUN dotnet build "Client1.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Client1.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Client1.dll"]