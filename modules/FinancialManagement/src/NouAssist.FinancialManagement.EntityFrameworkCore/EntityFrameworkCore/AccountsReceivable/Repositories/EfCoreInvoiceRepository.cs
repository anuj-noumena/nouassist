using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.AccountsReceivable.Repositories
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

        public override async Task<IQueryable<Invoice>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(i => i.LineItems);
        }

        public async Task<Invoice?> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Invoices
                .Include(i => i.LineItems)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Invoices
                .WhereIf(tenantId.HasValue, i => i.TenantId == tenantId)
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<List<Invoice>> GetInvoicesByCustomerAsync(Guid customerId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Invoices
                .Where(i => i.CustomerId == customerId)
                .Include(i => i.LineItems)
                .OrderByDescending(i => i.CreationTime)
                .ToListAsync();
        }

        public async Task<List<Invoice>> GetInvoicesByStatusAsync(Guid? tenantId, InvoiceStatus status)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Invoices
                .WhereIf(tenantId.HasValue, i => i.TenantId == tenantId)
                .Where(i => i.Status == status)
                .Include(i => i.LineItems)
                .OrderBy(i => i.DueDate)
                .ToListAsync();
        }

        public async Task<List<Invoice>> GetOverdueInvoicesAsync(Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            var today = DateTime.UtcNow;
            return await dbContext.Invoices
                .WhereIf(tenantId.HasValue, i => i.TenantId == tenantId)
                .Where(i => i.DueDate < today && (i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue))
                .Include(i => i.LineItems)
                .OrderBy(i => i.DueDate)
                .ToListAsync();
        }

        public async Task<List<Invoice>> GetInvoicesDueInPeriodAsync(Guid? tenantId, DateTime startDate, DateTime endDate)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Invoices
                .WhereIf(tenantId.HasValue, i => i.TenantId == tenantId)
                .Where(i => i.DueDate >= startDate && i.DueDate <= endDate)
                .Include(i => i.LineItems)
                .OrderBy(i => i.DueDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalReceivablesAsync(Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Invoices
                .WhereIf(tenantId.HasValue, i => i.TenantId == tenantId)
                .Where(i => i.Status != InvoiceStatus.Paid)
                .SumAsync(i => i.TotalAmount.Amount);
        }

        public async Task<string> GetNextInvoiceNumberAsync(Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            var maxNumber = await dbContext.Invoices
                .WhereIf(tenantId.HasValue, i => i.TenantId == tenantId)
                .MaxAsync(i => (string?)i.InvoiceNumber);

            if (string.IsNullOrEmpty(maxNumber))
            {
                return "INV-00001";
            }

            // Extract number from format like "INV-00123"
            var parts = maxNumber.Split('-');
            if (parts.Length == 2 && int.TryParse(parts[1], out var number))
            {
                return $"INV-{(number + 1):D5}";
            }

            return "INV-00001";
        }
    }
}
