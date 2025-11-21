using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.ChangeManagement.Services
{
    /// <summary>
    /// Service for assessing the impact of change requests
    /// </summary>
    public interface IChangeImpactAssessmentService : IDomainService
    {
        /// <summary>
        /// Conducts impact assessment for a change request
        /// </summary>
        Task<ImpactAssessmentResult> ConductImpactAssessmentAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Estimates cost impact of a change request
        /// </summary>
        Task<decimal?> EstimateCostImpactAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Estimates time impact of a change request
        /// </summary>
        Task<int?> EstimateTimeImpactAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Analyzes dependencies affected by a change request
        /// </summary>
        Task<List<DependencyAnalysisResult>> AnalyzeDependenciesAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Result of impact assessment
    /// </summary>
    public class ImpactAssessmentResult
    {
        public decimal? EstimatedCost { get; set; }
        public int? EstimatedTimeDays { get; set; }
        public List<string> AffectedComponents { get; set; } = new();
        public List<DependencyAnalysisResult> Dependencies { get; set; } = new();
        public List<string> Risks { get; set; } = new();
        public List<ImpactDetails> DetailedImpacts { get; set; } = new();
    }

    /// <summary>
    /// Result of dependency analysis
    /// </summary>
    public class DependencyAnalysisResult
    {
        public Guid DependentRequirementId { get; set; }
        public string DependencyType { get; set; }
        public string ImpactDescription { get; set; }
        public bool RequiresUpdate { get; set; }
    }
}
