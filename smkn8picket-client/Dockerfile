# Stage 1: Build pakai Bun
FROM oven/bun:1 AS build

WORKDIR /app

# Copy dependency files
COPY package.json bun.lock ./

# Install dependencies
RUN bun install

# Copy source
COPY . .

# Build Vite
RUN bun run build-only

# Stage 2: Serve dengan Nginx
FROM nginx:alpine

RUN apk add --no-cache gettext

# Copy hasil build
COPY --from=build /app/dist /usr/share/nginx/html

# Copy nginx config
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
COPY ./env.template.js /usr/share/nginx/html/env.template.js
COPY ./docker-entrypoint.sh /docker-entrypoint.sh

RUN chmod +x /docker-entrypoint.sh

EXPOSE 3001

ENTRYPOINT ["/docker-entrypoint.sh"]
