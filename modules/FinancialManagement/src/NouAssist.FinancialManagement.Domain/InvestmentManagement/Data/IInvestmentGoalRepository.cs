using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.InvestmentManagement.Data
{
    public interface IInvestmentGoalRepository : IRepository<InvestmentGoal, Guid>
    {
        Task<List<InvestmentGoal>> GetUserGoalsAsync(Guid userId);
        Task<List<InvestmentGoal>> GetActiveGoalsAsync(Guid userId);
        Task<List<InvestmentGoal>> GetGoalsByPortfolioAsync(Guid portfolioId);
    }
}
