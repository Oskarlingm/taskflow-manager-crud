namespace TaskFlow.SeleniumTests;

public static class TestConfig
{
    public static string BaseUrl =>
        Environment.GetEnvironmentVariable("TASKFLOW_BASE_URL") ?? "http://localhost:5116";

    public const string ValidEmail = "admin@taskflow.com";
    public const string ValidPassword = "TaskFlow123!";
}
