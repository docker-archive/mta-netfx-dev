Write-Output '*** Generating PROD stack file'

& docker-compose $config `
     -f .\app\docker-compose.yml `
     -f .\app\docker-compose-PROD.yml `
    config > docker-stack.yml

Write-Output '*** Deploying to PROD'   

& docker $prodConfig stack deploy -c docker-stack.yml signup