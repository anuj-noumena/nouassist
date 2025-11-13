using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsPayable.Data
{
    public interface IVendorRepository : IRepository<Vendor, Guid>
    {
        Task<Vendor> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<bool> VendorExistsAsync(
            string name,
            CancellationToken cancellationToken = default);
    }
}
