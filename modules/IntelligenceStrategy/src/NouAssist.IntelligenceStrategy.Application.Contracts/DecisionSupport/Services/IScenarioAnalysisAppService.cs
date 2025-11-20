using System;
using System.Threading.Tasks;
using NouAssist.DecisionSupport.Dtos.ScenarioAnalysis;
using NouAssist.DecisionSupport.Dtos.ScenarioModel;
using NouAssist.DecisionSupport.Dtos.ScenarioOutcome;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DecisionSupport.Services
{
    /// <summary>
    /// Application service for scenario analysis operations
    /// </summary>
    public interface IScenarioAnalysisAppService :
        ICrudAppService<
            ScenarioAnalysisDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateScenarioAnalysisDto,
            UpdateScenarioAnalysisDto>
    {
        /// <summary>
        /// Gets scenario analyses by decision model
        /// </summary>
        Task<PagedResultDto<ScenarioAnalysisDto>> GetByDecisionModelAsync(Guid decisionModelId, PagedAndSortedResultRequestDto input);
        
        /// <summary>
        /// Adds a scenario model to an analysis
        /// </summary>
        Task<ScenarioAnalysisDto> AddScenarioModelAsync(CreateScenarioModelDto input);
        
        /// <summary>
        /// Adds an outcome to a scenario model
        /// </summary>
        Task<ScenarioAnalysisDto> AddScenarioOutcomeAsync(CreateScenarioOutcomeDto input);
    }
}
