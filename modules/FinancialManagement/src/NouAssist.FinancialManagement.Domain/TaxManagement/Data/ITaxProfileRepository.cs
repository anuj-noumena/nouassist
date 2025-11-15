using System;
using System.Threading.Tasks;
using NouAssist.TaxManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.TaxManagement.Data
{
    public interface ITaxProfileRepository : IRepository<TaxProfile, Guid>
    {
        Task<TaxProfile?> GetByUserIdAsync(Guid userId);
    }
}
