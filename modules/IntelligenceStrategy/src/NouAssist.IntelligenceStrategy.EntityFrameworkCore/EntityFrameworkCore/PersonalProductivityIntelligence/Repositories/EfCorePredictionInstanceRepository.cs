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
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Repositories;

/// <summary>
/// EF Core implementation of the PredictionInstance repository
/// </summary>
public class EfCorePredictionInstanceRepository :
    EfCoreRepository<IPersonalProductivityIntelligenceDbContext, PredictionInstance, Guid>,
    IPredictionInstanceRepository
{
    public EfCorePredictionInstanceRepository(
        IDbContextProvider<IPersonalProductivityIntelligenceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    /// <summary>
    /// Gets predictions for a user
    /// </summary>
    public async Task<List<PredictionInstance>> GetUserPredictionsAsync(
        Guid userId,
        string predictionType = null,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(p => p.ExplainabilityLogs);

        query = query.Where(p => p.UserId == userId);

        if (!string.IsNullOrEmpty(predictionType))
        {
            query = query.Where(p => p.PredictionType == predictionType);
        }

        return await query
            .OrderByDescending(p => p.PredictionTime)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets predictions for a specific entity
    /// </summary>
    public async Task<List<PredictionInstance>> GetPredictionsForEntityAsync(
        string entityType,
        Guid entityId,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(p => p.ExplainabilityLogs);

        return await query
            .Where(p => p.TargetEntityType == entityType && p.TargetEntityId == entityId)
            .OrderByDescending(p => p.PredictionTime)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets predictions matching a specification
    /// </summary>
    public async Task<List<PredictionInstance>> GetListAsync(
        Specification<PredictionInstance> specification,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(p => p.ExplainabilityLogs);

        return await query
            .Where(specification.ToExpression())
            .OrderByDescending(p => p.PredictionTime)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Overrides the GetQueryableAsync method to include details by default
    /// </summary>
    public override async Task<IQueryable<PredictionInstance>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.ExplainabilityLogs);
    }
}
