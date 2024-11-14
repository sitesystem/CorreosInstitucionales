FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /var/dotnet
COPY ./publish /var/dotnet

ENTRYPOINT ["dotnet", "/var/dotnet/SyncDB.dll"]