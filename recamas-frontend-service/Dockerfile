# syntax=docker/dockerfile:1

# Base stage - shared dependencies
FROM node:22-alpine AS base

WORKDIR /app

COPY package*.json ./

RUN npm config set registry https://registry.npmjs.org/ \
 && npm config set audit false \
 && npm config set fund false \
 && npm config set fetch-retries 5 \
 && npm config set fetch-retry-maxtimeout 60000 \
 && npm ci

COPY . .

# Development stage - Vite dev server, port comes from vite.config.ts
FROM base AS development

EXPOSE 5173

CMD ["sh", "-c", "CHOKIDAR_USEPOLLING=true npm run dev -- --host 0.0.0.0"]

# Build stage
FROM base AS build

RUN npm run build

# Production stage - static files behind nginx
FROM nginx:alpine AS production

COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 5173

CMD ["nginx", "-g", "daemon off;"]
