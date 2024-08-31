@echo off

REM Set the version variable
set pushVersion=1.0.3


@echo ----------------------------------------
@echo Benraz.Infrastructure.Authorization version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\Benraz.Infrastructure.Authorization.%pushVersion%.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Common version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\Benraz.Infrastructure.Common.%pushVersion%.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Domain version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\Benraz.Infrastructure.Domain.%pushVersion%.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Emails version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\Benraz.Infrastructure.Emails.%pushVersion%.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Phone version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\Benraz.Infrastructure.Phone.%pushVersion%.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Files version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\Benraz.Infrastructure.Files.%pushVersion%.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.EF version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\Benraz.Infrastructure.EF.%pushVersion%.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Gateways version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\bin\Release\Benraz.Infrastructure.Gateways.%pushVersion%.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Web version: %pushVersion%
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az  .\src\BenrazNugetPackages\Benraz.Infrastructure.Web.%pushVersion%.nupkg

pause