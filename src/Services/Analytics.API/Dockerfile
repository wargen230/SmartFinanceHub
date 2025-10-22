FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем только csproj (для кэширования restore)
COPY *.csproj ./
RUN dotnet restore ./Analytics.API.csproj

# Копируем исходники и публикуем
COPY . .
RUN dotnet publish ./Analytics.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Analytics.API.dll"]
