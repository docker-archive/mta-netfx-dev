# escape=`
FROM dockersamples/mta-dev-web-builder:3.5 AS builder
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

WORKDIR C:\src\SignUp.Web
COPY .\src\SignUp\SignUp.Web\packages.config .
RUN nuget restore packages.config -PackagesDirectory ..\packages

COPY src\SignUp C:\src
RUN msbuild SignUp.Web.csproj /p:OutputPath=c:\out /p:DeployOnBuild=true

# app image
FROM microsoft/aspnet:3.5-windowsservercore-10.0.14393.1884
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

ENV APP_ROOT="C:\web-app" `
    DB_CONNECTION_STRING_PATH=""

WORKDIR $APP_ROOT
RUN Import-Module WebAdministration; `
    Set-ItemProperty 'IIS:\AppPools\.NET v2.0' -Name processModel.identityType -Value LocalSystem; `
    Remove-Website -Name 'Default Web Site'; `
    New-Website -Name 'web-app' -Port 80 -PhysicalPath $env:APP_ROOT -ApplicationPool '.NET v2.0'

COPY .\docker\web\start.ps1 .
ENTRYPOINT ["powershell", ".\\start.ps1"]

COPY --from=builder C:\out\_PublishedWebsites\SignUp.Web .