# Stage 1: Сборка приложения
FROM node:20-alpine AS builder

# Установка рабочей директории
WORKDIR /app

# Копирование package.json и package-lock.json
COPY text-share-client/package*.json ./

# Установка зависимостей
RUN npm install

# Копирование исходного кода
COPY text-share-client/ .

# Сборка приложения
RUN npm run build

# Stage 2: Подготовка финального образа с nginx
FROM nginx:alpine

# Копирование статических файлов из builder в nginx
COPY --from=builder /app/build /usr/share/nginx/html

# Копирование конфигурации nginx
COPY --from=builder /app/nginx.conf /etc/nginx/nginx.conf

# Открытие порта
EXPOSE 80

# Запуск nginx
CMD ["nginx", "-g", "daemon off;"]