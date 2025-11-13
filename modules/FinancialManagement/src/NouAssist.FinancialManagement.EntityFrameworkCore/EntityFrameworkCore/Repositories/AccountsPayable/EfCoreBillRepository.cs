using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsPayable
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

        public async Task<Bill> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Bills
                .Include(b => b.LineItems)
                .Include(b => b.Approvals)
                .FirstOrDefaultAsync(
                    b => b.Id == id,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<List<Bill>> GetByVendorAsync(
            Guid vendorId,
            BillStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.Where(b => b.VendorId == vendorId);

            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            return await query
                .OrderByDescending(b => b.IssueDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Bill>> GetOverdueBillsAsync(
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var today = DateTime.Today;

            return await dbSet
                .Where(b => b.DueDate < today && 
                           b.Status != BillStatus.Paid && 
                           b.Status != BillStatus.Cancelled)
                .OrderBy(b => b.DueDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<Bill> GetByBillNumberAsync(
            Guid vendorId,
            string billNumber,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(
                    b => b.VendorId == vendorId && b.BillNumber == billNumber,
                    GetCancellationToken(cancellationToken));
        }

        public override async Task<IQueryable<Bill>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(b => b.LineItems)
                .Include(b => b.Approvals);
        }
    }
}
