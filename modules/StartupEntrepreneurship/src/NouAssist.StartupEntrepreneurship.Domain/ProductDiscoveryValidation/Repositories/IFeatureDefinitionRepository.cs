using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories
{
    /// <summary>
    /// Repository interface for FeatureDefinition aggregate
    /// </summary>
    public interface IFeatureDefinitionRepository : IRepository<FeatureDefinition, Guid>
    {
        /// <summary>
        /// Finds features matching the given specification
        /// </summary>
        Task<List<FeatureDefinition>> GetListAsync(
            Expression<Func<FeatureDefinition, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds features by priority
        /// </summary>
        Task<List<FeatureDefinition>> GetByPriorityAsync(
            FeaturePriority priority,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds features by MVP status
        /// </summary>
        Task<List<FeatureDefinition>> GetByMvpStatusAsync(
            MvpStatus mvpStatus,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets feature with all priority scores
        /// </summary>
        Task<FeatureDefinition> GetWithPriorityScoresAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds MVP-ready features
        /// </summary>
        Task<List<FeatureDefinition>> GetMvpReadyAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds high-impact features
        /// </summary>
        Task<List<FeatureDefinition>> GetHighImpactAsync(
            decimal minimumPriorityScore = 10m,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds features needing prioritization
        /// </summary>
        Task<List<FeatureDefinition>> GetNeedingPrioritizationAsync(
            int daysSinceLastPrioritization = 30,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds features related to validated hypotheses
        /// </summary>
        Task<List<FeatureDefinition>> GetValidatedHypothesisAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets feature statistics
        /// </summary>
        Task<FeatureStatistics> GetStatisticsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches features by name or description
        /// </summary>
        Task<List<FeatureDefinition>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets features ordered by priority score
        /// </summary>
        Task<List<FeatureDefinition>> GetOrderedByPriorityAsync(
            int? maxResults = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets feature priority distribution
        /// </summary>
        Task<Dictionary<FeaturePriority, int>> GetPriorityDistributionAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets MVP status distribution
        /// </summary>
        Task<Dictionary<MvpStatus, int>> GetMvpStatusDistributionAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds features related to a specific hypothesis
        /// </summary>
        Task<List<FeatureDefinition>> GetByHypothesisAsync(
            Guid hypothesisId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds features related to a specific prototype
        /// </summary>
        Task<List<FeatureDefinition>> GetByPrototypeAsync(
            Guid prototypeId,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Data transfer object for feature statistics
    /// </summary>
    public class FeatureStatistics
    {
        public int TotalFeatures { get; set; }
        public Dictionary<FeaturePriority, int> FeaturesByPriority { get; set; }
        public Dictionary<MvpStatus, int> FeaturesByMvpStatus { get; set; }
        public int FeaturesWithPriorityScores { get; set; }
        public int FeaturesInMvp { get; set; }
        public int FeaturesNeedingPrioritization { get; set; }
        public decimal AveragePriorityScore { get; set; }
        public int HighImpactFeatures { get; set; }
        public Dictionary<string, int> FeaturesByHypothesis { get; set; }
        public Dictionary<string, int> FeaturesByPrototype { get; set; }
    }
}
