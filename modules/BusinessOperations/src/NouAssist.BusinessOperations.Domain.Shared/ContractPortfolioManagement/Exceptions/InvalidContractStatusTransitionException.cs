namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a contract status transition is invalid
    /// </summary>
    public class InvalidContractStatusTransitionException : ContractPortfolioManagementDomainException
    {
        public ContractStatus CurrentStatus { get; }
        public ContractStatus RequestedStatus { get; }

        public InvalidContractStatusTransitionException(ContractStatus currentStatus, ContractStatus requestedStatus)
            : base("ContractPortfolioManagement:ErrorMessage:InvalidContractStatusTransition")
        {
            CurrentStatus = currentStatus;
            RequestedStatus = requestedStatus;
            WithData("currentStatus", currentStatus);
            WithData("requestedStatus", requestedStatus);
        }
    }
}
