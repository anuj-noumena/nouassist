using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Data;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CreditScoreMonitoring.EntityFrameworkCore.Repositories
{
    public class EfCoreCreditReportRepository 
        : EfCoreRepository<ICreditScoreMonitoringDbContext, CreditReport, Guid>, 
          ICreditReportRepository
    {
        public EfCoreCreditReportRepository(
            IDbContextProvider<ICreditScoreMonitoringDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<CreditReport?> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CreditReports
                .Include(r => r.Accounts)
                .Include(r => r.Inquiries)
                .Include(r => r.Alerts)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<CreditReport?> GetLatestReportAsync(Guid userId, CreditBureau bureau)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CreditReports
                .Where(r => r.UserId == userId && r.Bureau == bureau)
                .OrderByDescending(r => r.ReportDate)
                .Include(r => r.Accounts)
                .Include(r => r.Inquiries)
                .Include(r => r.Alerts)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CreditReport>> GetUserReportsAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CreditReports
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReportDate)
                .ToListAsync();
        }

        public async Task<List<CreditReport>> GetReportHistoryAsync(
            Guid userId, 
            CreditBureau bureau, 
            int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.CreditReports
                .Where(r => r.UserId == userId && 
                           r.Bureau == bureau && 
                           r.ReportDate >= startDate)
                .OrderBy(r => r.ReportDate)
                .ToListAsync();
        }
    }
}
