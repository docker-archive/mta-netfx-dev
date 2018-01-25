Write-Output '*** Stopping App'

& docker $config container rm -f mta-dev-e2e-tests

& docker-compose $config `
     -f .\app\docker-compose.yml `
     -f .\app\docker-compose-test.yml `
    down

Write-Output '*** Verifying running containers'

& docker $config container ls