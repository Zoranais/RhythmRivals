# Stage 1
FROM node:20.9.0 as build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build
# Stage 2
FROM nginx:latest
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/dist/rhythm-rivals/browser /usr/share/nginx/html
EXPOSE 80