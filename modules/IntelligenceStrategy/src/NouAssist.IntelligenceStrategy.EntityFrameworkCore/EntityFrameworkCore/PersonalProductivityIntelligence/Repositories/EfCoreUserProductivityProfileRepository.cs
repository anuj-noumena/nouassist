using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Repositories;

/// <summary>
/// EF Core implementation of the UserProductivityProfile repository
/// </summary>
public class EfCoreUserProductivityProfileRepository :
    EfCoreRepository<IPersonalProductivityIntelligenceDbContext, UserProductivityProfile, Guid>,
    IUserProductivityProfileRepository
{
    public EfCoreUserProductivityProfileRepository(
        IDbContextProvider<IPersonalProductivityIntelligenceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    /// <summary>
    /// Finds a profile by user ID
    /// </summary>
    public async Task<UserProductivityProfile> FindByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // Get queryable with all navigation properties (e.g., WorkPattern)
        var query = await WithDetailsAsync(
            p => p.WorkPattern,
            p => p.PreferenceSettings,
            p => p.PerformanceData,
            p => p.TaskCompletionRecords,
            p => p.ExternalContextRefs);

        return await query
            .Where(p => p.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Gets profiles with high cognitive load
    /// </summary>
    public async Task<List<UserProductivityProfile>> GetHighCognitiveLoadProfilesAsync(
        CognitiveLoadLevel minLoadLevel,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query
            .Where(p => p.CognitiveLoadLevel >= minLoadLevel)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets profiles with work patterns updated before a specific date
    /// </summary>
    public async Task<List<UserProductivityProfile>> GetProfilesWithOutdatedWorkPatternsAsync(
        DateTime thresholdDate,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(p => p.WorkPattern);

        return await query
            .Where(p => p.WorkPattern == null || p.WorkPattern.CreatedAt < thresholdDate)
            .OrderBy(p => p.WorkPattern != null ? p.WorkPattern.CreatedAt : DateTime.MinValue)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Overrides the GetQueryableAsync method to include details by default
    /// </summary>
    public override async Task<IQueryable<UserProductivityProfile>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.WorkPattern);
    }
}
