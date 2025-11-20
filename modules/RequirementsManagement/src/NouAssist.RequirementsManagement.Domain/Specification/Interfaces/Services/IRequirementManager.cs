namespace NouAssist.RequirementsManagement.Specification.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for requirement management
    /// </summary>
    public interface IRequirementManager : IDomainService
    {
        Task<Requirement> CreateAsync(
            RequirementIdentifier identifier,
            RequirementText text,
            RequirementType type,
            RequirementPriority priority,
            Guid? tenantId = null);

        Task<bool> IsIdentifierUniqueAsync(RequirementIdentifier identifier, Guid? excludeId = null);

        Task ValidateForDeletionAsync(Requirement requirement);
    }
}
