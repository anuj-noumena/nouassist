using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories
{
    /// <summary>
    /// Repository interface for ProblemHypothesis aggregate
    /// </summary>
    public interface IProblemHypothesisRepository : IRepository<ProblemHypothesis, Guid>
    {
        /// <summary>
        /// Finds hypotheses matching the given specification
        /// </summary>
        Task<List<ProblemHypothesis>> GetListAsync(
            Expression<Func<ProblemHypothesis, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds hypotheses by validation status
        /// </summary>
        Task<List<ProblemHypothesis>> GetByStatusAsync(
            ValidationStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds hypotheses related to a customer segment
        /// </summary>
        Task<List<ProblemHypothesis>> GetByCustomerSegmentAsync(
            Guid customerSegmentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets hypothesis with all validation attempts and metrics
        /// </summary>
        Task<ProblemHypothesis> GetWithValidationDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds strongly validated hypotheses
        /// </summary>
        Task<List<ProblemHypothesis>> GetStronglyValidatedAsync(
            int minimumValidationAttempts = 2,
            decimal minimumFitScore = 70m,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds hypotheses ready for validation
        /// </summary>
        Task<List<ProblemHypothesis>> GetReadyForValidationAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds stalled validation hypotheses
        /// </summary>
        Task<List<ProblemHypothesis>> GetStalledValidationAsync(
            int daysWithoutProgress = 14,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets validation portfolio metrics
        /// </summary>
        Task<ValidationPortfolioSummary> GetPortfolioSummaryAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches hypotheses by statement content
        /// </summary>
        Task<List<ProblemHypothesis>> SearchByStatementAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets hypotheses that need attention
        /// </summary>
        Task<List<ProblemHypothesis>> GetNeedingAttentionAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts hypotheses by validation status
        /// </summary>
        Task<Dictionary<ValidationStatus, int>> GetStatusDistributionAsync(
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Data transfer object for validation portfolio summary
    /// </summary>
    public class ValidationPortfolioSummary
    {
        public int TotalHypotheses { get; set; }
        public Dictionary<ValidationStatus, int> StatusDistribution { get; set; }
        public int HypothesesWithValidationAttempts { get; set; }
        public int CompletedValidations { get; set; }
        public decimal OverallSuccessRate { get; set; }
        public int AverageDaysToValidation { get; set; }
        public List<string> CommonValidationMethods { get; set; }
        public int HypothesesNeedingAttention { get; set; }
    }
}
