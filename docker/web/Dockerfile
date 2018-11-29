# escape=`
FROM microsoft/dotnet-framework:4.7.2-sdk AS builder
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

WORKDIR C:\src
COPY src\SignUp .
RUN .\build.ps1

# app image
FROM microsoft/aspnet:4.7.2-windowsservercore-ltsc2016
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

WORKDIR C:\web-app
RUN Remove-Website -Name 'Default Web Site'; `
    New-Website -Name 'web-app' -Port 80 -PhysicalPath 'C:\web-app' -ApplicationPool '.NET v2.0'

COPY --from=builder C:\out\web\SignUpWeb\_PublishedWebsites\SignUp.Web .