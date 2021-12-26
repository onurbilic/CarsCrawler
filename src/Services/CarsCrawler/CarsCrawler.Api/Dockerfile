FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5010

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/CarsCrawler/CarsCrawler.API/CarsCrawler.API.csproj", "CarsCrawler.API/"]
RUN dotnet restore "Services/CarsCrawler/CarsCrawler.API/CarsCrawler.API.csproj"
COPY . .
WORKDIR "/src/CarsCrawler.API"
RUN dotnet build "CarsCrawler.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarsCrawler.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarsCrawler.API.dll"]
