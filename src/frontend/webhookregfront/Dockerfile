# Build the app
FROM node:18.15.0-alpine3.17 as build
WORKDIR /src
COPY ./eltrello-client/package.json ./eltrello-client/package-lock.json ./
COPY ./eltrello-client .
RUN npm install
ARG configuration=production
RUN npm run build --prod

# Deploy the app
FROM nginx:1.23.4-alpine
COPY --from=build /src/dist/eltrello-client /usr/share/nginx/html
COPY ./eltrello-client/nginx-custom.conf /etc/nginx/conf.d/default.conf
COPY ./eltrello-client/env.sh ./

RUN chmod +x env.sh
ENTRYPOINT [ "sh", "/env.sh" ]
# CMD ["sh", "-c", "/env.sh && nginx -g 'daemon off;'"]
CMD [ "nginx", "-g", "daemon off;" ]

EXPOSE 80
