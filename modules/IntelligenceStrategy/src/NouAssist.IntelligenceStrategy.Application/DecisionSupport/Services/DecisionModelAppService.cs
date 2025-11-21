using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.DecisionSupport.Dtos.Alternative;
using NouAssist.DecisionSupport.Dtos.Criterion;
using NouAssist.DecisionSupport.Dtos.DecisionModel;
using NouAssist.DecisionSupport.Dtos.Evaluation;
using NouAssist.DecisionSupport.Dtos.Score;
using NouAssist.DecisionSupport.Permissions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Repositories;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services;
using NouAssist.IntelligenceStrategy.DecisionSupport.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.DecisionSupport.Services
{
    [Authorize(DecisionSupportPermissions.DecisionModels.Default)]
    public class DecisionModelAppService : 
        CrudAppService<
            DecisionModelInstance,
            DecisionModelDto,
            Guid,
            GetDecisionModelsInput,
            CreateDecisionModelDto,
            UpdateDecisionModelDto>,
        IDecisionModelAppService
    {
        private readonly IDecisionModelRepository _decisionModelRepository;
        private readonly IDecisionModelManager _decisionModelManager;
        private readonly IRepository<Criterion, Guid> _criterionRepository;
        private readonly IRepository<Alternative, Guid> _alternativeRepository;

        public DecisionModelAppService(
            IDecisionModelRepository decisionModelRepository,
            IDecisionModelManager decisionModelManager,
            IRepository<Criterion, Guid> criterionRepository,
            IRepository<Alternative, Guid> alternativeRepository)
            : base(decisionModelRepository)
        {
            _decisionModelRepository = decisionModelRepository;
            _decisionModelManager = decisionModelManager;
            _criterionRepository = criterionRepository;
            _alternativeRepository = alternativeRepository;
            
            GetPolicyName = DecisionSupportPermissions.DecisionModels.Default;
            GetListPolicyName = DecisionSupportPermissions.DecisionModels.Default;
            CreatePolicyName = DecisionSupportPermissions.DecisionModels.Create;
            UpdatePolicyName = DecisionSupportPermissions.DecisionModels.Edit;
            DeletePolicyName = DecisionSupportPermissions.DecisionModels.Delete;
        }

        protected override async Task<IQueryable<DecisionModelInstance>> CreateFilteredQueryAsync(GetDecisionModelsInput input)
        {
            var query = await base.CreateFilteredQueryAsync(input);
            
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(x => x.Name.Contains(input.Filter) || 
                                         x.Description.Contains(input.Filter));
            }
            
            return query;
        }

        public override async Task<DecisionModelDto> GetAsync(Guid id)
        {
            var decisionModel = await _decisionModelRepository.GetAsync(id);
            
            var dto = ObjectMapper.Map<DecisionModelInstance, DecisionModelDto>(decisionModel);
            
            dto.CriteriaCount = decisionModel.Criteria.Count;
            dto.AlternativesCount = decisionModel.Alternatives.Count;
            
            return dto;
        }

        public async Task<DecisionModelDetailDto> GetDetailsAsync(Guid id)
        {
            var decisionModel = await _decisionModelRepository.GetWithDetailsAsync(id);
            
            var dto = ObjectMapper.Map<DecisionModelInstance, DecisionModelDetailDto>(decisionModel);
            
            // Map criteria
            dto.Criteria = ObjectMapper.Map<List<Criterion>, List<CriterionDto>>(decisionModel.Criteria.ToList());
            
            // Map alternatives with scores
            dto.Alternatives = ObjectMapper.Map<List<Alternative>, List<AlternativeDto>>(decisionModel.Alternatives.ToList());
            
            // Enrich alternative scores with criterion names
            foreach (var alternative in dto.Alternatives)
            {
                foreach (var score in alternative.Scores)
                {
                    var criterion = dto.Criteria.FirstOrDefault(c => c.Id == score.CriterionId);
                    if (criterion != null)
                    {
                        score.CriterionName = criterion.Name;
                        score.WeightedScore = score.Score * criterion.Weight;
                    }
                }
            }
            
            return dto;
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Create)]
        public override async Task<DecisionModelDto> CreateAsync(CreateDecisionModelDto input)
        {
            var decisionModel = await _decisionModelManager.CreateDecisionModelAsync(
                input.Name,
                input.Description,
                input.Context,
                input.DecisionDueDate);
                
            return ObjectMapper.Map<DecisionModelInstance, DecisionModelDto>(decisionModel);
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Edit)]
        public override async Task<DecisionModelDto> UpdateAsync(Guid id, UpdateDecisionModelDto input)
        {
            var decisionModel = await _decisionModelManager.UpdateDecisionModelAsync(
                id,
                input.Name,
                input.Description,
                input.Context,
                input.DecisionDueDate);
                
            return ObjectMapper.Map<DecisionModelInstance, DecisionModelDto>(decisionModel);
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Create)]
        public async Task<CriterionDto> AddCriterionAsync(CreateCriterionDto input)
        {
            var decisionModel = await _decisionModelRepository.GetAsync(input.DecisionModelId);
            
            var criterion = decisionModel.AddCriterion(
                input.Name,
                input.Description,
                input.Weight);
                
            await _decisionModelRepository.UpdateAsync(decisionModel);
            
            return ObjectMapper.Map<Criterion, CriterionDto>(criterion);
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Edit)]
        public async Task<CriterionDto> UpdateCriterionAsync(Guid id, UpdateCriterionDto input)
        {
            var criterion = await _criterionRepository.GetAsync(id);
            
            var decisionModel = await _decisionModelRepository.GetAsync(criterion.DecisionModelId);
            
            var updatedCriterion = decisionModel.UpdateCriterion(
                id,
                input.Name,
                input.Description,
                input.Weight);
                
            await _decisionModelRepository.UpdateAsync(decisionModel);
            
            return ObjectMapper.Map<Criterion, CriterionDto>(updatedCriterion);
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Delete)]
        public async Task DeleteCriterionAsync(Guid id)
        {
            var criterion = await _criterionRepository.GetAsync(id);
            
            var decisionModel = await _decisionModelRepository.GetAsync(criterion.DecisionModelId);
            
            decisionModel.RemoveCriterion(id);
            
            await _decisionModelRepository.UpdateAsync(decisionModel);
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Create)]
        public async Task<AlternativeDto> AddAlternativeAsync(CreateAlternativeDto input)
        {
            var decisionModel = await _decisionModelRepository.GetAsync(input.DecisionModelId);
            
            var alternative = decisionModel.AddAlternative(
                input.Name,
                input.Description);
                
            await _decisionModelRepository.UpdateAsync(decisionModel);
            
            return ObjectMapper.Map<Alternative, AlternativeDto>(alternative);
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Edit)]
        public async Task<AlternativeDto> UpdateAlternativeAsync(Guid id, UpdateAlternativeDto input)
        {
            var alternative = await _alternativeRepository.GetAsync(id);
            
            var decisionModel = await _decisionModelRepository.GetAsync(alternative.DecisionModelId);
            
            var updatedAlternative = decisionModel.UpdateAlternative(
                id,
                input.Name,
                input.Description);
                
            await _decisionModelRepository.UpdateAsync(decisionModel);
            
            return ObjectMapper.Map<Alternative, AlternativeDto>(updatedAlternative);
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Delete)]
        public async Task DeleteAlternativeAsync(Guid id)
        {
            var alternative = await _alternativeRepository.GetAsync(id);
            
            var decisionModel = await _decisionModelRepository.GetAsync(alternative.DecisionModelId);
            
            decisionModel.RemoveAlternative(id);
            
            await _decisionModelRepository.UpdateAsync(decisionModel);
        }

        [Authorize(DecisionSupportPermissions.DecisionModels.Edit)]
        public async Task SetScoreAsync(SetScoreDto input)
        {
            var alternative = await _alternativeRepository.GetAsync(input.AlternativeId);
            
            var decisionModel = await _decisionModelRepository.GetAsync(alternative.DecisionModelId);
            
            decisionModel.SetAlternativeScore(
                input.AlternativeId,
                input.CriterionId,
                input.Score,
                input.Justification);
                
            await _decisionModelRepository.UpdateAsync(decisionModel);
        }

        public async Task<List<AlternativeEvaluationResultDto>> EvaluateAlternativesAsync(Guid decisionModelId)
        {
            var results = await _decisionModelManager.CompareAlternativesAsync(decisionModelId);
            
            var decisionModel = await _decisionModelRepository.GetWithDetailsAsync(decisionModelId);
            var ranks = decisionModel.RankAlternatives();
            
            var evaluationResults = new List<AlternativeEvaluationResultDto>();
            
            foreach (var alternativeId in results.Keys)
            {
                var alternative = decisionModel.Alternatives.FirstOrDefault(a => a.Id == alternativeId);
                if (alternative != null)
                {
                    evaluationResults.Add(new AlternativeEvaluationResultDto
                    {
                        AlternativeId = alternativeId,
                        AlternativeName = alternative.Name,
                        TotalScore = results[alternativeId],
                        Rank = ranks[alternativeId]
                    });
                }
            }
            
            return evaluationResults.OrderBy(r => r.Rank).ToList();
        }
    }
}
