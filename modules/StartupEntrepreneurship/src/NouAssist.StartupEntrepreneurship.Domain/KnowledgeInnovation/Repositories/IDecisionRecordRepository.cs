namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories
{
    /// <summary>
    /// Repository for DecisionRecord aggregate
    /// </summary>
    public interface IDecisionRecordRepository : IRepository<DecisionRecord, Guid>
    {
        /// <summary>
        /// Find decision records by specification
        /// </summary>
        Task<List<DecisionRecord>> GetListAsync(
            Specification<DecisionRecord> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Count decision records matching specification
        /// </summary>
        Task<int> CountAsync(
            Specification<DecisionRecord> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if any decision records match specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<DecisionRecord> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get decisions by status
        /// </summary>
        Task<List<DecisionRecord>> GetByStatusAsync(
            DecisionStatus status, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get recent decisions since a specific date
        /// </summary>
        Task<List<DecisionRecord>> GetRecentDecisionsAsync(
            DateTime sinceDate, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get decisions with specific tag
        /// </summary>
        Task<List<DecisionRecord>> GetByTagAsync(
            string tag, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get decisions that can be implemented
        /// </summary>
        Task<List<DecisionRecord>> GetImplementableDecisionsAsync(
            CancellationToken cancellationToken = default);
    }
}
