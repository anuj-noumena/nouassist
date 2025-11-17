using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.DebtManagement.Enums;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.DebtManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreDebtAccountRepository 
        : EfCoreRepository<IDebtManagementDbContext, DebtAccount, Guid>, 
          IDebtAccountRepository
    {
        public EfCoreDebtAccountRepository(
            IDbContextProvider<IDebtManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<DebtAccount?> GetWithPaymentsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DebtAccounts
                .Include(d => d.Payments)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<DebtAccount>> GetActiveDebtsByUserAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DebtAccounts
                .Where(d => d.UserId == userId && d.Status == DebtStatus.Active)
                .Include(d => d.Payments)
                .OrderBy(d => d.InterestRate)
                .ThenBy(d => d.CurrentBalance)
                .ToListAsync();
        }

        public async Task<List<DebtAccount>> GetDebtsByTypeAsync(Guid userId, DebtType type)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DebtAccounts
                .Where(d => d.UserId == userId && d.Type == type)
                .Include(d => d.Payments)
                .OrderByDescending(d => d.CurrentBalance)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalDebtBalanceAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.DebtAccounts
                .Where(d => d.UserId == userId && d.Status == DebtStatus.Active)
                .SumAsync(d => d.CurrentBalance);
        }
    }
}
