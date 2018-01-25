# escape=`
FROM microsoft/dotnet-framework-build:4.7.1-windowsservercore-ltsc2016 AS builder

WORKDIR C:\src\SignUp.MessageHandlers.SaveProspect
COPY .\src\SignUp\SignUp.MessageHandlers.SaveProspect\packages.config .
RUN nuget restore packages.config -PackagesDirectory ..\packages

COPY src\SignUp C:\src
RUN msbuild SignUp.MessageHandlers.SaveProspect.csproj /p:OutputPath=c:\out\

# app image
FROM microsoft/dotnet-framework:4.7.1-windowsservercore-10.0.14393.1884

ENV APP_ROOT="C:\\save-prospect-handler" `
    MESSAGE_QUEUE_URL="nats://message-queue:4222" `
    DB_CONNECTION_STRING_PATH=""

WORKDIR $APP_ROOT
COPY docker\save-handler\start.ps1 .
CMD ["powershell", ".\\start.ps1"]

COPY --from=builder C:\out .