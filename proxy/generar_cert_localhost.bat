dotnet dev-certs https -ep .\app\https\localhost.crt -np --trust --format pem
::dotnet dev-certs https -ep .\app\https\localhost.pfx -p "#Password!!1"
::dotnet dev-certs https --trust
PAUSE
