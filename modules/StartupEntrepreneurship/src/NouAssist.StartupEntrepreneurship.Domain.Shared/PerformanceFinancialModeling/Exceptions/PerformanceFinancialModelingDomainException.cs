namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Exceptions
{
    /// <summary>
    /// Base exception for Performance & Financial Modeling domain
    /// </summary>
    public class PerformanceFinancialModelingDomainException : BusinessException
    {
        public PerformanceFinancialModelingDomainException(string code)
            : base(code)
        {
        }

        public PerformanceFinancialModelingDomainException(string code, string message)
            : base(code, message)
        {
        }

        public PerformanceFinancialModelingDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
