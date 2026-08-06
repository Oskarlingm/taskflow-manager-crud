# TaskFlow Manager CRUD

Sistema web para administrar tareas, ampliado con inicio de sesion y pruebas automatizadas con Selenium.

## Funciones

- Inicio y cierre de sesion.
- Crear, consultar, actualizar y eliminar tareas.
- Validaciones y confirmacion antes de eliminar.
- 15 escenarios Selenium en 5 historias de usuario.
- Reporte HTML y captura automatica por escenario.

## Tecnologias

- ASP.NET Core Web API, Entity Framework Core y SQLite.
- HTML, CSS y JavaScript.
- C#, NUnit, Selenium WebDriver y ExtentReports.

## Ejecutar la aplicacion

```powershell
cd backend\TaskFlow.API
dotnet restore
dotnet run --launch-profile http
```

Abra `http://localhost:5116` e inicie sesion con:

- Correo: `admin@taskflow.com`
- Contrasena: `TaskFlow123!`

## Ejecutar Selenium

Con la aplicacion iniciada, abra otra terminal:

```powershell
cd selenium\TaskFlow.SeleniumTests
dotnet restore
dotnet test
```

El reporte queda en `TestResults\TaskFlowReport.html` y las evidencias en `TestResults\Screenshots`.

Las historias listas para Jira/Azure estan en `docs/HISTORIAS_USUARIO.md`.
