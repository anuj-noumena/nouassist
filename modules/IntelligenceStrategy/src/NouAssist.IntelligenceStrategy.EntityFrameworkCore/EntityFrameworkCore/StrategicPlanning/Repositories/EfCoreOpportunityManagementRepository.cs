using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core repository implementation for Opportunity Management Record aggregate
    /// </summary>
    public class EfCoreOpportunityManagementRepository 
        : EfCoreRepository<IStrategicPlanningDbContext, OpportunityManagementRecord, Guid>, IOpportunityManagementRepository 
    {
        public EfCoreOpportunityManagementRepository(
            IDbContextProvider<IStrategicPlanningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<OpportunityManagementRecord>> GetActiveRecordsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.IsActive)
                .OrderBy(r => r.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<OpportunityManagementRecord> GetRecordWithFilterRulesAsync(
            Guid recordId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var record = await dbContext.OpportunityManagementRecords
                .Include(r => r.FilterRules)
                .FirstOrDefaultAsync(r => r.Id == recordId, cancellationToken);

            if (record == null)
            {
                throw new EntityNotFoundException(typeof(OpportunityManagementRecord), recordId);
            }

            return record;
        }

        public async Task<List<OpportunityManagementRecord>> GetRecordsByFocusLevelAsync(
            FocusLevel focusLevel,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.CurrentFocusLevel == focusLevel)
                .OrderBy(r => r.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        // Override to include details by default
        public override async Task<OpportunityManagementRecord> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetRecordWithFilterRulesAsync(id, cancellationToken);
            }

            return await base.GetAsync(id, false, cancellationToken);
        }
    }
}
