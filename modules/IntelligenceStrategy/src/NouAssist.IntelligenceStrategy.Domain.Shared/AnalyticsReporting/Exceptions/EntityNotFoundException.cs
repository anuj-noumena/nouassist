namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class EntityNotFoundException : AnalyticsReportingDomainException
    {
        public EntityNotFoundException(string entityType, object id)
            : base("AnalyticsReporting:ErrorMessage:EntityNotFound")
        {
            WithData("entityType", entityType);
            WithData("id", id);
        }
    }
}
