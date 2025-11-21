using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Exceptions;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Domain service for sensitivity analysis
    /// </summary>
    public class SensitivityAnalysisService : DomainService, ISensitivityAnalysisService
    {
        private readonly IFinancialModelingEngine _modelingEngine;
        private readonly ILogger<SensitivityAnalysisService> _logger;

        public SensitivityAnalysisService(
            IFinancialModelingEngine modelingEngine,
            ILogger<SensitivityAnalysisService> logger)
        {
            _modelingEngine = modelingEngine;
            _logger = logger;
        }

        public async Task<SensitivityResult> PerformSensitivityAnalysisAsync(
            FinancialModel model, ModelScenario scenario, string variableToAnalyze, decimal variancePercentage)
        {
            try
            {
                var assumptions = GetScenarioAssumptions(model, scenario);
                if (!assumptions.ContainsKey(variableToAnalyze))
                {
                    throw new InvalidScenarioException(scenario.Name, $"Variable '{variableToAnalyze}' not found in assumptions");
                }

                var baseValue = assumptions[variableToAnalyze];
                var results = new Dictionary<decimal, ProjectionResult>();

                // Test different variance levels
                var varianceLevels = new[] { -variancePercentage, -variancePercentage/2, 0, variancePercentage/2, variancePercentage };

                foreach (var variance in varianceLevels)
                {
                    var modifiedAssumptions = new Dictionary<string, decimal>(assumptions);
                    modifiedAssumptions[variableToAnalyze] = baseValue * (1 + variance / 100);

                    // Create temporary scenario with modified assumptions
                    var tempScenario = await CreateTempScenarioWithAssumptions(model, scenario, modifiedAssumptions);
                    var projection = await _modelingEngine.CalculateCashFlowProjectionAsync(model, tempScenario);
                    
                    results[variance] = projection;
                }

                // Calculate elasticity and impact
                var baseResult = results[0];
                var maxVarianceResult = results[variancePercentage];
                var minVarianceResult = results[-variancePercentage];

                var baseValue100 = baseResult.GetTotalProjected();
                var maxValue = maxVarianceResult.GetTotalProjected();
                var minValue = minVarianceResult.GetTotalProjected();

                var maxImpact = baseValue100 != 0 ? ((maxValue - baseValue100) / baseValue100) * 100 : 0;
                var minImpact = baseValue100 != 0 ? ((minValue - baseValue100) / baseValue100) * 100 : 0;
                var elasticity = variancePercentage != 0 ? maxImpact / variancePercentage : 0;

                return new SensitivityResult
                {
                    VariableName = variableToAnalyze,
                    BaseValue = baseValue,
                    Results = results,
                    MaxImpact = maxImpact,
                    MinImpact = minImpact,
                    Elasticity = elasticity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to perform sensitivity analysis for variable {Variable} in model {ModelId}", 
                               variableToAnalyze, model.Id);
                throw;
            }
        }

        public async Task<List<SensitivityResult>> PerformMultiVariableSensitivityAsync(
            FinancialModel model, ModelScenario scenario, Dictionary<string, decimal> variablesToAnalyze)
        {
            var results = new List<SensitivityResult>();

            foreach (var variable in variablesToAnalyze)
            {
                var result = await PerformSensitivityAnalysisAsync(model, scenario, variable.Key, variable.Value);
                results.Add(result);
            }

            return results;
        }

        public async Task<SensitivityMatrix> GenerateSensitivityMatrixAsync(
            FinancialModel model, ModelScenario scenario, 
            string variable1, string variable2, decimal variance1, decimal variance2)
        {
            try
            {
                var assumptions = GetScenarioAssumptions(model, scenario);
                
                if (!assumptions.ContainsKey(variable1) || !assumptions.ContainsKey(variable2))
                {
                    throw new InvalidScenarioException(scenario.Name, "One or both variables not found in assumptions");
                }

                var baseValue1 = assumptions[variable1];
                var baseValue2 = assumptions[variable2];

                var variance1Levels = new[] { -variance1, -variance1/2, 0, variance1/2, variance1 };
                var variance2Levels = new[] { -variance2, -variance2/2, 0, variance2/2, variance2 };

                var results = new decimal[variance1Levels.Length, variance2Levels.Length];
                var variable1Values = variance1Levels.Select(v => baseValue1 * (1 + v / 100)).ToArray();
                var variable2Values = variance2Levels.Select(v => baseValue2 * (1 + v / 100)).ToArray();

                for (int i = 0; i < variance1Levels.Length; i++)
                {
                    for (int j = 0; j < variance2Levels.Length; j++)
                    {
                        var modifiedAssumptions = new Dictionary<string, decimal>(assumptions);
                        modifiedAssumptions[variable1] = baseValue1 * (1 + variance1Levels[i] / 100);
                        modifiedAssumptions[variable2] = baseValue2 * (1 + variance2Levels[j] / 100);

                        var tempScenario = await CreateTempScenarioWithAssumptions(model, scenario, modifiedAssumptions);
                        var projection = await _modelingEngine.CalculateCashFlowProjectionAsync(model, tempScenario);
                        
                        results[i, j] = projection.GetTotalProjected();
                    }
                }

                return new SensitivityMatrix
                {
                    Variable1Name = variable1,
                    Variable2Name = variable2,
                    Results = results,
                    Variable1Values = variable1Values,
                    Variable2Values = variable2Values
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate sensitivity matrix for variables {Variable1}, {Variable2} in model {ModelId}", 
                               variable1, variable2, model.Id);
                throw;
            }
        }

        private Dictionary<string, decimal> GetScenarioAssumptions(FinancialModel model, ModelScenario scenario)
        {
            var allAssumptions = new Dictionary<string, decimal>();

            foreach (var assumptionSetId in scenario.AssociatedAssumptionSets)
            {
                var assumptionSet = model.GetAssumptionSet(assumptionSetId);
                if (assumptionSet != null)
                {
                    foreach (var assumption in assumptionSet.Assumptions)
                    {
                        allAssumptions[assumption.Name] = assumption.Value;
                    }
                }
            }

            return allAssumptions;
        }

        private async Task<ModelScenario> CreateTempScenarioWithAssumptions(
            FinancialModel model, ModelScenario originalScenario, Dictionary<string, decimal> newAssumptions)
        {
            // This would typically create a temporary scenario for calculation purposes
            // For now, we'll return the original scenario as this is domain logic
            await Task.CompletedTask;
            return originalScenario;
        }
    }
}
