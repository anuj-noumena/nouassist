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
    public class EfCoreCustomerRepository
        : EfCoreRepository<IFinancialManagementDbContext, Customer, Guid>,
          ICustomerRepository
    {
        public EfCoreCustomerRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Customer?> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByEmailAsync(string email, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Customers
                .WhereIf(tenantId.HasValue, c => c.TenantId == tenantId)
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<List<Customer>> GetCustomersByTypeAsync(Guid? tenantId, CustomerType type)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Customers
                .WhereIf(tenantId.HasValue, c => c.TenantId == tenantId)
                .Where(c => c.CustomerType == type)
                .OrderBy(c => c.CustomerName)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetActiveCustomersAsync(Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Customers
                .WhereIf(tenantId.HasValue, c => c.TenantId == tenantId)
                .Where(c => c.IsActive)
                .OrderBy(c => c.CustomerName)
                .ToListAsync();
        }

        public async Task<List<Customer>> SearchCustomersAsync(string searchTerm, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            var lowerSearchTerm = searchTerm.ToLower();
            return await dbContext.Customers
                .WhereIf(tenantId.HasValue, c => c.TenantId == tenantId)
                .Where(c => c.CustomerName.ToLower().Contains(lowerSearchTerm) ||
                           (c.Email != null && c.Email.ToLower().Contains(lowerSearchTerm)))
                .OrderBy(c => c.CustomerName)
                .ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email, Guid? tenantId, Guid? excludeId = null)
        {
            var dbContext = await GetDbContextAsync();
            return !await dbContext.Customers
                .WhereIf(tenantId.HasValue, c => c.TenantId == tenantId)
                .WhereIf(excludeId.HasValue, c => c.Id != excludeId.Value)
                .AnyAsync(c => c.Email == email);
        }
    }
}
