# escape=`
FROM microsoft/dotnet-framework:4.7.1-windowsservercore-10.0.14393.1884
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

RUN Invoke-WebRequest -UseBasicParsing https://chocolatey.org/install.ps1 | Invoke-Expression; `
    choco install nunit-console-runner --version 3.6.1 -y;

ENV NUNIT_PATH="C:\NUnit.ConsoleRunner.3.6.1\tools"
RUN $env:PATH = $env:NUNIT_PATH + ';' + $env:PATH; `
	[Environment]::SetEnvironmentVariable('PATH', $env:PATH, [EnvironmentVariableTarget]::Machine);

ENTRYPOINT ["powershell"]