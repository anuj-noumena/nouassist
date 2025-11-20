namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Exceptions
{
    /// <summary>
    /// Exception thrown when a scenario configuration is invalid
    /// </summary>
    public class InvalidScenarioException : PerformanceFinancialModelingDomainException
    {
        public InvalidScenarioException(string scenarioName, string reason)
            : base("PerformanceFinancialModeling:ErrorMessage:InvalidScenario")
        {
            WithData("scenarioName", scenarioName);
            WithData("reason", reason);
        }
    }
}
