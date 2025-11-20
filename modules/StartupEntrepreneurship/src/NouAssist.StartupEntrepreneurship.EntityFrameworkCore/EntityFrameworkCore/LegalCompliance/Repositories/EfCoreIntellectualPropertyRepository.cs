using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Entities;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Repositories
{
    public class EfCoreIntellectualPropertyRepository 
        : EfCoreRepository<ILegalComplianceDbContext, IntellectualPropertyAsset, Guid>, 
          IIntellectualPropertyRepository
    {
        public EfCoreIntellectualPropertyRepository(
            IDbContextProvider<ILegalComplianceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<IntellectualPropertyAsset>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(x => x.FilingDeadlines);
        }

        public async Task<bool> ExistsByIdentifierAsync(
            string identifier, 
            IntellectualPropertyType type, 
            Guid? exceptId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            var exists = await query
                .Where(ip => 
                    EF.Property<string>(ip, "Identifier") == identifier && 
                    EF.Property<string>(ip, "IdentifierType") == type.ToString() &&
                    (exceptId == null || ip.Id != exceptId))
                .AnyAsync(cancellationToken);
                
            return exists;
        }

        public async Task<List<IntellectualPropertyAsset>> GetWithApproachingDeadlinesAsync(
            int daysThreshold, 
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;
            var threshold = now.AddDays(daysThreshold);
            
            var query = await WithDetailsAsync();
            
            return await query
                .Where(ip => ip.FilingDeadlines.Any(fd => 
                    !fd.IsCompleted && 
                    fd.Deadline > now && 
                    fd.Deadline <= threshold))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<IntellectualPropertyAsset>> GetByStatusAsync(
            FilingStatusType status, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(ip => EF.Property<string>(ip, "FilingStatus") == status.ToString())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<IntellectualPropertyAsset>> GetByTypeAsync(
            IntellectualPropertyType type,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(ip => ip.Type == type)
                .ToListAsync(cancellationToken);
        }
    }
}
