@echo off

set api-key=*************************** API KEY ***************************************

REM Set the version variable
set pushVersion=1.0.3
@echo ----------------------------------------
@echo Benraz.Infrastructure.Authorization version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.Authorization.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json


set pushVersion=1.0.4
@echo ----------------------------------------
@echo Benraz.Infrastructure.Common version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.Common.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json


set pushVersion=1.0.3
@echo ----------------------------------------
@echo Benraz.Infrastructure.Domain version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.Domain.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json


set pushVersion=1.0.3
@echo ----------------------------------------
@echo Benraz.Infrastructure.EF version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.EF.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json



set pushVersion=1.0.4
@echo ----------------------------------------
@echo Benraz.Infrastructure.Emails version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.Emails.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json


set pushVersion=1.0.4
@echo ----------------------------------------
@echo Benraz.Infrastructure.Files version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.Files.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json


set pushVersion=1.0.6
@echo ----------------------------------------
@echo Benraz.Infrastructure.Gateways version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.Gateways.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json


set pushVersion=1.0.3
@echo ----------------------------------------
@echo Benraz.Infrastructure.Phone version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.Phone.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json


set pushVersion=1.0.3
@echo ----------------------------------------
@echo Benraz.Infrastructure.Web version: %pushVersion%
@echo ----------------------------------------
dotnet nuget push .\src\BenrazNugetPackages\Benraz.Infrastructure.Web.%pushVersion%.nupkg --api-key %api-key% --source https://api.nuget.org/v3/index.json

pause