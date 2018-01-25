Write-Output '*** Building E2E test image'

& docker $config image build -t dockersamples/mta-dev-e2e-tests -f .\docker\e2e-tests\Dockerfile .

Write-Output '*** Running E2E tests'

& docker $config container rm -f mta-dev-e2e-tests
& docker $config container run --name mta-dev-e2e-tests dockersamples/mta-dev-e2e-tests

Write-Output '*** Checking results'

& docker $config container cp mta-dev-e2e-tests:C:\e2e-tests\TestResult.xml .
$results = [xml] (Get-Content .\TestResult.xml)
$result = ($results.SelectNodes('./test-run/test-suite')).result
Write-Output "*** E2E test result: $result"

if ($result -eq 'Failed') { exit 1 }