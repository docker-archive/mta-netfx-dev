Write-Output '*** Running app with local ports'

& docker-compose $config `
    -f .\app\docker-compose.yml `
    -f .\app\docker-compose-local.yml `
    up -d
