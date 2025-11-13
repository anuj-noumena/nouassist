using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsReceivable
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

        public async Task<Customer> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(
                    c => c.Name == name,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<bool> CustomerExistsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .AnyAsync(
                    c => c.Name == name,
                    GetCancellationToken(cancellationToken));
        }
    }
}
