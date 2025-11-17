using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.TaxManagement.Data
{
    public interface ITaxProfileRepository : IRepository<TaxProfile, Guid>
    {
        Task<TaxProfile?> GetByUserIdAsync(Guid userId);
    }
}
