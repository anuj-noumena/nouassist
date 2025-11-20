using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Repositories;

/// <summary>
/// EF Core implementation of the PerformanceSnapshot repository
/// </summary>
public class EfCorePerformanceSnapshotRepository :
    EfCoreRepository<IPersonalProductivityIntelligenceDbContext, PerformanceSnapshot, Guid>,
    IPerformanceSnapshotRepository
{
    public EfCorePerformanceSnapshotRepository(
        IDbContextProvider<IPersonalProductivityIntelligenceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    /// <summary>
    /// Gets snapshots for a user
    /// </summary>
    public async Task<List<PerformanceSnapshot>> GetUserSnapshotsAsync(
        Guid userId,
        string snapshotType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(
            s => s.MetricValues,
            s => s.Benchmarks,
            s => s.Insights);

        query = query.Where(s => s.UserId == userId);

        if (!string.IsNullOrEmpty(snapshotType))
        {
            query = query.Where(s => s.SnapshotType == snapshotType);
        }

        if (startDate.HasValue)
        {
            query = query.Where(s => s.StartDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(s => s.EndDate <= endDate.Value);
        }

        return await query
            .OrderByDescending(s => s.EndDate)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the latest snapshot for a user
    /// </summary>
    public async Task<PerformanceSnapshot> GetLatestSnapshotAsync(
        Guid userId,
        string snapshotType = null,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(
            s => s.MetricValues,
            s => s.Benchmarks,
            s => s.Insights);

        query = query.Where(s => s.UserId == userId);

        if (!string.IsNullOrEmpty(snapshotType))
        {
            query = query.Where(s => s.SnapshotType == snapshotType);
        }

        return await query
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Overrides the GetQueryableAsync method to include details by default
    /// </summary>
    public override async Task<IQueryable<PerformanceSnapshot>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(s => s.MetricValues)
            .Include(s => s.Benchmarks)
            .Include(s => s.Insights);
    }
}
