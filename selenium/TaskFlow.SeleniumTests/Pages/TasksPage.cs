using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TaskFlow.SeleniumTests.Pages;

public class TasksPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public TasksPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public void Create(string title, string description = "Escenario automatizado con Selenium")
    {
        Set(By.Id("task-title"), title);
        Set(By.Id("task-description"), description);
        _driver.FindElement(By.Id("save-task")).Click();
        _wait.Until(d => !string.IsNullOrWhiteSpace(d.FindElement(By.Id("form-message")).Text));
    }

    public void Edit(string currentTitle, string newTitle, bool completed = true)
    {
        var item = FindTask(currentTitle);
        item.FindElement(By.CssSelector(".edit-task")).Click();
        Set(By.Id("task-title"), newTitle);
        var check = _driver.FindElement(By.Id("task-completed"));
        if (check.Selected != completed) check.Click();
        _driver.FindElement(By.Id("save-task")).Click();
    }

    public void Delete(string title, bool confirm = true)
    {
        FindTask(title).FindElement(By.CssSelector(".delete-task")).Click();
        _wait.Until(d => d.FindElement(By.Id("confirm-modal")).Displayed);
        _driver.FindElement(By.Id(confirm ? "confirm-delete" : "cancel-delete")).Click();
    }

    public bool Exists(string title)
    {
        try
        {
            return _wait.Until(d =>
            {
                try
                {
                    return d.FindElements(By.CssSelector("#task-list .task h3"))
                        .Any(x => x.Text == title);
                }
                catch (StaleElementReferenceException) { return false; }
            });
        }
        catch (WebDriverTimeoutException) { return false; }
    }

    public string Message => _wait.Until(d => d.FindElement(By.Id("form-message")).Text);
    public string TitleValue => _driver.FindElement(By.Id("task-title")).GetAttribute("value") ?? string.Empty;

    private IWebElement FindTask(string title) => _wait.Until(d =>
    {
        try
        {
            return d.FindElements(By.CssSelector("#task-list .task"))
                .FirstOrDefault(x => x.FindElement(By.TagName("h3")).Text == title);
        }
        catch (StaleElementReferenceException) { return null; }
    })!;

    private void Set(By by, string value)
    {
        var element = _driver.FindElement(by);
        element.Clear();
        element.SendKeys(value);
    }
}
