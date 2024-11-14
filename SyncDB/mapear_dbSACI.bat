SETLOCAL
::https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli

::dotnet add package Microsoft.EntityFrameworkCore.SqlServer

::dotnet tool install --global dotnet-ef
::dotnet add package Microsoft.EntityFrameworkCore.Design

::-Provider Microsoft.EntityFrameworkCore.SqlServer ^
::"SQLServer_Connection": "Server=www.developers.upiicsa.ipn.mx;Database=db_CorreosInstitucionales_UPIICSA;User ID=correos_institucionales;Password=correos_institucionales;Trusted_Connection=False;TrustServerCertificate=True;",
::"Docker_Connection": "Server=148.204.211.186;Database=db_Centralizada_UPIICSA;User ID=sa;Password=#Password!!1;Trusted_Connection=False;TrustServerCertificate=True;"

dotnet ef dbcontext scaffold ^
	"Server=www.developers.upiicsa.ipn.mx;Database=db_CorreosInstitucionales_UPIICSA;User ID=correos_institucionales;Password=correos_institucionales;Trusted_Connection=False;TrustServerCertificate=True;" ^
	Microsoft.EntityFrameworkCore.SqlServer ^
	--namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContext ^
	--output-dir dbSACI ^
	--data-annotations ^
	--force
	
:: ..\CorreosInstitucionales\Shared\CapaDataAccess\DBContext
PAUSE