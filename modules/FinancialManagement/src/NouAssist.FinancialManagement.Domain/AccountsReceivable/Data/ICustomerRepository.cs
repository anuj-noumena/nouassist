using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable.Data
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer?> GetWithDetailsAsync(Guid id);
        Task<Customer?> GetByEmailAsync(string email, Guid? tenantId);
        Task<List<Customer>> GetCustomersByTypeAsync(Guid? tenantId, CustomerType type);
        Task<List<Customer>> GetActiveCustomersAsync(Guid? tenantId);
        Task<List<Customer>> SearchCustomersAsync(string searchTerm, Guid? tenantId);
        Task<bool> IsEmailUniqueAsync(string email, Guid? tenantId, Guid? excludeId = null);
    }
}
