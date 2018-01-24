Write-Output '*** Building App'

& docker-compose $config `
    -f .\app\part-3\docker-compose.yml `
    -f .\app\part-3\docker-compose-build.yml `
    build