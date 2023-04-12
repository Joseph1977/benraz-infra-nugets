@echo ----------------------------------------
@echo Benraz.Infrastructure.Authorization
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az .\src\Benraz.Infrastructure.Authorization\bin\Release\*.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Common
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az .\src\Benraz.Infrastructure.Common\bin\Release\*.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Domain
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az .\src\Benraz.Infrastructure.Domain\bin\Release\*.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Emails
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az .\src\Benraz.Infrastructure.Emails\bin\Release\*.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Files
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az .\src\Benraz.Infrastructure.Files\bin\Release\*.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.EF
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az .\src\Benraz.Infrastructure.EF\bin\Release\*.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Gateways
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az .\src\Benraz.Infrastructure.Gateways\bin\Release\*.nupkg

@echo ----------------------------------------
@echo Benraz.Infrastructure.Web
@echo ----------------------------------------
nuget.exe push -Source "BenrazNuGet" -ApiKey az .\src\Benraz.Infrastructure.Web\bin\Release\*.nupkg

pause