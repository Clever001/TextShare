# Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем проектные файлы
COPY ["TextShareApi/TextShareApi.csproj", "TextShareApi/"]
RUN dotnet restore "TextShareApi/TextShareApi.csproj"

# Копируем остальные файлы
COPY . .
WORKDIR "/src/TextShareApi"

# Установка EF Core Tools
RUN dotnet new tool-manifest && \
    dotnet tool restore && \
    dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Публикация с Native AOT
RUN dotnet publish "TextShareApi.csproj" -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --self-contained false \
    -p:PublishAot=false

# Создаем sql скрипт
RUN dotnet ef migrations add InitialCreate && \
    dotnet ef migrations script --output \
    /app/publish/migrations/migrations.sql

# Удаление BOM-метки
RUN sed -i '1s/^\xEF\xBB\xBF//' /app/publish/migrations/migrations.sql

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Установка зависимостей
RUN apt-get update && apt-get install -y --no-install-recommends \
    libicu-dev wget postgresql-client && \
    wget https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh \
    && chmod +x ./wait-for-it.sh

# Копируем скомпилированные файлы
COPY --from=build /app/publish .

# Порт приложения
EXPOSE 7285

# Запуск приложения
ENTRYPOINT ["dotnet", "./TextShareApi.dll"]
