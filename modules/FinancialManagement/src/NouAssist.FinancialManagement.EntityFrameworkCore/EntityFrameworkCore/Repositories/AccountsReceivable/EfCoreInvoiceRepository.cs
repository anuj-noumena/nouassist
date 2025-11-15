using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.AccountsReceivable
{
    public class EfCoreInvoiceRepository 
        : EfCoreRepository<IFinancialManagementDbContext, Invoice, Guid>
    {
        public EfCoreInvoiceRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<Invoice>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(i => i.LineItems);
        }
    }
}
