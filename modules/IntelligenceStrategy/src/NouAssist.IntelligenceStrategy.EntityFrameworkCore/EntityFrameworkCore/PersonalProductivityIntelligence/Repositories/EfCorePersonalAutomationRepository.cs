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
/// EF Core implementation of the PersonalAutomation repository
/// </summary>
public class EfCorePersonalAutomationRepository :
    EfCoreRepository<IPersonalProductivityIntelligenceDbContext, PersonalAutomation, Guid>,
    IPersonalAutomationRepository
{
    public EfCorePersonalAutomationRepository(
        IDbContextProvider<IPersonalProductivityIntelligenceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    /// <summary>
    /// Gets automations for a user
    /// </summary>
    public async Task<List<PersonalAutomation>> GetUserAutomationsAsync(
        Guid userId,
        bool? isEnabled = null,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(a => a.Triggers, a => a.Actions);

        query = query.Where(a => a.UserId == userId);

        if (isEnabled.HasValue)
        {
            query = query.Where(a => a.IsEnabled == isEnabled.Value);
        }

        return await query
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets automations with a specific trigger type
    /// </summary>
    public async Task<List<PersonalAutomation>> GetAutomationsWithTriggerTypeAsync(
        Guid userId,
        AutomationTriggerType triggerType,
        CancellationToken cancellationToken = default)
    {
        var query = await WithDetailsAsync(a => a.Triggers, a => a.Actions);

        return await query
            .Where(a => a.UserId == userId)
            .Where(a => a.IsEnabled)
            .Where(a => a.Triggers.Any(t => t.TriggerType == triggerType))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Overrides the GetQueryableAsync method to include details by default
    /// </summary>
    public override async Task<IQueryable<PersonalAutomation>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(a => a.Triggers)
            .Include(a => a.Actions);
    }
}
