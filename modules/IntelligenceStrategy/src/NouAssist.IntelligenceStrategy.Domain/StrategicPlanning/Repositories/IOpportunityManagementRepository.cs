namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories
{
    /// <summary>
    /// Repository interface for Opportunity Management Record aggregate
    /// </summary>
    public interface IOpportunityManagementRepository : IRepository<OpportunityManagementRecord, Guid>
    {
        Task<List<OpportunityManagementRecord>> GetActiveRecordsAsync(
            CancellationToken cancellationToken = default);

        Task<OpportunityManagementRecord> GetRecordWithFilterRulesAsync(
            Guid recordId,
            CancellationToken cancellationToken = default);

        Task<List<OpportunityManagementRecord>> GetRecordsByFocusLevelAsync(
            FocusLevel focusLevel,
            CancellationToken cancellationToken = default);
    }
}
