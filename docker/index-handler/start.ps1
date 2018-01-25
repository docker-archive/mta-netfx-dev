Write-Output 'Configuring app settings'
if ($env:APP_SETTINGS_PATH -And (Test-Path $env:APP_SETTINGS_PATH)) {
    
    Remove-Item -Force -Path "$env:APP_ROOT\appSettings.config"
        
    New-Item -Path "$env:APP_ROOT\appSettings.config" `
             -ItemType SymbolicLink `
             -Value $env:APP_SETTINGS_PATH

    Write-Verbose 'INFO: Using configured app settings path'
}
else {
    Write-Verbose 'WARN: Using default app settings, APP_SETTINGS_PATH not set'
}

Write-Output 'Starting handler'
.\SignUp.MessageHandlers.IndexProspect.exe