FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# Nginx
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html

# Nginx escucha por defecto en el puerto 80
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]