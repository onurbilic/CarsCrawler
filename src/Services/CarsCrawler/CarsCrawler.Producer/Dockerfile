FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/CarsCrawler/CarsCrawler.Producer/CarsCrawler.Producer.csproj", "CarsCrawler.Producer/"]
RUN dotnet restore "Services/CarsCrawler/CarsCrawler.Producer/CarsCrawler.Producer.csproj"
COPY . .
WORKDIR "/src/CarsCrawler.Producer"
RUN dotnet build "CarsCrawler.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarsCrawler.Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarsCrawler.Producer.dll"]
