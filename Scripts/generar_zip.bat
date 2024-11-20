@ECHO OFF
SETLOCAL

SET "publish_path=.\CorreosInstitucionales\Server\bin\Release\net8.0\publish"

ECHO ELIMINANDO LOGS
ECHO %publish_path%\wwwroot\logs\
DEL %publish_path%\wwwroot\logs\*.txt
::PAUSE

ECHO COPIANDO ARCHIVO DE CONFIGURACIÓN...
COPY %publish_path%\appsettings.production.json %publish_path%\appsettings.json /V /Y
ECHO.

IF EXIST publish.zip (
	ECHO ELIMINADO PUBLISH.ZIP
	DEL publish.zip
	ECHO.
)

ECHO GENERANDO PUBLISH.ZIP
.\software\7zip\7za.exe a publish.zip -x^!publish\wwwroot\repositorio -mx0 %publish_path%
::-v50M 

::tar -cvf publish.zip .\CorreosInstitucionales\Server\bin\Release\net8.0\publish\*
::.\CorreosInstitucionales\Server\bin\Release\net8.0\publish
ENDLOCAL
PAUSE