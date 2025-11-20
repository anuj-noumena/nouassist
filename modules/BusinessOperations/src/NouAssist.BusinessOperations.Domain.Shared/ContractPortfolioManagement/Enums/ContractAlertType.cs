namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Enums
{
    /// <summary>
    /// Represents the type of a contract alert
    /// </summary>
    public enum ContractAlertType
    {
        Renewal = 0,
        Expiration = 10,
        Payment = 20,
        Deliverable = 30,
        Compliance = 40,
        Custom = 50
    }
}
