namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Enums
{
    /// <summary>
    /// Represents the type of dependency between items
    /// </summary>
    public enum DependencyType
    {
        FinishToStart = 0,
        StartToStart = 10,
        FinishToFinish = 20,
        StartToFinish = 30
    }
}
