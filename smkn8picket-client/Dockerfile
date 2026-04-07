# Stage 1: Build pakai Bun
FROM oven/bun:1 AS build

ENV VITE_API_URL=http://localhost:5001
ENV VITE_NAMA_SEKOLAH="SMKN 8 TIK KOTA JAYAPURA"
ENV VITE_ALAMAT_SEKOLAH="Jln. "
ENV VITE_NO_TELP="0967-"
ENV VITE_EMAIL="smkn8tik@gmail.com"
ENV VITE_KOTA="KOTA JAYAPURA"
ENV VITE_PROVINSI="PAPUA:"
ENV VITE_KODE_POS=99111:
ENV VITE_LOGO=/smk.png

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

# Copy hasil build
COPY --from=build /app/dist /usr/share/nginx/html

# Copy nginx config
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf

EXPOSE 80



CMD ["nginx", "-g", "daemon off;"]