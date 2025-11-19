using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Data;
using NouAssist.FinancialManagement.InvestmentManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.InvestmentManagement.EntityFrameworkCore.Repositories
{
    public class EfCorePortfolioRepository 
        : EfCoreRepository<IInvestmentManagementDbContext, Portfolio, Guid>, 
          IPortfolioRepository
    {
        public EfCorePortfolioRepository(
            IDbContextProvider<IInvestmentManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Portfolio?> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Portfolios
                .Include(p => p.Holdings)
                .Include(p => p.Transactions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Portfolio>> GetUserPortfoliosAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Portfolios
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.TotalValue)
                .ToListAsync();
        }

        public async Task<List<Portfolio>> GetActivePortfoliosAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Portfolios
                .Where(p => p.UserId == userId && p.Status == PortfolioStatus.Active)
                .Include(p => p.Holdings)
                .OrderByDescending(p => p.TotalValue)
                .ToListAsync();
        }
    }
}
