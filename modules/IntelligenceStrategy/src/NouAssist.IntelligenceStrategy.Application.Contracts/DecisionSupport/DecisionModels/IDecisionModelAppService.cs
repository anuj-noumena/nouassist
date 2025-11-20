using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels
{
    /// <summary>
    /// Application service for managing decision models
    /// </summary>
    public interface IDecisionModelAppService : IApplicationService
    {
        /// <summary>
        /// Gets a decision model by ID
        /// </summary>
        Task<DecisionModelDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a filtered and paged list of decision models
        /// </summary>
        Task<PagedResultDto<DecisionModelListDto>> GetListAsync(GetDecisionModelListDto input);
        
        /// <summary>
        /// Creates a new decision model
        /// </summary>
        Task<DecisionModelDto> CreateAsync(CreateDecisionModelDto input);
        
        /// <summary>
        /// Deletes a decision model
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Adds a criterion to a decision model
        /// </summary>
        Task<CriterionDto> AddCriterionAsync(Guid id, AddCriterionDto input);
        
        /// <summary>
        /// Adds an alternative to a decision model
        /// </summary>
        Task<AlternativeDto> AddAlternativeAsync(Guid id, AddAlternativeDto input);
        
        /// <summary>
        /// Sets a score for an alternative on a criterion
        /// </summary>
        Task SetScoreAsync(Guid id, Guid alternativeId, SetScoreDto input);
        
        /// <summary>
        /// Finalizes a decision model by selecting an alternative
        /// </summary>
        Task<DecisionModelDto> FinalizeDecisionAsync(Guid id, FinalizeDecisionDto input);
    }
}
