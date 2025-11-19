using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Data;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.CreditScoreMonitoring.Repositories
{
    public class EfCoreCreditDisputeRepository 
        : EfCoreRepository<IFinancialManagementDbContext, CreditDispute, Guid>, 
          ICreditDisputeRepository
    {
        public EfCoreCreditDisputeRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CreditDispute>> GetUserDisputesAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CreditDisputes
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.SubmittedDate)
                .ToListAsync();
        }

        public async Task<List<CreditDispute>> GetActiveDisputesAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CreditDisputes
                .Where(d => d.UserId == userId && 
                           (d.Status == DisputeStatus.Submitted || 
                            d.Status == DisputeStatus.UnderInvestigation))
                .OrderBy(d => d.SubmittedDate)
                .ToListAsync();
        }

        public async Task<List<CreditDispute>> GetDisputesByStatusAsync(
            Guid userId, 
            DisputeStatus status)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CreditDisputes
                .Where(d => d.UserId == userId && d.Status == status)
                .OrderByDescending(d => d.SubmittedDate)
                .ToListAsync();
        }
    }
}
