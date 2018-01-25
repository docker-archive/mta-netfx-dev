# escape=`
FROM microsoft/dotnet-framework-build:4.7.1-windowsservercore-ltsc2016 AS builder

WORKDIR C:\src\SignUp.EndToEndTests
COPY src\SignUp\SignUp.EndToEndTests\packages.config .
RUN nuget restore packages.config -PackagesDirectory ..\packages

COPY src\SignUp C:\src
RUN msbuild SignUp.EndToEndTests.csproj /p:OutputPath=C:\out

# test runner
FROM dockersamples/mta-dev-test-runner 
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; "]

ENV APP_ROOT="C:\\e2e-tests" `
    DB_CONNECTION_STRING_PATH=""

WORKDIR $APP_ROOT
COPY .\docker\e2e-tests\run.ps1 .
ENTRYPOINT ["powershell", ".\\run.ps1"]

COPY --from=builder C:\out .