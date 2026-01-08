# Stage 1: Сборка приложения
FROM node:25-alpine AS builder

# Установка рабочей директории
WORKDIR /app

# Добавление аргументов для переменных окружения
ARG REACT_APP_SERVER_URL_PATH
ARG REACT_APP_CLIENT_URL_PATH

# Преобразование ARG в ENV для использования во время сборки
ENV REACT_APP_SERVER_URL_PATH=${REACT_APP_SERVER_URL_PATH}
ENV REACT_APP_CLIENT_URL_PATH=${REACT_APP_CLIENT_URL_PATH}

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