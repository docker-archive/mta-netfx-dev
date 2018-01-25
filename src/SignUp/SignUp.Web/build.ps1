
nuget restore packages.config -PackagesDirectory ..\packages
msbuild SignUp.Web.csproj /p:OutputPath=c:\out\web\SignUpWeb /p:DeployOnBuild=true