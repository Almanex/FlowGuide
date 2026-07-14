$ErrorActionPreference = "Stop"

$outputDir = ".\Release"
if (Test-Path $outputDir) {
    Remove-Item $outputDir -Recurse -Force
}
New-Item -ItemType Directory -Path $outputDir | Out-Null

Write-Host "Building FlowGuide.Recorder..." -ForegroundColor Cyan
dotnet publish "FlowGuide.Recorder/FlowGuide.Recorder.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o "$outputDir\Recorder"

Write-Host "Building FlowGuide.Player..." -ForegroundColor Cyan
dotnet publish "FlowGuide.Player/FlowGuide.Player.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o "$outputDir\Player"

# Copy OpenCV DLLs if needed (usually handled by nuget, but good to check)
# Write-Host "Checking dependencies..."

# Create a launcher script
$launcherContent = @"
@echo off
start "" "Recorder\FlowGuide.Recorder.exe"
"@
Set-Content -Path "$outputDir\RunRecorder.bat" -Value $launcherContent

Write-Host "Build complete! Output in $outputDir" -ForegroundColor Green
Write-Host "You can zip the 'Release' folder and distribute it." -ForegroundColor Yellow
