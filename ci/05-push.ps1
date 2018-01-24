Write-Output '*** Pushing Images'

$services = 'signup-app', 'signup-save-handler', 'signup-index-handler', 'signup-homepage'

foreach ($service in $services) {

    & docker-compose $config `
        -f .\app\docker-compose.yml `
        push $service
}