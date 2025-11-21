using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Service for performing financial modeling calculations
    /// </summary>
    public interface IFinancialModelingEngine
    {
        Task<ProjectionResult> CalculateCashFlowProjectionAsync(FinancialModel model, ModelScenario scenario);
        Task<ProjectionResult> CalculateProfitAndLossProjectionAsync(FinancialModel model, ModelScenario scenario);
        Task<ProjectionResult> CalculateRevenueProjectionAsync(FinancialModel model, ModelScenario scenario);
        Task<ProjectionResult> CalculateExpenseProjectionAsync(FinancialModel model, ModelScenario scenario);
        Task<ProjectionResult> CalculateBreakEvenAnalysisAsync(FinancialModel model, ModelScenario scenario);
        Task<int?> CalculateRunwayProjectionAsync(FinancialModel model, ModelScenario scenario);
        Task<List<ProjectionResult>> CalculateAllProjectionsAsync(FinancialModel model, ModelScenario scenario);
    }
}
