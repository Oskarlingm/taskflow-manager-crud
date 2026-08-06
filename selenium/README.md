# Pruebas Selenium de TaskFlow

Las pruebas usan C#, NUnit, Selenium WebDriver y ExtentReports. Cubren 5 historias de usuario con camino feliz, prueba negativa y prueba de limites (15 escenarios).

## Ejecucion

1. Inicie la aplicacion:
   ```powershell
   cd backend\TaskFlow.API
   dotnet run --launch-profile http
   ```
2. En otra terminal, ejecute las pruebas:
   ```powershell
   cd selenium\TaskFlow.SeleniumTests
   dotnet test
   ```
3. Abra `TestResults\TaskFlowReport.html`.

Cada prueba crea una captura en `TestResults\Screenshots`.
