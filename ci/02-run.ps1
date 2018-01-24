Write-Output '*** Running App'

& docker-compose $config -f .\app\part-3\docker-compose.yml up -d

Write-Output '*** Verifying running containers'

& docker $config container ls

Write-Output '*** Waiting for app startup & verifying IIS'

Start-Sleep -s 15
$ip = & docker $config container inspect --format '{{ .NetworkSettings.Networks.nat.IPAddress }}' part3_signup-app_1
Invoke-WebRequest -UseBasicParsing "http://$ip"

Write-Output '*** Web app running at http://$ip'