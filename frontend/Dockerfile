FROM node:current-alpine AS build-env
WORKDIR src/

# install dependencies
COPY package*.json ./
RUN npm install

# build angular application
COPY . .
RUN npm run build

FROM nginx:alpine

# copy nginx config
COPY /src/etc/nginx/conf.d/ng.conf /etc/nginx/conf.d/
RUN rm /etc/nginx/conf.d/default.conf

# copy angular app
COPY --from=build-env src/dist/* /usr/share/nginx/html
