namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions
{
    /// <summary>
    /// Base exception for ContractPortfolioManagement domain
    /// </summary>
    public class ContractPortfolioManagementDomainException : BusinessException
    {
        public ContractPortfolioManagementDomainException(string code)
            : base(code)
        {
        }

        public ContractPortfolioManagementDomainException(string code, string message)
            : base(code, message)
        {
        }

        public ContractPortfolioManagementDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
