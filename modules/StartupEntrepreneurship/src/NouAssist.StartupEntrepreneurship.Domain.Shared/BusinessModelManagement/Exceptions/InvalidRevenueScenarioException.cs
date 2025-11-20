namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a revenue scenario contains invalid parameters
    /// </summary>
    public class InvalidRevenueScenarioException : BusinessModelManagementDomainException
    {
        public InvalidRevenueScenarioException(string parameterName, object value)
            : base("BusinessModelManagement:ErrorMessage:InvalidRevenueScenario")
        {
            WithData("parameterName", parameterName);
            WithData("value", value);
        }
    }
}
