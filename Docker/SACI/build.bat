TITLE SACI - DEV
IF NOT EXIST .\app\publish\ (
	MKDIR .\app\publish\
	COPY ..\..\CorreosInstitucionales\Server\bin\Release\net8.0\publish .\app\publish\
	.\app\publish\appsettings.production.json  .\app\publish\appsettings.json /V /Y
)
docker-compose up --build
PAUSE
