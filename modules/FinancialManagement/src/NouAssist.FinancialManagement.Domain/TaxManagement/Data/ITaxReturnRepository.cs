using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.TaxManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.TaxManagement.Data
{
    public interface ITaxReturnRepository : IRepository<TaxReturn, Guid>
    {
        Task<TaxReturn?> GetWithDetailsAsync(Guid id);
        Task<TaxReturn?> GetByUserAndYearAsync(Guid userId, int taxYear);
        Task<List<TaxReturn>> GetUserTaxReturnsAsync(Guid userId);
        Task<bool> HasReturnForYearAsync(Guid userId, int taxYear);
    }
}
