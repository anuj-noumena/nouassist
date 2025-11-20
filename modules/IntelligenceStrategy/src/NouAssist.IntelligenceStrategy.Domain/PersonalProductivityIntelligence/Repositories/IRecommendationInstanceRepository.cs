using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Specifications;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories
{
    /// <summary>
    /// Repository for RecommendationInstance aggregate
    /// </summary>
    public interface IRecommendationInstanceRepository : IRepository<RecommendationInstance, Guid>
    {
        /// <summary>
        /// Gets active recommendations for a user
        /// </summary>
        Task<List<RecommendationInstance>> GetActiveRecommendationsAsync(
            Guid userId,
            RecommendationType? recommendationType = null,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets recommendations matching a specification
        /// </summary>
        Task<List<RecommendationInstance>> GetListAsync(
            Specification<RecommendationInstance> specification,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets recommendations requiring user attention
        /// </summary>
        Task<List<RecommendationInstance>> GetAttentionNeededRecommendationsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
