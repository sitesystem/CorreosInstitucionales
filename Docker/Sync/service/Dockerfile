FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /var/dotnet

COPY ./publish /var/dotnet
COPY ./openssl.cnf /etc/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "/var/dotnet/SyncDB.dll"]
