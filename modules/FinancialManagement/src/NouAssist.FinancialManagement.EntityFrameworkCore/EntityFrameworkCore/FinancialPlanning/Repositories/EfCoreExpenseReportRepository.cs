using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Repositories
{
    public class EfCoreExpenseReportRepository
        : EfCoreRepository<IFinancialManagementDbContext, ExpenseReport, Guid>,
          IExpenseReportRepository
    {
        public EfCoreExpenseReportRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ExpenseReport?> GetWithExpensesAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ExpenseReports
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ExpenseReport>> GetReportsBySubmitterAsync(Guid submitterId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ExpenseReports
                .Where(r => r.SubmitterId == submitterId)
                .OrderByDescending(r => r.CreationTime)
                .ToListAsync();
        }

        public async Task<List<ExpenseReport>> GetReportsByStatusAsync(Guid? tenantId, ReportStatus status)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ExpenseReports
                .WhereIf(tenantId.HasValue, r => r.TenantId == tenantId)
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreationTime)
                .ToListAsync();
        }

        public async Task<List<ExpenseReport>> GetPendingApprovalsAsync(Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ExpenseReports
                .WhereIf(tenantId.HasValue, r => r.TenantId == tenantId)
                .Where(r => r.Status == ReportStatus.Submitted)
                .OrderBy(r => r.SubmittedDate)
                .ToListAsync();
        }

        public async Task<List<ExpenseReport>> GetReportsInPeriodAsync(Guid? tenantId, DateTime startDate, DateTime endDate)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ExpenseReports
                .WhereIf(tenantId.HasValue, r => r.TenantId == tenantId)
                .Where(r => r.PeriodStart >= startDate && r.PeriodEnd <= endDate)
                .OrderBy(r => r.PeriodStart)
                .ToListAsync();
        }
    }
}
