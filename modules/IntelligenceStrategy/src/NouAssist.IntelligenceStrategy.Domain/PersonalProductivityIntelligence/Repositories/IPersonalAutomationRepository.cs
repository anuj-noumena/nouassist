using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories
{
    /// <summary>
    /// Repository for PersonalAutomation aggregate
    /// </summary>
    public interface IPersonalAutomationRepository : IRepository<PersonalAutomation, Guid>
    {
        /// <summary>
        /// Gets automations for a user
        /// </summary>
        Task<List<PersonalAutomation>> GetUserAutomationsAsync(
            Guid userId,
            bool? isEnabled = null,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets automations with a specific trigger type
        /// </summary>
        Task<List<PersonalAutomation>> GetAutomationsWithTriggerTypeAsync(
            Guid userId,
            AutomationTriggerType triggerType,
            CancellationToken cancellationToken = default);
    }
}
