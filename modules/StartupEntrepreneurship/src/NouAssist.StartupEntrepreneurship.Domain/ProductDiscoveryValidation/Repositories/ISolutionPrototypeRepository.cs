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
    /// Repository interface for SolutionPrototype aggregate
    /// </summary>
    public interface ISolutionPrototypeRepository : IRepository<SolutionPrototype, Guid>
    {
        /// <summary>
        /// Finds prototypes matching the given specification
        /// </summary>
        Task<List<SolutionPrototype>> GetListAsync(
            Expression<Func<SolutionPrototype, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds prototypes by type
        /// </summary>
        Task<List<SolutionPrototype>> GetByTypeAsync(
            PrototypeType type,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds prototypes related to a hypothesis
        /// </summary>
        Task<List<SolutionPrototype>> GetByHypothesisAsync(
            Guid hypothesisId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets prototype with all versions
        /// </summary>
        Task<SolutionPrototype> GetWithVersionsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds test-ready prototypes
        /// </summary>
        Task<List<SolutionPrototype>> GetTestReadyAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds prototypes with multiple iterations
        /// </summary>
        Task<List<SolutionPrototype>> GetIteratedPrototypesAsync(
            int minimumVersions = 2,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds stale prototypes needing updates
        /// </summary>
        Task<List<SolutionPrototype>> GetStalePrototypesAsync(
            int daysSinceLastUpdate = 60,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets prototype statistics
        /// </summary>
        Task<PrototypeStatistics> GetStatisticsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches prototypes by name or description
        /// </summary>
        Task<List<SolutionPrototype>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets prototypes ready for user testing
        /// </summary>
        Task<List<SolutionPrototype>> GetReadyForTestingAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts prototypes by type
        /// </summary>
        Task<Dictionary<PrototypeType, int>> GetTypeDistributionAsync(
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Data transfer object for prototype statistics
    /// </summary>
    public class PrototypeStatistics
    {
        public int TotalPrototypes { get; set; }
        public Dictionary<PrototypeType, int> PrototypesByType { get; set; }
        public int PrototypesWithMultipleVersions { get; set; }
        public int PrototypesReadyForTesting { get; set; }
        public int StalePrototypes { get; set; }
        public decimal AverageVersionsPerPrototype { get; set; }
        public Dictionary<string, int> PrototypesByHypothesis { get; set; }
    }
}
