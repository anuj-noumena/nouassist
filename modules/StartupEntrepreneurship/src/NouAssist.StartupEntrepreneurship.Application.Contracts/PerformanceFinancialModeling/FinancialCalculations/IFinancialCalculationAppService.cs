using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations
{
    /// <summary>
    /// Application service for financial calculations and analysis
    /// </summary>
    public interface IFinancialCalculationAppService : IApplicationService
    {
        // Sensitivity analysis
        Task<SensitivityAnalysisResultDto> PerformSensitivityAnalysisAsync(
            Guid modelId, Guid scenarioId, PerformSensitivityAnalysisDto input);
        
        Task<List<SensitivityAnalysisResultDto>> PerformMultiVariableSensitivityAsync(
            Guid modelId, Guid scenarioId, PerformMultiVariableSensitivityDto input);

        // Runway analysis
        Task<RunwayAnalysisResultDto> PerformRunwayAnalysisAsync(
            Guid modelId, Guid scenarioId, PerformRunwayAnalysisDto input);

        Task<List<RunwayAlertDto>> GetRunwayAlertsAsync(Guid modelId);

        // Break-even analysis
        Task<BreakEvenResultDto> PerformBreakEvenAnalysisAsync(
            Guid modelId, Guid scenarioId, PerformBreakEvenAnalysisDto input);

        // Runway optimization
        Task<RunwayOptimizationDto> OptimizeRunwayAsync(
            Guid modelId, Guid scenarioId, int targetMonths = 18);

        // Calculation history
        Task<List<CalculationResultDto>> GetCalculationHistoryAsync(Guid modelId, Guid? scenarioId = null);
    }
}
