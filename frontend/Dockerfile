# Stage 1
FROM node:20.9.0 as build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
RUN npm install -g @angular/cli
RUN npm install
COPY . .
RUN ng build
# Stage 2
FROM nginx:latest
COPY --from=build /usr/src/app/dist/rhythm-rivals/browser /usr/share/nginx/html
EXPOSE 80