$env:DOTNET_ENVIRONMENT = "Development"
$env:COREHOST_TRACE = "1"

Write-Host "Starting FlowGuide Player with Debug Output..." -ForegroundColor Green
Write-Host "Debug logs will appear below:" -ForegroundColor Yellow
Write-Host "=" * 60 -ForegroundColor Yellow

dotnet run --project FlowGuide.Player/FlowGuide.Player.csproj
