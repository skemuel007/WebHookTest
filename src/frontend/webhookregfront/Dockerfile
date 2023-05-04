# Build the app
FROM node:18.15.0-alpine3.17 as build
WORKDIR /src
COPY ./src/frontend/webhookregfront/package.json ./
COPY ./src/frontend/webhookregfront/package-lock.json ./
COPY ./src/frontend/webhookregfront .
RUN npm install
ARG configuration=production
RUN npm run build --prod

# Deploy the app
FROM nginx:1.23.4-alpine
COPY --from=build /src/dist/web-hook-reg-front /usr/share/nginx/html
COPY ./src/frontend/webhookregfront/nginx-custom.conf /etc/nginx/conf.d/default.conf
COPY ./src/frontend/webhookregfront/env.sh ./

RUN chmod +x env.sh
ENTRYPOINT [ "sh", "/env.sh" ]
# CMD ["sh", "-c", "/env.sh && nginx -g 'daemon off;'"]
CMD [ "nginx", "-g", "daemon off;" ]

EXPOSE 80
