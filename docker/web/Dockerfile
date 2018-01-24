# escape=`
FROM dockersamples/mta-dev-web-builder:4.7.1 AS builder
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

WORKDIR C:\src\SignUp.Web
COPY .\src\SignUp\SignUp.Web\packages.config .
RUN nuget restore packages.config -PackagesDirectory ..\packages

COPY src\SignUp C:\src
RUN msbuild SignUp.Web.csproj /p:OutputPath=c:\out /p:DeployOnBuild=true

# app image
FROM microsoft/aspnet:4.7.1-windowsservercore-10.0.14393.1884
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

ENV APP_ROOT="C:\\web-app" `
    MESSAGE_QUEUE_URL="nats://message-queue:4222" `
    DB_CONNECTION_STRING_PATH=""`
    APP_SETTINGS_PATH=""

WORKDIR $APP_ROOT
RUN Import-Module WebAdministration; `
    Set-ItemProperty 'DefaultAppPool' -Name processModel.identityType -Value LocalSystem; `
    Remove-Website -Name 'Default Web Site'; `
    New-Website -Name 'web-app' -Port 80 -PhysicalPath $env:$APP_ROOT

COPY .\docker\web\v3\start.ps1 .
ENTRYPOINT ["powershell", ".\\start.ps1"]

COPY --from=builder C:\out\_PublishedWebsites\SignUp.Web .