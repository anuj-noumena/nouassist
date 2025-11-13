using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsPayable
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

        public async Task<Vendor> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(
                    v => v.Name == name,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<bool> VendorExistsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .AnyAsync(
                    v => v.Name == name,
                    GetCancellationToken(cancellationToken));
        }
    }
}
