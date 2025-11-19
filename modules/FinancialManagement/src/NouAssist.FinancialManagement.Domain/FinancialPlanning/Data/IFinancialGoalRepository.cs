using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning.Data
{
    public interface IFinancialGoalRepository : IRepository<FinancialGoal, Guid>
    {
        Task<List<FinancialGoal>> GetGoalsByUserAsync(Guid userId);
        Task<List<FinancialGoal>> GetActiveGoalsAsync(Guid userId);
        Task<List<FinancialGoal>> GetGoalsByTypeAsync(Guid userId, GoalType type);
        Task<List<FinancialGoal>> GetGoalsByStatusAsync(Guid userId, GoalStatus status);
        Task<List<FinancialGoal>> GetGoalsDueByDateAsync(Guid userId, DateTime targetDate);
    }
}
