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
    public class EfCoreLegalAgreementRepository 
        : EfCoreRepository<ILegalComplianceDbContext, LegalAgreement, Guid>, 
          ILegalAgreementRepository
    {
        public EfCoreLegalAgreementRepository(
            IDbContextProvider<ILegalComplianceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<LegalAgreement>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(x => x.Terms)
                .Include(x => x.Reminders);
        }

        public async Task<List<LegalAgreement>> GetExpiringAgreementsAsync(
            int daysThreshold, 
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;
            var threshold = now.AddDays(daysThreshold);
            
            var query = await WithDetailsAsync();
            
            return await query
                .Where(a => 
                    (EF.Property<string>(a, "FilingStatus") == FilingStatusType.Approved.ToString() || 
                     EF.Property<string>(a, "FilingStatus") == FilingStatusType.Renewed.ToString()) &&
                    a.ExpirationDate.HasValue &&
                    a.ExpirationDate.Value > now &&
                    a.ExpirationDate.Value <= threshold)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LegalAgreement>> GetByStatusAsync(
            FilingStatusType status, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(a => EF.Property<string>(a, "FilingStatus") == status.ToString())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LegalAgreement>> GetByTypeAsync(
            LegalAgreementType type,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(a => a.Type == type)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LegalAgreement>> GetByCounterpartyAsync(
            string counterpartyName,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(a => a.CounterpartyName.Contains(counterpartyName))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LegalAgreement>> GetActiveAgreementsAsync(
            DateTime referenceDate,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(a => 
                    (EF.Property<string>(a, "FilingStatus") == FilingStatusType.Approved.ToString() || 
                     EF.Property<string>(a, "FilingStatus") == FilingStatusType.Renewed.ToString()) &&
                    a.EffectiveDate <= referenceDate &&
                    (!a.ExpirationDate.HasValue || a.ExpirationDate.Value > referenceDate))
                .ToListAsync(cancellationToken);
        }
    }
}
