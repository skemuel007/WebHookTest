#!/bin/sh
# Replace placeholders in the Nginx config file with actual values
# envsubst < /etc/nginx/conf.d/default.conf > /etc/nginx/conf.d/default.conf
envsubst < /usr/share/nginx/html/assets/config/config.template.json > /usr/share/nginx/html/config.json && exec nginx -g 'daemon off;'
# Copy environment variables to the Angular app
#for var in `env`
#do
#    if [[ $var == MY_APP_* ]]
#    then
#        key=$(echo "$var" | awk -F = '{print $1}')
#        value=$(echo "$var" | awk -F = '{print $2}')
#        ng set --global $key=$value
#    fi
#done
