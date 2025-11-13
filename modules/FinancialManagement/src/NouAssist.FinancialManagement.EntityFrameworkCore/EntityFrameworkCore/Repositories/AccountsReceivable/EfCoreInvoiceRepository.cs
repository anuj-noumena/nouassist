using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsReceivable
{
    public class EfCoreInvoiceRepository 
        : EfCoreRepository<IFinancialManagementDbContext, Invoice, Guid>,
          IInvoiceRepository
    {
        public EfCoreInvoiceRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Invoice> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Invoices
                .Include(i => i.LineItems)
                .Include(i => i.PaymentApplications)
                .FirstOrDefaultAsync(
                    i => i.Id == id,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<List<Invoice>> GetByCustomerAsync(
            Guid customerId,
            InvoiceStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.Where(i => i.CustomerId == customerId);

            if (status.HasValue)
            {
                query = query.Where(i => i.Status == status.Value);
            }

            return await query
                .OrderByDescending(i => i.IssueDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Invoice>> GetOverdueInvoicesAsync(
            int? daysOverdue = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var today = DateTime.Today;

            var query = dbSet.Where(i => 
                i.DueDate < today && 
                (i.Status == InvoiceStatus.Sent || 
                 i.Status == InvoiceStatus.PartiallyPaid ||
                 i.Status == InvoiceStatus.Overdue));

            if (daysOverdue.HasValue)
            {
                var cutoffDate = today.AddDays(-daysOverdue.Value);
                query = query.Where(i => i.DueDate <= cutoffDate);
            }

            return await query
                .OrderBy(i => i.DueDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<IQueryable<Invoice>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(i => i.LineItems)
                .Include(i => i.PaymentApplications);
        }
    }
}
