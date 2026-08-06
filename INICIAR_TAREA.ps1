$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "Iniciando TaskFlow en http://localhost:5116" -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$root\backend\TaskFlow.API'; dotnet restore; dotnet run --launch-profile http"

Write-Host "Espera a que aparezca: Now listening on http://localhost:5116" -ForegroundColor Yellow
Write-Host "Luego abre http://localhost:5116" -ForegroundColor Green
Write-Host "Para las pruebas ejecuta EJECUTAR_PRUEBAS.ps1" -ForegroundColor Green
