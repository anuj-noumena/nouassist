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
    public class EfCoreComplianceObligationRepository 
        : EfCoreRepository<ILegalComplianceDbContext, ComplianceObligation, Guid>, 
          IComplianceObligationRepository
    {
        public EfCoreComplianceObligationRepository(
            IDbContextProvider<ILegalComplianceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ComplianceObligation>> GetWithApproachingDeadlinesAsync(
            int daysThreshold, 
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;
            var threshold = now.AddDays(daysThreshold);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(o => 
                    EF.Property<string>(o, "ComplianceStatus") != ComplianceStatus.Completed.ToString() &&
                    EF.Property<string>(o, "ComplianceStatus") != ComplianceStatus.Waived.ToString() &&
                    EF.Property<string>(o, "ComplianceStatus") != ComplianceStatus.NotApplicable.ToString() &&
                    o.Deadline.HasValue &&
                    o.Deadline.Value > now &&
                    o.Deadline.Value <= threshold)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ComplianceObligation>> GetByStatusAsync(
            ComplianceStatus status, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(o => EF.Property<string>(o, "ComplianceStatus") == status.ToString())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ComplianceObligation>> GetByJurisdictionAsync(
            string country,
            string state = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (state != null)
            {
                return await query
                    .Where(o => 
                        EF.Property<string>(o, "JurisdictionCountry") == country &&
                        EF.Property<string>(o, "JurisdictionState") == state)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                return await query
                    .Where(o => EF.Property<string>(o, "JurisdictionCountry") == country)
                    .ToListAsync(cancellationToken);
            }
        }

        public async Task<List<ComplianceObligation>> GetOverdueObligationsAsync(
            DateTime referenceDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(o => 
                    EF.Property<string>(o, "ComplianceStatus") != ComplianceStatus.Completed.ToString() &&
                    EF.Property<string>(o, "ComplianceStatus") != ComplianceStatus.Waived.ToString() &&
                    EF.Property<string>(o, "ComplianceStatus") != ComplianceStatus.NotApplicable.ToString() &&
                    o.Deadline.HasValue &&
                    o.Deadline.Value < referenceDate)
                .ToListAsync(cancellationToken);
        }
    }
}
