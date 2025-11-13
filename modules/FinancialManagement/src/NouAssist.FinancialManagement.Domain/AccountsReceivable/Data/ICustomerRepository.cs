using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable.Data
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<bool> CustomerExistsAsync(
            string name,
            CancellationToken cancellationToken = default);
    }
}
