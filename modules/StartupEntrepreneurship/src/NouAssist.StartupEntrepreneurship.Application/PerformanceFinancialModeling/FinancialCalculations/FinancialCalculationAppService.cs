using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations
{
    [Authorize(PerformanceFinancialModelingPermissions.FinancialCalculations.Default)]
    public class FinancialCalculationAppService : ApplicationService, IFinancialCalculationAppService
    {
        private readonly IFinancialModelRepository _financialModelRepository;
        private readonly ISensitivityRunRepository _sensitivityRunRepository;
        private readonly ISensitivityAnalysisService _sensitivityAnalysisService;
        private readonly IRunwayProjectionService _runwayProjectionService;
        private readonly IFinancialModelingEngine _financialModelingEngine;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;

        public FinancialCalculationAppService(
            IFinancialModelRepository financialModelRepository,
            ISensitivityRunRepository sensitivityRunRepository,
            ISensitivityAnalysisService sensitivityAnalysisService,
            IRunwayProjectionService runwayProjectionService,
            IFinancialModelingEngine financialModelingEngine,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _financialModelRepository = financialModelRepository;
            _sensitivityRunRepository = sensitivityRunRepository;
            _sensitivityAnalysisService = sensitivityAnalysisService;
            _runwayProjectionService = runwayProjectionService;
            _financialModelingEngine = financialModelingEngine;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(PerformanceFinancialModelingResource);
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialCalculations.SensitivityAnalysis)]
        public virtual async Task<SensitivityAnalysisResultDto> PerformSensitivityAnalysisAsync(
            Guid modelId, Guid scenarioId, PerformSensitivityAnalysisDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                if (!model.CanCalculate())
                {
                    throw new UserFriendlyException(L["ErrorMessage:ModelNotReady"]);
                }

                // Perform sensitivity analysis
                var result = await _sensitivityAnalysisService.PerformSensitivityAnalysisAsync(
                    model, scenario, input.VariableToAnalyze, input.VariancePercentage);

                // Save sensitivity run
                var sensitivityRun = new SensitivityRun(
                    model.Id,
                    scenario.Id,
                    $"Sensitivity Analysis - {input.VariableToAnalyze}",
                    new List<string> { input.VariableToAnalyze },
                    $"Variance: {input.VariancePercentage}%",
                    System.Text.Json.JsonSerializer.Serialize(result),
                    input.Notes
                );
                await _sensitivityRunRepository.InsertAsync(sensitivityRun, autoSave: true);

                // Map to DTO
                var dto = new SensitivityAnalysisResultDto
                {
                    ModelId = model.Id,
                    ModelName = model.Name,
                    ScenarioId = scenario.Id,
                    ScenarioName = scenario.Name,
                    CalculatedAt = DateTime.UtcNow,
                    IsSuccessful = true,
                    Notes = input.Notes,
                    VariableName = result.VariableName,
                    BaseValue = result.BaseValue,
                    BaseResult = result.BaseResult,
                    SensitivityPoints = result.SensitivityPoints,
                    Elasticity = result.Elasticity,
                    MaxImpact = result.MaxImpact,
                    MinImpact = result.MinImpact,
                    IsHighSensitivity = result.IsHighSensitivity(1.0m),
                    RiskRanges = result.GetRiskRanges()
                };

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialCalculations.SensitivityAnalysis)]
        public virtual async Task<List<SensitivityAnalysisResultDto>> PerformMultiVariableSensitivityAsync(
            Guid modelId, Guid scenarioId, PerformMultiVariableSensitivityDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                if (!model.CanCalculate())
                {
                    throw new UserFriendlyException(L["ErrorMessage:ModelNotReady"]);
                }

                // Perform multi-variable sensitivity analysis
                var results = await _sensitivityAnalysisService.PerformMultiVariableSensitivityAsync(
                    model, scenario, input.VariablesToAnalyze);

                // Save sensitivity run
                var sensitivityRun = new SensitivityRun(
                    model.Id,
                    scenario.Id,
                    "Multi-Variable Sensitivity Analysis",
                    input.VariablesToAnalyze.Keys.ToList(),
                    System.Text.Json.JsonSerializer.Serialize(input.VariablesToAnalyze),
                    System.Text.Json.JsonSerializer.Serialize(results),
                    input.Notes
                );
                await _sensitivityRunRepository.InsertAsync(sensitivityRun, autoSave: true);

                // Map to DTOs
                var dtos = results.Select(result => new SensitivityAnalysisResultDto
                {
                    ModelId = model.Id,
                    ModelName = model.Name,
                    ScenarioId = scenario.Id,
                    ScenarioName = scenario.Name,
                    CalculatedAt = DateTime.UtcNow,
                    IsSuccessful = true,
                    Notes = input.Notes,
                    VariableName = result.VariableName,
                    BaseValue = result.BaseValue,
                    BaseResult = result.BaseResult,
                    SensitivityPoints = result.SensitivityPoints,
                    Elasticity = result.Elasticity,
                    MaxImpact = result.MaxImpact,
                    MinImpact = result.MinImpact,
                    IsHighSensitivity = result.IsHighSensitivity(1.0m),
                    RiskRanges = result.GetRiskRanges()
                }).ToList();

                return dtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialCalculations.RunwayProjections)]
        public virtual async Task<RunwayAnalysisResultDto> PerformRunwayAnalysisAsync(
            Guid modelId, Guid scenarioId, PerformRunwayAnalysisDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                if (!model.CanCalculate())
                {
                    throw new UserFriendlyException(L["ErrorMessage:ModelNotReady"]);
                }

                // Calculate runway
                var runway = await _runwayProjectionService.CalculateCurrentRunwayAsync(model, scenario);
                var burnRate = await _runwayProjectionService.CalculateBurnRateAsync(model, scenario);
                var projections = await _runwayProjectionService.ProjectRunwayAsync(
                    model, scenario, input.ProjectionMonths);
                var alerts = await _runwayProjectionService.CheckRunwayAlertsAsync(model, new List<ModelScenario> { scenario });

                var dto = new RunwayAnalysisResultDto
                {
                    ModelId = model.Id,
                    ModelName = model.Name,
                    ScenarioId = scenario.Id,
                    ScenarioName = scenario.Name,
                    CalculatedAt = DateTime.UtcNow,
                    IsSuccessful = true,
                    Notes = input.Notes,
                    CurrentRunway = ObjectMapper.Map<RunwayLength, RunwayLengthDto>(runway),
                    BurnRate = ObjectMapper.Map<BurnRateValue, BurnRateValueDto>(burnRate),
                    Projections = projections.Select(p => new RunwayProjectionDto
                    {
                        MonthIndex = p.MonthIndex,
                        Date = p.Date,
                        CashPosition = p.CashPosition,
                        BurnRate = p.BurnRate,
                        RemainingMonths = p.RemainingMonths,
                        Status = p.Status
                    }).ToList(),
                    Alerts = alerts.Select(a => new RunwayAlertDto
                    {
                        ModelId = a.ModelId,
                        ModelName = a.ModelName,
                        ScenarioName = a.ScenarioName,
                        RemainingMonths = a.RemainingMonths,
                        AlertType = a.AlertType,
                        Message = a.Message,
                        Priority = a.Priority,
                        AlertDate = a.AlertDate
                    }).ToList()
                };

                // Include optimization if requested
                if (input.IncludeOptimization)
                {
                    var optimization = await _runwayProjectionService.OptimizeRunwayAsync(
                        model, scenario, input.TargetRunwayMonths);

                    dto.Optimization = new RunwayOptimizationDto
                    {
                        CurrentRunway = ObjectMapper.Map<RunwayLength, RunwayLengthDto>(optimization.CurrentRunway),
                        OptimizedRunway = ObjectMapper.Map<RunwayLength, RunwayLengthDto>(optimization.OptimizedRunway),
                        Options = optimization.Options.Select(o => new OptimizationOptionDto
                        {
                            Type = o.Type,
                            Description = o.Description,
                            Impact = o.Impact,
                            CostSavings = o.CostSavings,
                            AdditionalMonths = o.AdditionalMonths,
                            Difficulty = o.Difficulty
                        }).ToList(),
                        FundingRequired = optimization.FundingRequired,
                        Recommendations = optimization.Recommendations
                    };
                }

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialCalculations.ViewResults)]
        public virtual async Task<List<RunwayAlertDto>> GetRunwayAlertsAsync(Guid modelId)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenarios = model.Scenarios.Where(s => s.IsActive).ToList();
                var alerts = await _runwayProjectionService.CheckRunwayAlertsAsync(model, scenarios);

                return alerts.Select(a => new RunwayAlertDto
                {
                    ModelId = a.ModelId,
                    ModelName = a.ModelName,
                    ScenarioName = a.ScenarioName,
                    RemainingMonths = a.RemainingMonths,
                    AlertType = a.AlertType,
                    Message = a.Message,
                    Priority = a.Priority,
                    AlertDate = a.AlertDate
                }).ToList();
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialCalculations.Execute)]
        public virtual async Task<BreakEvenResultDto> PerformBreakEvenAnalysisAsync(
            Guid modelId, Guid scenarioId, PerformBreakEvenAnalysisDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                if (!model.CanCalculate())
                {
                    throw new UserFriendlyException(L["ErrorMessage:ModelNotReady"]);
                }

                // Get existing break-even projection or calculate new one
                var breakEvenProjection = await _financialModelingEngine.CalculateBreakEvenAnalysisAsync(model, scenario);

                // Extract break-even specific data from projection
                var breakEvenDate = breakEvenProjection.GetBreakEvenDate();
                var breakEvenMonth = breakEvenProjection.GetRunwayMonths();

                // Get assumptions for break-even calculation
                var assumptions = GetScenarioAssumptions(model, scenario);
                var fixedCosts = input.FixedCosts ?? assumptions.GetValueOrDefault("MonthlyFixedExpenses", 0m);
                var variableCost = input.VariableCostPerUnit ?? assumptions.GetValueOrDefault("VariableCostPerUnit", 0m);
                var pricePerUnit = input.PricePerUnit ?? assumptions.GetValueOrDefault("PricePerUnit", 0m);

                // Calculate break-even metrics
                decimal breakEvenUnits = 0;
                decimal breakEvenRevenue = 0;

                if (pricePerUnit > variableCost && pricePerUnit > 0)
                {
                    var contributionMargin = pricePerUnit - variableCost;
                    breakEvenUnits = fixedCosts / contributionMargin;
                    breakEvenRevenue = breakEvenUnits * pricePerUnit;
                }

                var dto = new BreakEvenResultDto
                {
                    BreakEvenDate = breakEvenDate,
                    BreakEvenMonthIndex = breakEvenMonth,
                    BreakEvenRevenue = breakEvenRevenue,
                    BreakEvenUnits = breakEvenUnits,
                    FixedCosts = fixedCosts,
                    VariableCostPerUnit = variableCost,
                    PricePerUnit = pricePerUnit
                };

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialCalculations.RunwayProjections)]
        public virtual async Task<RunwayOptimizationDto> OptimizeRunwayAsync(
            Guid modelId, Guid scenarioId, int targetMonths = 18)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                if (!model.CanCalculate())
                {
                    throw new UserFriendlyException(L["ErrorMessage:ModelNotReady"]);
                }

                var optimization = await _runwayProjectionService.OptimizeRunwayAsync(model, scenario, targetMonths);

                return new RunwayOptimizationDto
                {
                    CurrentRunway = ObjectMapper.Map<RunwayLength, RunwayLengthDto>(optimization.CurrentRunway),
                    OptimizedRunway = ObjectMapper.Map<RunwayLength, RunwayLengthDto>(optimization.OptimizedRunway),
                    Options = optimization.Options.Select(o => new OptimizationOptionDto
                    {
                        Type = o.Type,
                        Description = o.Description,
                        Impact = o.Impact,
                        CostSavings = o.CostSavings,
                        AdditionalMonths = o.AdditionalMonths,
                        Difficulty = o.Difficulty
                    }).ToList(),
                    FundingRequired = optimization.FundingRequired,
                    Recommendations = optimization.Recommendations
                };
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialCalculations.ViewResults)]
        public virtual async Task<List<CalculationResultDto>> GetCalculationHistoryAsync(Guid modelId, Guid? scenarioId = null)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId);
                CheckResourceAccess(model);

                List<SensitivityRun> runs;
                if (scenarioId.HasValue)
                {
                    runs = await _sensitivityRunRepository.GetByScenarioAsync(scenarioId.Value);
                }
                else
                {
                    runs = await _sensitivityRunRepository.GetByModelAsync(modelId);
                }

                return runs.Select(r => new CalculationResultDto
                {
                    ModelId = r.ModelId,
                    ModelName = model.Name,
                    ScenarioId = r.ScenarioId,
                    ScenarioName = model.GetScenario(r.ScenarioId)?.Name ?? "Unknown",
                    CalculatedAt = r.RunDate,
                    IsSuccessful = true,
                    Notes = r.Notes
                }).ToList();
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper methods
        protected virtual void CheckResourceAccess(FinancialModel model)
        {
            if (model.TenantId != _currentTenant.Id)
            {
                throw new EntityNotFoundException(typeof(FinancialModel), model.Id);
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in FinancialCalculationAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
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
    }
}
