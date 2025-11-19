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
    public class EfCoreCustomerPaymentRepository
        : EfCoreRepository<IFinancialManagementDbContext, CustomerPayment, Guid>,
          ICustomerPaymentRepository
    {
        public EfCoreCustomerPaymentRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<CustomerPayment?> GetByReferenceAsync(string reference, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CustomerPayments
                .WhereIf(tenantId.HasValue, p => p.TenantId == tenantId)
                .FirstOrDefaultAsync(p => p.Reference == reference);
        }

        public async Task<List<CustomerPayment>> GetPaymentsByCustomerAsync(Guid customerId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CustomerPayments
                .Where(p => p.CustomerId == customerId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<List<CustomerPayment>> GetPaymentsBySourceAsync(Guid? tenantId, PaymentSource source)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CustomerPayments
                .WhereIf(tenantId.HasValue, p => p.TenantId == tenantId)
                .Where(p => p.PaymentSource == source)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<List<CustomerPayment>> GetPaymentsInPeriodAsync(Guid? tenantId, DateTime startDate, DateTime endDate)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CustomerPayments
                .WhereIf(tenantId.HasValue, p => p.TenantId == tenantId)
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .OrderBy(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaymentsForCustomerAsync(Guid customerId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.CustomerPayments
                .Where(p => p.CustomerId == customerId)
                .SumAsync(p => p.Amount.Amount);
        }
    }
}
