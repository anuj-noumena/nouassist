using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories
{
    /// <summary>
    /// Repository for PerformanceSnapshot aggregate
    /// </summary>
    public interface IPerformanceSnapshotRepository : IRepository<PerformanceSnapshot, Guid>
    {
        /// <summary>
        /// Gets snapshots for a user
        /// </summary>
        Task<List<PerformanceSnapshot>> GetUserSnapshotsAsync(
            Guid userId,
            string snapshotType = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets the latest snapshot for a user
        /// </summary>
        Task<PerformanceSnapshot> GetLatestSnapshotAsync(
            Guid userId,
            string snapshotType = null,
            CancellationToken cancellationToken = default);
    }
}
