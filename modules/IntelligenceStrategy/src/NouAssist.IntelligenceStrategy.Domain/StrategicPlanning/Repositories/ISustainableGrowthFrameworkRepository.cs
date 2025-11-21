using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories
{
    /// <summary>
    /// Repository interface for Sustainable Growth Framework aggregate
    /// </summary>
    public interface ISustainableGrowthFrameworkRepository : IRepository<SustainableGrowthFramework, Guid>
    {
        Task<List<SustainableGrowthFramework>> GetActiveFrameworksAsync(
            CancellationToken cancellationToken = default);

        Task<SustainableGrowthFramework> GetFrameworkWithEsgGoalsAsync(
            Guid frameworkId,
            CancellationToken cancellationToken = default);
    }
}
