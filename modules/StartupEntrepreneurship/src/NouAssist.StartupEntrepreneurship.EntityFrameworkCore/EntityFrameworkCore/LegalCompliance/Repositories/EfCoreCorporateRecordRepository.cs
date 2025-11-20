using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Repositories
{
    public class EfCoreCorporateRecordRepository 
        : EfCoreRepository<ILegalComplianceDbContext, CorporateRecord, Guid>, 
          ICorporateRecordRepository
    {
        public EfCoreCorporateRecordRepository(
            IDbContextProvider<ILegalComplianceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CorporateRecord>> GetByTypeAsync(
            CorporateRecordType type,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.Type == type)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CorporateRecord>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.RecordDate >= startDate && r.RecordDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CorporateRecord>> GetByStatusAsync(
            FilingStatusType status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => EF.Property<string>(r, "FilingStatus") == status.ToString())
                .ToListAsync(cancellationToken);
        }
    }
}
