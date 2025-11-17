using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.InvestmentManagement.Data
{
    public interface IPortfolioRepository : IRepository<Portfolio, Guid>
    {
        Task<Portfolio?> GetWithDetailsAsync(Guid id);
        Task<List<Portfolio>> GetUserPortfoliosAsync(Guid userId);
        Task<List<Portfolio>> GetActivePortfoliosAsync(Guid userId);
    }
}
