namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories
{
    /// <summary>
    /// Repository interface for GrowthExperiment aggregate
    /// </summary>
    public interface IGrowthExperimentRepository : IRepository<GrowthExperiment, Guid>
    {
        /// <summary>
        /// Gets experiments with their variants included
        /// </summary>
        Task<List<GrowthExperiment>> GetListWithVariantsAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets an experiment with all details included
        /// </summary>
        Task<GrowthExperiment> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets all experiments for a specific status
        /// </summary>
        Task<List<GrowthExperiment>> GetByStatusAsync(
            ExperimentStatus status,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets the results for a specific experiment variant
        /// </summary>
        Task<List<ExperimentResultData>> GetResultsForVariantAsync(
            Guid variantId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Adds result data for an experiment variant
        /// </summary>
        Task AddResultDataAsync(
            ExperimentResultData resultData,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates result data for an experiment variant
        /// </summary>
        Task UpdateResultDataAsync(
            ExperimentResultData resultData,
            CancellationToken cancellationToken = default);
    }
}
