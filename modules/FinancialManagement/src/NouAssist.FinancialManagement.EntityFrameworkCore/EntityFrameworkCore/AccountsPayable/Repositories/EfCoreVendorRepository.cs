using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.AccountsPayable.Repositories
{
    public class EfCoreVendorRepository
        : EfCoreRepository<IFinancialManagementDbContext, Vendor, Guid>,
          IVendorRepository
    {
        public EfCoreVendorRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Vendor?> GetByNameAsync(string vendorName, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Vendors
                .WhereIf(tenantId.HasValue, v => v.TenantId == tenantId)
                .FirstOrDefaultAsync(v => v.VendorName == vendorName);
        }

        public async Task<Vendor?> GetByTaxIdAsync(string taxId, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Vendors
                .WhereIf(tenantId.HasValue, v => v.TenantId == tenantId)
                .FirstOrDefaultAsync(v => v.TaxId.Value == taxId);
        }

        public async Task<List<Vendor>> GetActiveVendorsAsync(Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Vendors
                .WhereIf(tenantId.HasValue, v => v.TenantId == tenantId)
                .Where(v => v.IsActive)
                .OrderBy(v => v.VendorName)
                .ToListAsync();
        }

        public async Task<List<Vendor>> SearchVendorsAsync(string searchTerm, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            var lowerSearchTerm = searchTerm.ToLower();
            return await dbContext.Vendors
                .WhereIf(tenantId.HasValue, v => v.TenantId == tenantId)
                .Where(v => v.VendorName.ToLower().Contains(lowerSearchTerm) ||
                           (v.ContactEmail != null && v.ContactEmail.ToLower().Contains(lowerSearchTerm)))
                .OrderBy(v => v.VendorName)
                .ToListAsync();
        }

        public async Task<bool> IsVendorNameUniqueAsync(string vendorName, Guid? tenantId, Guid? excludeId = null)
        {
            var dbContext = await GetDbContextAsync();
            return !await dbContext.Vendors
                .WhereIf(tenantId.HasValue, v => v.TenantId == tenantId)
                .WhereIf(excludeId.HasValue, v => v.Id != excludeId.Value)
                .AnyAsync(v => v.VendorName == vendorName);
        }
    }
}
