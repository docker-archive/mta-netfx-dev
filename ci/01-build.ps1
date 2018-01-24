Write-Output '*** Building App'

& docker-compose $config `
    -f .\app\docker-compose.yml `
    -f .\app\docker-compose-build.yml `
    build