namespace NouAssist.RequirementsManagement.Elicitation.Services
{
    /// <summary>
    /// Domain service interface for managing stakeholder profiles
    /// </summary>
    public interface IStakeholderProfileManager : IDomainService
    {
        Task<StakeholderProfile> CreateAsync(
            string name,
            string description,
            StakeholderRole role,
            string organization,
            ContactInfo contactInfo,
            InfluenceData influenceData);

        Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null);

        Task<List<StakeholderProfile>> GetKeyStakeholdersAsync();

        Task<List<StakeholderProfile>> GetStakeholdersByRoleAsync(params StakeholderRole[] roles);

        Task ValidateStakeholderAsync(Guid stakeholderId);
    }
}
