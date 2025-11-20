using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Repositories;

/// <summary>
/// EF Core implementation of the RecommendationInstance repository
/// </summary>
public class EfCoreRecommendationInstanceRepository :
    EfCoreRepository<IPersonalProductivityIntelligenceDbContext, RecommendationInstance, Guid>,
    IRecommendationInstanceRepository
{
    public EfCoreRecommendationInstanceRepository(
        IDbContextProvider<IPersonalProductivityIntelligenceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    /// <summary>
    /// Gets active recommendations for a user
    /// </summary>
    public async Task<List<RecommendationInstance>> GetActiveRecommendationsAsync(
        Guid userId,
        RecommendationType? recommendationType = null,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(r => r.Rules, r => r.ExplainabilityLogs);

        query = query
            .Where(r => r.UserId == userId)
            .Where(r => !r.IsActedOn && !r.IsDismissed)
            .Where(r => !r.ExpiresAt.HasValue || r.ExpiresAt.Value > DateTime.UtcNow);

        if (recommendationType.HasValue)
        {
            query = query.Where(r => r.RecommendationType == recommendationType.Value);
        }

        return await query
            .OrderByDescending(r => r.Priority.Value)
            .ThenByDescending(r => r.GeneratedAt)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets recommendations matching a specification
    /// </summary>
    public async Task<List<RecommendationInstance>> GetListAsync(
        Specification<RecommendationInstance> specification,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(r => r.Rules, r => r.ExplainabilityLogs);

        return await query
            .Where(specification.ToExpression())
            .OrderByDescending(r => r.Priority.Value)
            .ThenByDescending(r => r.GeneratedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets recommendations requiring user attention
    /// </summary>
    public async Task<List<RecommendationInstance>> GetAttentionNeededRecommendationsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var specification = new AttentionNeededRecommendationSpecification();
        var query = await WithDetailsAsync(r => r.Rules, r => r.ExplainabilityLogs);

        return await query
            .Where(r => r.UserId == userId)
            .Where(specification.ToExpression())
            .OrderByDescending(r => r.Priority.Value)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Overrides the GetQueryableAsync method to include details by default
    /// </summary>
    public override async Task<IQueryable<RecommendationInstance>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(r => r.Rules)
            .Include(r => r.ExplainabilityLogs);
    }
}
