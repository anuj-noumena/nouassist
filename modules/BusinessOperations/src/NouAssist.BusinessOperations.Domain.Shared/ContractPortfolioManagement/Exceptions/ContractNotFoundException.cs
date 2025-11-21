using System;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a contract is not found
    /// </summary>
    public class ContractNotFoundException : ContractPortfolioManagementDomainException
    {
        public Guid ContractId { get; }

        public ContractNotFoundException(Guid contractId)
            : base("ContractPortfolioManagement:ErrorMessage:ContractNotFound")
        {
            ContractId = contractId;
            WithData("contractId", contractId);
        }
    }
}
