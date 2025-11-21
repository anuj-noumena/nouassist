using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service interface for calculating prioritization scores and rankings
    /// </summary>
    public interface IPrioritizationCalculationService
    {
        Task<Dictionary<Guid, RankScore>> CalculatePrioritizationResultsAsync(
            Guid activityId,
            CancellationToken cancellationToken = default);

        Task<RankScore> CalculateIndividualScoreAsync(
            Guid activityId,
            Guid requirementId,
            CancellationToken cancellationToken = default);

        Task<decimal> CalculateConsensusAsync(
            Guid activityId,
            CancellationToken cancellationToken = default);

        Task<Dictionary<Guid, decimal>> CalculateStakeholderInfluenceAsync(
            Guid activityId,
            CancellationToken cancellationToken = default);
    }
}
