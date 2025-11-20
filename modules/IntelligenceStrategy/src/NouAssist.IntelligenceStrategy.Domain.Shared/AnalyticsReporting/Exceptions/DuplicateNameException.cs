namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity with the same name already exists
    /// </summary>
    public class DuplicateNameException : AnalyticsReportingDomainException
    {
        public DuplicateNameException(string entityType, string name)
            : base("AnalyticsReporting:ErrorMessage:DuplicateName")
        {
            WithData("entityType", entityType);
            WithData("name", name);
        }
    }
}
