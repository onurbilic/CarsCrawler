FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/CarsCrawler/CarsCrawler.Consumers/CarsCrawler.Consumers.csproj", "CarsCrawler.Consumers/"]
RUN dotnet restore "Services/CarsCrawler/CarsCrawler.Consumers/CarsCrawler.Consumers.csproj"
COPY . .
WORKDIR "/src/CarsCrawler.Consumers"
RUN dotnet build "CarsCrawler.Consumers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarsCrawler.Consumers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarsCrawler.Consumers.dll"]
