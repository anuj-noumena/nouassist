using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Specifications;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories
{
    /// <summary>
    /// Repository for PredictionInstance aggregate
    /// </summary>
    public interface IPredictionInstanceRepository : IRepository<PredictionInstance, Guid>
    {
        /// <summary>
        /// Gets predictions for a user
        /// </summary>
        Task<List<PredictionInstance>> GetUserPredictionsAsync(
            Guid userId,
            string predictionType = null,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets predictions for a specific entity
        /// </summary>
        Task<List<PredictionInstance>> GetPredictionsForEntityAsync(
            string entityType,
            Guid entityId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets predictions matching a specification
        /// </summary>
        Task<List<PredictionInstance>> GetListAsync(
            Specification<PredictionInstance> specification,
            CancellationToken cancellationToken = default);
    }
}
