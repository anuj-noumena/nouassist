using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TaxManagement.Aggregates;
using NouAssist.TaxManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TaxManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreTaxProfileRepository 
        : EfCoreRepository<ITaxManagementDbContext, TaxProfile, Guid>, 
          ITaxProfileRepository
    {
        public EfCoreTaxProfileRepository(
            IDbContextProvider<ITaxManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaxProfile?> GetByUserIdAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.TaxProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}
