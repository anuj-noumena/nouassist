using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.AccountsPayable.Repositories
{
    public class EfCoreBillRepository
        : EfCoreRepository<IFinancialManagementDbContext, Bill, Guid>,
          IBillRepository
    {
        public EfCoreBillRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<Bill>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(b => b.Approvals)
                .Include(b => b.LineItems);
        }

        public async Task<Bill?> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Bills
                .Include(b => b.Approvals)
                .Include(b => b.LineItems)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Bill>> GetBillsByVendorAsync(Guid vendorId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Bills
                .Where(b => b.VendorId == vendorId)
                .Include(b => b.LineItems)
                .OrderByDescending(b => b.CreationTime)
                .ToListAsync();
        }

        public async Task<List<Bill>> GetBillsByStatusAsync(Guid? tenantId, BillStatus status)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Bills
                .WhereIf(tenantId.HasValue, b => b.TenantId == tenantId)
                .Where(b => b.Status == status)
                .Include(b => b.LineItems)
                .OrderBy(b => b.DueDate)
                .ToListAsync();
        }

        public async Task<List<Bill>> GetOverdueBillsAsync(Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            var today = DateTime.UtcNow;
            return await dbContext.Bills
                .WhereIf(tenantId.HasValue, b => b.TenantId == tenantId)
                .Where(b => b.DueDate < today && b.Status != BillStatus.Paid)
                .Include(b => b.LineItems)
                .OrderBy(b => b.DueDate)
                .ToListAsync();
        }

        public async Task<List<Bill>> GetBillsDueInPeriodAsync(Guid? tenantId, DateTime startDate, DateTime endDate)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Bills
                .WhereIf(tenantId.HasValue, b => b.TenantId == tenantId)
                .Where(b => b.DueDate >= startDate && b.DueDate <= endDate)
                .Include(b => b.LineItems)
                .OrderBy(b => b.DueDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalOutstandingAsync(Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Bills
                .WhereIf(tenantId.HasValue, b => b.TenantId == tenantId)
                .Where(b => b.Status != BillStatus.Paid)
                .SumAsync(b => b.TotalAmount.Amount);
        }
    }
}
