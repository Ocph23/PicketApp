FROM node:lts-alpine AS build-stage
# set the working directory

ARG VITE_API_URL
ARG VITE_LOGO
ENV VITE_API_URL=http://localhost:5001
ENV VITE_LOGO=/smk.png

ENV VITE_NAMA_SEKOLAH="SMKN 8 TIK KOTA JAYAPURA"
ENV VITE_ALAMAT_SEKOLAH="Jln." 
ENV VITE_NO_TELP="0967-123456"
ENV VITE_EMAIL="admin@smkn8tikjayapura.sch.id"
ENV VITE_KOTA="KOTA JAYAPURA"
ENV VITE_PROVINSI="PAPUA"
ENV VITE_KODE_POS="99111"
ENV VITE_LOGO=/smk.png

# RUN npm i npm-run-all -g
# RUN npm i vue-tsc -g

WORKDIR /app
# Copy the working directory in the container
COPY ["package.json",  "npm-shrinkwrap.json*", "./"]
# Install the project dependencies
RUN npm install 
# Copy the rest of the project files to the container
COPY . .
# Build the Vue.js application to the production mode to dist folder
RUN npm run build
# Use the lightweight Nginx image from the previous stage for the nginx container
FROM nginx:stable-alpine AS production-stage
# Copy the build application from the previous stage to the Nginx container
COPY --from=build-stage /app/dist /usr/share/nginx/html
# Copy the nginx configuration file
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
# Expose the port 80
EXPOSE 80
# Start Nginx to serve the application
CMD ["nginx", "-g", "daemon off;"]