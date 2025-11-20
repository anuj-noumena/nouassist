namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories
{
    /// <summary>
    /// Repository for intellectual property assets
    /// </summary>
    public interface IIntellectualPropertyRepository : IRepository<IntellectualPropertyAsset, Guid>
    {
        /// <summary>
        /// Checks if an IP asset with the given identifier already exists
        /// </summary>
        Task<bool> ExistsByIdentifierAsync(
            string identifier, 
            IntellectualPropertyType type, 
            Guid? exceptId = null,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets IP assets with approaching deadlines
        /// </summary>
        Task<List<IntellectualPropertyAsset>> GetWithApproachingDeadlinesAsync(
            int daysThreshold, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets IP assets by filing status
        /// </summary>
        Task<List<IntellectualPropertyAsset>> GetByStatusAsync(
            FilingStatusType status, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets IP assets by type
        /// </summary>
        Task<List<IntellectualPropertyAsset>> GetByTypeAsync(
            IntellectualPropertyType type,
            CancellationToken cancellationToken = default);
    }
}
