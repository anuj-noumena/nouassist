using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp.Domain.Services;

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
