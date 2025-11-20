using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels
{
    /// <summary>
    /// Application service for FinancialModel management
    /// </summary>
    public interface IFinancialModelAppService : IApplicationService
    {
        // CRUD operations
        Task<FinancialModelDto> GetAsync(Guid id);
        Task<FinancialModelDetailDto> GetDetailAsync(Guid id);
        Task<PagedResultDto<FinancialModelDto>> GetListAsync(GetFinancialModelListDto input);
        Task<FinancialModelDto> CreateAsync(CreateFinancialModelDto input);
        Task<FinancialModelDto> UpdateAsync(Guid id, UpdateFinancialModelDto input);
        Task DeleteAsync(Guid id);

        // Status management
        Task<FinancialModelDto> ChangeStatusAsync(Guid id, ChangeModelStatusDto input);
        Task<FinancialModelDto> ActivateAsync(Guid id);
        Task<FinancialModelDto> DeactivateAsync(Guid id);

        // Scenario management
        Task<ModelScenarioDto> CreateScenarioAsync(Guid modelId, CreateModelScenarioDto input);
        Task<ModelScenarioDto> UpdateScenarioAsync(Guid scenarioId, UpdateModelScenarioDto input);
        Task DeleteScenarioAsync(Guid scenarioId);
        Task<ModelScenarioDto> ActivateScenarioAsync(Guid scenarioId);
        Task<ModelScenarioDto> DeactivateScenarioAsync(Guid scenarioId);
        Task<List<ModelScenarioDto>> GetScenariosAsync(Guid modelId);
        Task AssociateAssumptionSetsAsync(Guid scenarioId, AssociateAssumptionSetsDto input);

        // Assumption set management
        Task<AssumptionSetDto> CreateAssumptionSetAsync(Guid modelId, CreateAssumptionSetDto input);
        Task<AssumptionSetDto> UpdateAssumptionSetAsync(Guid assumptionSetId, UpdateAssumptionSetDto input);
        Task DeleteAssumptionSetAsync(Guid assumptionSetId);
        Task<AssumptionSetDto> ActivateAssumptionSetAsync(Guid assumptionSetId);
        Task<AssumptionSetDto> DeactivateAssumptionSetAsync(Guid assumptionSetId);
        Task<List<AssumptionSetDto>> GetAssumptionSetsAsync(Guid modelId);
        Task<AssumptionSetDto> UpdateAssumptionAsync(Guid assumptionSetId, UpdateAssumptionDto input);

        // Projection management
        Task<ProjectionResultDto> CalculateProjectionAsync(Guid modelId, Guid scenarioId, CalculateProjectionDto input);
        Task<List<ProjectionResultDto>> CalculateAllProjectionsAsync(Guid modelId, Guid scenarioId);
        Task<List<ProjectionResultDto>> GetProjectionsAsync(Guid modelId, Guid? scenarioId = null);
        Task<ProjectionResultDto> GetProjectionAsync(Guid modelId, Guid scenarioId, ProjectionType type);

        // Lookups
        Task<List<FinancialModelDto>> GetActiveModelsAsync();
        Task<List<FinancialModelDto>> GetCalculableModelsAsync();
    }
}
