namespace NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces
{
    /// <summary>
    /// Interface for the baselining manager domain service
    /// </summary>
    public interface IBaseliningManager : IDomainService
    {
        /// <summary>
        /// Creates a new baseline
        /// </summary>
        Task<BaselineDefinition> CreateBaselineAsync(
            string name,
            string description,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Adds a requirement to a baseline
        /// </summary>
        Task AddRequirementToBaselineAsync(
            Guid baselineId,
            Guid requirementId,
            string version,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Removes a requirement from a baseline
        /// </summary>
        Task RemoveRequirementFromBaselineAsync(
            Guid baselineId,
            Guid requirementId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates a requirement's version in a baseline
        /// </summary>
        Task UpdateRequirementVersionInBaselineAsync(
            Guid baselineId,
            Guid requirementId,
            string newVersion,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Activates a baseline
        /// </summary>
        Task ActivateBaselineAsync(
            Guid baselineId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Archives a baseline
        /// </summary>
        Task ArchiveBaselineAsync(
            Guid baselineId,
            CancellationToken cancellationToken = default);
    }
}
