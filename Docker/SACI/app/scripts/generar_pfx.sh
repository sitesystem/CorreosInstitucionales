#!/bin/bash
mkdir /https/
cd /scripts/

openssl req -x509 -nodes -days 3650 -newkey rsa:2048 -keyout 148.204.211.186.key -out 148.204.211.186.crt -config 148.204.211.186.conf -extensions v3_req
openssl pkcs12 -export -out /https/CorreosInstitucionales.pfx -inkey 148.204.211.186.key -in 148.204.211.186.crt -passout pass:#Password!!1