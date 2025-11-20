namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Exceptions
{
    /// <summary>
    /// Exception thrown when a financial model is not found
    /// </summary>
    public class FinancialModelNotFoundException : PerformanceFinancialModelingDomainException
    {
        public FinancialModelNotFoundException(Guid modelId)
            : base("PerformanceFinancialModeling:ErrorMessage:ModelNotFound")
        {
            WithData("modelId", modelId);
        }
    }
}
