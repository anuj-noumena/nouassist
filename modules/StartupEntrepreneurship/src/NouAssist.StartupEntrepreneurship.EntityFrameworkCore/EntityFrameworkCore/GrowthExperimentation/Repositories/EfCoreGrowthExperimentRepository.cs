using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Repositories
{
    public class EfCoreGrowthExperimentRepository 
        : EfCoreRepository<GrowthExperimentationDbContext, GrowthExperiment, Guid>, IGrowthExperimentRepository
    {
        public EfCoreGrowthExperimentRepository(
            IDbContextProvider<GrowthExperimentationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<GrowthExperiment>> GetListWithVariantsAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            query = query.Include(e => e.Variants);
            
            if (includeDetails)
            {
                query = query.Include(e => e.Hypotheses);
            }
            
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<GrowthExperiment> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(e => e.Variants)
                .Include(e => e.Hypotheses)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<GrowthExperiment>> GetByStatusAsync(
            ExperimentStatus status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(e => e.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ExperimentResultData>> GetResultsForVariantAsync(
            Guid variantId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Set<ExperimentResultData>()
                .Where(r => r.VariantId == variantId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddResultDataAsync(
            ExperimentResultData resultData,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            await dbContext.Set<ExperimentResultData>().AddAsync(resultData, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateResultDataAsync(
            ExperimentResultData resultData,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            dbContext.Set<ExperimentResultData>().Update(resultData);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
