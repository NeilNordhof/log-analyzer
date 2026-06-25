dotnet pack ./log-analyzer/log-analyzer.csproj -c Release -o ./nupkg

if ($LASTEXITCODE -ne 0) {
    Write-Host "Pack failed."
    exit 1
}

dotnet tool install -g --add-source ./nupkg log-analyzer

if ($LASTEXITCODE -ne 0) {
    dotnet tool update -g --add-source ./nupkg log-analyzer
}
