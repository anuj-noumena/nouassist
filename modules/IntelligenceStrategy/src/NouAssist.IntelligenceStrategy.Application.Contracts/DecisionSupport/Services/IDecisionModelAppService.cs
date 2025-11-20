using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.DecisionSupport.Dtos.Alternative;
using NouAssist.DecisionSupport.Dtos.Criterion;
using NouAssist.DecisionSupport.Dtos.DecisionModel;
using NouAssist.DecisionSupport.Dtos.Evaluation;
using NouAssist.DecisionSupport.Dtos.Score;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DecisionSupport.Services
{
    /// <summary>
    /// Application service for decision model operations
    /// </summary>
    public interface IDecisionModelAppService : 
        ICrudAppService<
            DecisionModelDto,
            Guid,
            GetDecisionModelsInput,
            CreateDecisionModelDto,
            UpdateDecisionModelDto>
    {
        /// <summary>
        /// Gets detailed information about a decision model
        /// </summary>
        Task<DecisionModelDetailDto> GetDetailsAsync(Guid id);
        
        /// <summary>
        /// Adds a criterion to a decision model
        /// </summary>
        Task<CriterionDto> AddCriterionAsync(CreateCriterionDto input);
        
        /// <summary>
        /// Updates a criterion
        /// </summary>
        Task<CriterionDto> UpdateCriterionAsync(Guid id, UpdateCriterionDto input);
        
        /// <summary>
        /// Deletes a criterion
        /// </summary>
        Task DeleteCriterionAsync(Guid id);
        
        /// <summary>
        /// Adds an alternative to a decision model
        /// </summary>
        Task<AlternativeDto> AddAlternativeAsync(CreateAlternativeDto input);
        
        /// <summary>
        /// Updates an alternative
        /// </summary>
        Task<AlternativeDto> UpdateAlternativeAsync(Guid id, UpdateAlternativeDto input);
        
        /// <summary>
        /// Deletes an alternative
        /// </summary>
        Task DeleteAlternativeAsync(Guid id);
        
        /// <summary>
        /// Sets a score for an alternative on a criterion
        /// </summary>
        Task SetScoreAsync(SetScoreDto input);
        
        /// <summary>
        /// Evaluates and ranks alternatives in a decision model
        /// </summary>
        Task<List<AlternativeEvaluationResultDto>> EvaluateAlternativesAsync(Guid decisionModelId);
    }
}
