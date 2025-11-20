using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Repositories
{
    public class EfCoreConversionFunnelDefinitionRepository 
        : EfCoreRepository<GrowthExperimentationDbContext, ConversionFunnelDefinition, Guid>, IConversionFunnelDefinitionRepository
    {
        public EfCoreConversionFunnelDefinitionRepository(
            IDbContextProvider<GrowthExperimentationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ConversionFunnelDefinition> GetWithStagesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(f => f.Stages)
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }

        public async Task<List<ConversionFunnelDefinition>> GetActiveFunnelsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(f => f.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateStageConversionRateAsync(
            Guid funnelId,
            Guid stageId,
            ConversionRatePerStage conversionRate,
            CancellationToken cancellationToken = default)
        {
            var funnel = await GetWithStagesAsync(funnelId, cancellationToken);
            
            if (funnel == null)
            {
                throw new EntityNotFoundException(typeof(ConversionFunnelDefinition), funnelId);
            }
            
            var stage = funnel.Stages.FirstOrDefault(s => s.Id == stageId);
            
            if (stage == null)
            {
                throw new EntityNotFoundException(typeof(FunnelStage), stageId);
            }
            
            stage.UpdateConversionRate(conversionRate);
            
            await UpdateAsync(funnel, cancellationToken: cancellationToken);
        }

        public async Task<FunnelStage> GetStageAsync(
            Guid funnelId,
            Guid stageId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Set<FunnelStage>()
                .Where(s => s.FunnelId == funnelId && s.Id == stageId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
