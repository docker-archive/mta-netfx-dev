Write-Output 'Configuring DB connection'
if ($env:DB_CONNECTION_STRING_PATH -And (Test-Path $env:DB_CONNECTION_STRING_PATH)) {
    
    Remove-Item -Force -Path "$env:APP_ROOT\connectionStrings.config"
        
    New-Item -Path "$env:APP_ROOT\connectionStrings.config" `
             -ItemType SymbolicLink `
             -Value $env:DB_CONNECTION_STRING_PATH

    Write-Verbose "INFO: Using connection string from secret"
}
else {
    Write-Verbose "WARN: Using default connection strings, secret file not found at: $env:DB_CONNECTION_STRING_PATH"
}

Write-Output 'Starting IIS'
Start-Service W3SVC

Write-Output 'Tailing log'
Invoke-WebRequest http://localhost/SignUp.aspx -UseBasicParsing | Out-Null
Get-Content -path "$env:APP_ROOT\App_Data\SignUp.log" -Tail 1 -Wait 