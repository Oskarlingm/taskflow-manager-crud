using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TaskFlow.SeleniumTests.Pages;

namespace TaskFlow.SeleniumTests.Tests;

[TestFixture]
[NonParallelizable]
public class TaskFlowTests
{
    private static readonly string ResultsDirectory = Path.Combine(AppContext.BaseDirectory, "TestResults");
    private static ExtentReports _report = null!;
    private IWebDriver _driver = null!;
    private ExtentTest _extentTest = null!;
    private LoginPage _login = null!;
    private TasksPage _tasks = null!;

    [OneTimeSetUp]
    public void ConfigureReport()
    {
        Directory.CreateDirectory(ResultsDirectory);
        Directory.CreateDirectory(Path.Combine(ResultsDirectory, "Screenshots"));
        var reporter = new ExtentSparkReporter(Path.Combine(ResultsDirectory, "TaskFlowReport.html"));
        reporter.Config.DocumentTitle = "Reporte Selenium - TaskFlow";
        reporter.Config.ReportName = "Pruebas automatizadas de login y CRUD";
        _report = new ExtentReports();
        _report.AttachReporter(reporter);
        _report.AddSystemInfo("Aplicacion", "TaskFlow Manager CRUD");
        _report.AddSystemInfo("Navegador", "Google Chrome");
    }

    [SetUp]
    public void StartBrowser()
    {
        _extentTest = _report.CreateTest(TestContext.CurrentContext.Test.Name);
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-search-engine-choice-screen");
        if (Environment.GetEnvironmentVariable("HEADLESS") == "true") options.AddArgument("--headless=new");
        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        _login = new LoginPage(_driver);
        _tasks = new TasksPage(_driver);
        _login.Open();
    }

    [TearDown]
    public void FinishTest()
    {
        var name = TestContext.CurrentContext.Test.Name;
        var safeName = string.Concat(name.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
        var screenshotPath = Path.Combine(ResultsDirectory, "Screenshots", $"{safeName}.png");
        try
        {
            ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile(screenshotPath);
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
                _extentTest.Pass("Escenario ejecutado correctamente.");
            else
                _extentTest.Fail(TestContext.CurrentContext.Result.Message);
            _extentTest.AddScreenCaptureFromPath(Path.Combine("Screenshots", $"{safeName}.png"));
        }
        finally { _driver?.Quit(); }
    }

    [OneTimeTearDown]
    public void SaveReport() => _report.Flush();

    private void ValidLogin()
    {
        _login.Login(TestConfig.ValidEmail, TestConfig.ValidPassword);
        Assert.That(_login.DashboardVisible, Is.True);
    }

    private static string Unique(string prefix) => $"{prefix} {DateTime.Now:HHmmssfff}";

    // HU-01: Inicio de sesion
    [Test] public void HU01_Login_CaminoFeliz()
    {
        _login.Login(TestConfig.ValidEmail, TestConfig.ValidPassword);
        Assert.That(_login.DashboardVisible, Is.True);
    }

    [Test] public void HU01_Login_Negativa_CredencialesIncorrectas()
    {
        _login.Login("incorrecto@taskflow.com", "Contrasena99!");
        Assert.That(_login.Error, Does.Contain("incorrectos"));
    }

    [Test] public void HU01_Login_Limite_CamposVacios()
    {
        _login.SubmitEmpty();
        Assert.That(_login.Error, Does.Contain("Complete"));
    }

    // HU-02: Crear tarea
    [Test] public void HU02_Crear_CaminoFeliz()
    {
        ValidLogin(); var title = Unique("Tarea creada"); _tasks.Create(title);
        Assert.That(_tasks.Exists(title), Is.True);
    }

    [Test] public void HU02_Crear_Negativa_TituloMuyCorto()
    {
        ValidLogin(); _tasks.Create("AB");
        Assert.That(_tasks.Message, Does.Contain("entre 3 y 100"));
    }

    [Test] public void HU02_Crear_Limite_TituloTresCaracteres()
    {
        ValidLogin(); _tasks.Create("ABC");
        Assert.That(_tasks.Exists("ABC"), Is.True);
    }

    // HU-03: Consultar tareas
    [Test] public void HU03_Consultar_CaminoFeliz_TareaVisible()
    {
        ValidLogin(); var title = Unique("Consultar"); _tasks.Create(title);
        Assert.That(_tasks.Exists(title), Is.True);
    }

    [Test] public void HU03_Consultar_Negativa_TareaInexistente()
    {
        ValidLogin();
        Assert.That(_tasks.Exists("Tarea que no existe 000000"), Is.False);
    }

    [Test] public void HU03_Consultar_Limite_TituloLargoVisible()
    {
        ValidLogin(); var title = new string('L', 100); _tasks.Create(title);
        Assert.That(_tasks.Exists(title), Is.True);
    }

    // HU-04: Actualizar tarea
    [Test] public void HU04_Actualizar_CaminoFeliz()
    {
        ValidLogin(); var original = Unique("Original"); var edited = Unique("Editada");
        _tasks.Create(original); _tasks.Edit(original, edited);
        Assert.That(_tasks.Exists(edited), Is.True);
    }

    [Test] public void HU04_Actualizar_Negativa_TituloInvalido()
    {
        ValidLogin(); var title = Unique("Para editar"); _tasks.Create(title); _tasks.Edit(title, "AB");
        Assert.That(_tasks.Message, Does.Contain("entre 3 y 100"));
    }

    [Test] public void HU04_Actualizar_Limite_TituloTresCaracteres()
    {
        ValidLogin(); var title = Unique("Limite editar"); _tasks.Create(title); _tasks.Edit(title, "XYZ");
        Assert.That(_tasks.Exists("XYZ"), Is.True);
    }

    // HU-05: Eliminar tarea
    [Test] public void HU05_Eliminar_CaminoFeliz()
    {
        ValidLogin(); var title = Unique("Eliminar"); _tasks.Create(title); _tasks.Delete(title);
        Assert.That(_tasks.Exists(title), Is.False);
    }

    [Test] public void HU05_Eliminar_Negativa_CancelarEliminacion()
    {
        ValidLogin(); var title = Unique("Conservar"); _tasks.Create(title); _tasks.Delete(title, false);
        Assert.That(_tasks.Exists(title), Is.True);
    }

    [Test] public void HU05_Eliminar_Limite_UltimaTareaCreada()
    {
        ValidLogin(); var title = Unique("Ultima"); _tasks.Create(title); _tasks.Delete(title);
        Assert.That(_tasks.Exists(title), Is.False);
    }
}
