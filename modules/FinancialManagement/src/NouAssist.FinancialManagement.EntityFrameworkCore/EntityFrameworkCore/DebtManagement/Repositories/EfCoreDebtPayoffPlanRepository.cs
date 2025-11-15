using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.DebtManagement.Aggregates;
using NouAssist.DebtManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.DebtManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreDebtPayoffPlanRepository 
        : EfCoreRepository<IDebtManagementDbContext, DebtPayoffPlan, Guid>, 
          IDebtPayoffPlanRepository
    {
        public EfCoreDebtPayoffPlanRepository(
            IDbContextProvider<IDebtManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<DebtPayoffPlan?> GetWithDebtsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DebtPayoffPlans
                .Include(p => p.Debts)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<DebtPayoffPlan>> GetActiveUserPlansAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DebtPayoffPlans
                .Where(p => p.UserId == userId && p.IsActive)
                .Include(p => p.Debts)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync();
        }
    }
}
