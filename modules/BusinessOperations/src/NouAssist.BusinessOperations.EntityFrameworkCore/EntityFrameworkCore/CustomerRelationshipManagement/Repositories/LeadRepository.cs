using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    public class LeadRepository : EfCoreRepository<CRMDbContext, Lead, Guid>, ILeadRepository
    {
        public LeadRepository(IDbContextProvider<CRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<bool> ExistsByEmailAsync(
            string email, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.Leads
                .Where(l => EF.Property<string>(l.ContactInfo, "Email") == email);
                
            if (excludeId.HasValue)
            {
                query = query.Where(l => l.Id != excludeId.Value);
            }
            
            return await query.AnyAsync(cancellationToken);
        }

        public async Task<Lead> FindByEmailAsync(
            string email, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Leads
                .Where(l => EF.Property<string>(l.ContactInfo, "Email") == email)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Lead>> GetByStatusAsync(
            LeadStatus status, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Leads
                .Where(l => l.Status == status)
                .OrderBy(sorting ?? nameof(Lead.CreationTime) + " desc")
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Lead>> GetByMinimumScoreAsync(
            int minimumScore, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Leads
                .Where(l => l.Score >= minimumScore)
                .OrderBy(sorting ?? nameof(Lead.Score) + " desc")
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Lead>> GetBySourceAsync(
            LeadSource source, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Leads
                .Where(l => EF.Property<LeadSource>(l.Details, "Source") == source)
                .OrderBy(sorting ?? nameof(Lead.CreationTime) + " desc")
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Lead>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(l => l.SourceRecord);
        }
    }
}
