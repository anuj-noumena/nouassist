using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsPayable.Data
{
    public interface IVendorRepository : IRepository<Vendor, Guid>
    {
        Task<Vendor?> GetByNameAsync(string vendorName, Guid? tenantId);
        Task<Vendor?> GetByTaxIdAsync(string taxId, Guid? tenantId);
        Task<List<Vendor>> GetActiveVendorsAsync(Guid? tenantId);
        Task<List<Vendor>> SearchVendorsAsync(string searchTerm, Guid? tenantId);
        Task<bool> IsVendorNameUniqueAsync(string vendorName, Guid? tenantId, Guid? excludeId = null);
    }
}
