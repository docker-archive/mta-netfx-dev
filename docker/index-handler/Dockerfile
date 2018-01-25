# escape=`
FROM microsoft/dotnet-framework-build:4.7.1-windowsservercore-ltsc2016 AS builder

WORKDIR C:\src\SignUp.MessageHandlers.IndexProspect
COPY .\src\SignUp\SignUp.MessageHandlers.IndexProspect\packages.config .
RUN nuget restore packages.config -PackagesDirectory ..\packages

COPY src\SignUp C:\src
RUN msbuild SignUp.MessageHandlers.IndexProspect.csproj /p:OutputPath=c:\out\

# app image
FROM microsoft/dotnet-framework:4.7.1-windowsservercore-10.0.14393.1884

ENV APP_ROOT="C:\\index-prospect-handler" `
    MESSAGE_QUEUE_URL="nats://message-queue:4222" `
    APP_SETTINGS_PATH=""

WORKDIR $APP_ROOT
COPY docker\index-handler\start.ps1 .
CMD ["powershell", ".\\start.ps1"]

COPY --from=builder C:\out .