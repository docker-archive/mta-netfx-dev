Write-Output '*** Pushing Images'

& docker-compose $config `
    -f .\app\part-3\docker-compose.yml `
    push --ignore-push-failures