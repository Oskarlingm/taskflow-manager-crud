using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TaskFlow.SeleniumTests.Pages;

public class LoginPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public LoginPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public void Open() => _driver.Navigate().GoToUrl(TestConfig.BaseUrl);

    public void Login(string email, string password)
    {
        _driver.FindElement(By.Id("email")).Clear();
        _driver.FindElement(By.Id("email")).SendKeys(email);
        _driver.FindElement(By.Id("password")).Clear();
        _driver.FindElement(By.Id("password")).SendKeys(password);
        _driver.FindElement(By.Id("login-button")).Click();
    }

    public void SubmitEmpty() => _driver.FindElement(By.Id("login-button")).Click();
    public string Error => _wait.Until(d => d.FindElement(By.Id("login-error")).Text);
    public bool DashboardVisible => _wait.Until(d => d.FindElement(By.Id("app-view")).Displayed);
}
