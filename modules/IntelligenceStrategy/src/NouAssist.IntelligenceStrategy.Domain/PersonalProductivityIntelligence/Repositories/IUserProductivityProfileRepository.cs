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
    /// Repository for UserProductivityProfile aggregate
    /// </summary>
    public interface IUserProductivityProfileRepository : IRepository<UserProductivityProfile, Guid>
    {
        /// <summary>
        /// Finds a profile by user ID
        /// </summary>
        Task<UserProductivityProfile> FindByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets profiles with high cognitive load
        /// </summary>
        Task<List<UserProductivityProfile>> GetHighCognitiveLoadProfilesAsync(
            CognitiveLoadLevel minLoadLevel,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets profiles with work patterns updated before a specific date
        /// </summary>
        Task<List<UserProductivityProfile>> GetProfilesWithOutdatedWorkPatternsAsync(
            DateTime thresholdDate,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
    }
}
