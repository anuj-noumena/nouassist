using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.DecisionSupport.Aggregates;
using NouAssist.DecisionSupport.Dtos.ScenarioAnalysis;
using NouAssist.DecisionSupport.Dtos.ScenarioModel;
using NouAssist.DecisionSupport.Dtos.ScenarioOutcome;
using NouAssist.DecisionSupport.Interfaces.Repositories;
using NouAssist.DecisionSupport.Interfaces.Services;
using NouAssist.DecisionSupport.Permissions;
using NouAssist.DecisionSupport.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DecisionSupport.Services
{
    [Authorize(DecisionSupportPermissions.ScenarioAnalyses.Default)]
    public class ScenarioAnalysisAppService :
        CrudAppService<
            ScenarioAnalysis,
            ScenarioAnalysisDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateScenarioAnalysisDto,
            UpdateScenarioAnalysisDto>,
        IScenarioAnalysisAppService
    {
        private readonly IScenarioAnalysisRepository _scenarioRepository;
        private readonly IScenarioAnalysisManager _scenarioManager;

        public ScenarioAnalysisAppService(
            IScenarioAnalysisRepository scenarioRepository,
            IScenarioAnalysisManager scenarioManager)
            : base(scenarioRepository)
        {
            _scenarioRepository = scenarioRepository;
            _scenarioManager = scenarioManager;
            
            GetPolicyName = DecisionSupportPermissions.ScenarioAnalyses.Default;
            GetListPolicyName = DecisionSupportPermissions.ScenarioAnalyses.Default;
            CreatePolicyName = DecisionSupportPermissions.ScenarioAnalyses.Create;
            UpdatePolicyName = DecisionSupportPermissions.ScenarioAnalyses.Edit;
            DeletePolicyName = DecisionSupportPermissions.ScenarioAnalyses.Delete;
        }

        public override async Task<ScenarioAnalysisDto> GetAsync(Guid id)
        {
            var scenarioAnalysis = await _scenarioRepository.GetWithDetailsAsync(id);
            
            var dto = ObjectMapper.Map<ScenarioAnalysis, ScenarioAnalysisDto>(scenarioAnalysis);
            
            // Set most likely and highest impact scenarios
            var mostLikelyScenario = scenarioAnalysis.GetMostLikelyScenario();
            if (mostLikelyScenario != null)
            {
                dto.MostLikelyScenario = ObjectMapper.Map<Entities.ScenarioModel, ScenarioModelDto>(mostLikelyScenario);
            }
            
            var highestImpactScenario = scenarioAnalysis.GetHighestImpactScenario();
            if (highestImpactScenario != null)
            {
                dto.HighestImpactScenario = ObjectMapper.Map<Entities.ScenarioModel, ScenarioModelDto>(highestImpactScenario);
            }
            
            // Calculate average impact for each scenario model
            foreach (var scenarioModel in dto.ScenarioModels)
            {
                if (scenarioModel.Outcomes.Any())
                {
                    scenarioModel.AverageImpact = scenarioModel.Outcomes.Average(o => o.ImpactValue);
                }
            }
            
            return dto;
        }

        [Authorize(DecisionSupportPermissions.ScenarioAnalyses.Create)]
        public override async Task<ScenarioAnalysisDto> CreateAsync(CreateScenarioAnalysisDto input)
        {
            var scenarioAnalysis = await _scenarioManager.CreateScenarioAnalysisAsync(
                input.Title,
                input.Purpose,
                input.TimeHorizon,
                input.Scope,
                input.KeyDrivers,
                input.DecisionModelId);
                
            return ObjectMapper.Map<ScenarioAnalysis, ScenarioAnalysisDto>(scenarioAnalysis);
        }

        public async Task<PagedResultDto<ScenarioAnalysisDto>> GetByDecisionModelAsync(
            Guid decisionModelId, 
            PagedAndSortedResultRequestDto input)
        {
            var scenarioAnalyses = await _scenarioRepository.GetByDecisionModelIdAsync(decisionModelId);
            
            var totalCount = scenarioAnalyses.Count;
            
            var items = scenarioAnalyses
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();
                
            return new PagedResultDto<ScenarioAnalysisDto>(
                totalCount,
                ObjectMapper.Map<List<ScenarioAnalysis>, List<ScenarioAnalysisDto>>(items));
        }

        [Authorize(DecisionSupportPermissions.ScenarioAnalyses.Edit)]
        public async Task<ScenarioAnalysisDto> AddScenarioModelAsync(CreateScenarioModelDto input)
        {
            var scenarioAnalysis = await _scenarioManager.AddScenarioModelAsync(
                input.ScenarioAnalysisId,
                input.Name,
                input.Title,
                input.Description,
                input.Assumptions,
                input.Likelihood,
                input.ConfidenceLevel,
                input.LikelihoodRationale);
                
            return await GetAsync(scenarioAnalysis.Id);
        }

        [Authorize(DecisionSupportPermissions.ScenarioAnalyses.Edit)]
        public async Task<ScenarioAnalysisDto> AddScenarioOutcomeAsync(CreateScenarioOutcomeDto input)
        {
            var impact = new Impact(input.ImpactValue, input.ImpactDescription);
            
            var scenarioAnalysis = await _scenarioManager.AddScenarioOutcomeAsync(
                input.ScenarioModelId, // ScenarioAnalysisId is determined by the ScenarioModel
                input.ScenarioModelId,
                input.Area,
                input.Description,
                input.ImpactValue,
                input.ImpactDescription);
                
            return await GetAsync(scenarioAnalysis.Id);
        }
    }
}
