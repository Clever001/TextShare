# Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем проектные файлы
COPY ["TextShareApi/TextShareApi.csproj", "TextShareApi/"]
RUN dotnet restore "TextShareApi/TextShareApi.csproj"

# Копируем остальные файлы
COPY . .
WORKDIR "/src/TextShareApi"

# Установка EF Core Tools
RUN dotnet tool restore
RUN dotnet tool install --global dotnet-ef --version 9.0.5
ENV PATH="${PATH}:/root/.dotnet/tools"

# Создаем миграции
RUN dotnet ef migrations add InitialCreate

# Публикация с Native AOT
RUN dotnet publish "TextShareApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish \
    --self-contained false \
    -p:PublishAot=false

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Установка зависимостей (если нужны)
RUN apt-get update && apt-get install -y --no-install-recommends \
    libssl-dev \
    libicu-dev \
    && rm -rf /var/lib/apt/lists/* 

# Устанавливаем wait-for-it.sh, чтобы подождать, когда будет запущена бд.
RUN apt-get update && apt-get install -y wget && wget https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh \
    && chmod +x ./wait-for-it.sh

# Копируем скомпилированные файлы
COPY --from=build /app/publish .

# Копируем сертификат
COPY --from=build /src/TextShareApi/certs/localhost.pfx /https/

# Порт приложения
EXPOSE 7285

# Запуск приложения
ENTRYPOINT ["dotnet", "./TextShareApi.dll"]
