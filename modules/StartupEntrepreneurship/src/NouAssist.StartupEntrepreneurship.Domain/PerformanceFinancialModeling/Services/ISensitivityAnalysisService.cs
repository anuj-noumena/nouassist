using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Service for performing sensitivity analysis on financial models
    /// </summary>
    public interface ISensitivityAnalysisService
    {
        Task<SensitivityResult> PerformSensitivityAnalysisAsync(FinancialModel model, ModelScenario scenario, 
                                                              string variableToAnalyze, decimal variancePercentage);
        Task<List<SensitivityResult>> PerformMultiVariableSensitivityAsync(FinancialModel model, ModelScenario scenario, 
                                                                          Dictionary<string, decimal> variablesToAnalyze);
        Task<SensitivityMatrix> GenerateSensitivityMatrixAsync(FinancialModel model, ModelScenario scenario, 
                                                              string variable1, string variable2, 
                                                              decimal variance1, decimal variance2);
    }

    /// <summary>
    /// Result of sensitivity analysis
    /// </summary>
    public class SensitivityResult
    {
        public string VariableName { get; set; }
        public decimal BaseValue { get; set; }
        public Dictionary<decimal, ProjectionResult> Results { get; set; }
        public decimal MaxImpact { get; set; }
        public decimal MinImpact { get; set; }
        public decimal Elasticity { get; set; }
    }

    /// <summary>
    /// Matrix for two-variable sensitivity analysis
    /// </summary>
    public class SensitivityMatrix
    {
        public string Variable1Name { get; set; }
        public string Variable2Name { get; set; }
        public decimal[,] Results { get; set; }
        public decimal[] Variable1Values { get; set; }
        public decimal[] Variable2Values { get; set; }
    }
}
