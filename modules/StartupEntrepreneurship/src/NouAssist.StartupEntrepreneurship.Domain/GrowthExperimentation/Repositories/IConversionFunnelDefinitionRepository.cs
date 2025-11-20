namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories
{
    /// <summary>
    /// Repository interface for ConversionFunnelDefinition aggregate
    /// </summary>
    public interface IConversionFunnelDefinitionRepository : IRepository<ConversionFunnelDefinition, Guid>
    {
        /// <summary>
        /// Gets a funnel with all stages included
        /// </summary>
        Task<ConversionFunnelDefinition> GetWithStagesAsync(
            Guid id,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets all active funnels
        /// </summary>
        Task<List<ConversionFunnelDefinition>> GetActiveFunnelsAsync(
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates conversion rate data for a funnel stage
        /// </summary>
        Task UpdateStageConversionRateAsync(
            Guid funnelId,
            Guid stageId,
            ConversionRatePerStage conversionRate,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a specific stage from a funnel
        /// </summary>
        Task<FunnelStage> GetStageAsync(
            Guid funnelId,
            Guid stageId,
            CancellationToken cancellationToken = default);
    }
}
