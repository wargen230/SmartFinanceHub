# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем только проект-файл (для кэширования слоя restore)
COPY *.csproj ./
RUN dotnet restore ./AI-Advisor.API.csproj

# Копируем всё из контекста (т.е. из папки AI-Advisor.API)
COPY . .

# Публикуем
RUN dotnet publish ./AI-Advisor.API.csproj -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AI-Advisor.API.dll"]
