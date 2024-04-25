FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /var/dotnet
COPY ./publish /var/dotnet

ENV ASPNETCORE_HTTPS_PORTS=443
ENV DEBIAN_FRONTEND=noninteractive
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=#Password!!1
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/CorreosInstitucionales.pfx

RUN mv -f /var/dotnet/appsettings.production.json /var/dotnet/appsettings.json

ENTRYPOINT ["dotnet", "/var/dotnet/CorreosInstitucionales.Server.dll"]