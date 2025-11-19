using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialAnalytics.ReadModels;

namespace NouAssist.FinancialManagement.FinancialAnalytics.Data
{
    /// <summary>
    /// Repository for querying analytics read models
    /// </summary>
    public interface IAnalyticsQueryRepository
    {
        Task<List<AccountBalanceSummary>> GetAccountBalanceSummariesAsync(
            Guid? tenantId,
            DateTime reportDate);

        Task<CashFlowProjection> GetCashFlowProjectionAsync(
            Guid? tenantId,
            int daysAhead = 30);

        Task<BudgetPerformanceSummary> GetBudgetPerformanceSummaryAsync(
            Guid budgetId);

        Task<List<BudgetPerformanceSummary>> GetAllBudgetPerformanceSummariesAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null);
    }
}
