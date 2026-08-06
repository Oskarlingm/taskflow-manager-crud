$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location "$root\selenium\TaskFlow.SeleniumTests"

$env:TASKFLOW_BASE_URL = "http://localhost:5116"
dotnet restore
dotnet test --logger "console;verbosity=normal"

$report = Join-Path (Get-Location) "bin\Debug\net10.0\TestResults\TaskFlowReport.html"
if (Test-Path $report) {
    Write-Host "Pruebas finalizadas. Abriendo reporte HTML..." -ForegroundColor Green
    Start-Process $report
} else {
    Write-Host "No se genero el reporte. Revisa el error mostrado arriba." -ForegroundColor Red
}
