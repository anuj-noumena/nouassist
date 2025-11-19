using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.AccountsPayable.Repositories
{
    public class EfCoreBillRepository 
        : EfCoreRepository<IFinancialManagementDbContext, Bill, Guid>
    {
        public EfCoreBillRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<Bill>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(b => b.Approvals)
                .Include(b => b.LineItems);
        }
    }
}
