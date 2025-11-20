using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives
{
    /// <summary>
    /// Application service for managing strategic initiatives
    /// </summary>
    public interface IStrategicInitiativeAppService : IApplicationService
    {
        /// <summary>
        /// Gets a strategic initiative by ID
        /// </summary>
        Task<StrategicInitiativeDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a filtered and paged list of strategic initiatives
        /// </summary>
        Task<PagedResultDto<StrategicInitiativeListDto>> GetListAsync(GetStrategicInitiativeListDto input);
        
        /// <summary>
        /// Creates a new strategic initiative
        /// </summary>
        Task<StrategicInitiativeDto> CreateAsync(CreateStrategicInitiativeDto input);
        
        /// <summary>
        /// Updates an existing strategic initiative
        /// </summary>
        Task<StrategicInitiativeDto> UpdateAsync(Guid id, UpdateStrategicInitiativeDto input);
        
        /// <summary>
        /// Deletes a strategic initiative
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Approves a strategic initiative
        /// </summary>
        Task<StrategicInitiativeDto> ApproveAsync(Guid id, ApproveInitiativeDto input);
        
        /// <summary>
        /// Rejects a strategic initiative
        /// </summary>
        Task<StrategicInitiativeDto> RejectAsync(Guid id, RejectInitiativeDto input);
        
        /// <summary>
        /// Adds a supporting analysis reference to an initiative
        /// </summary>
        Task<StrategicInitiativeDto> AddAnalysisReferenceAsync(Guid id, AddAnalysisReferenceDto input);
        
        /// <summary>
        /// Removes a supporting analysis reference from an initiative
        /// </summary>
        Task RemoveAnalysisReferenceAsync(Guid id, string referenceType, string referenceKey);
        
        /// <summary>
        /// Creates a strategic initiative from a decision model
        /// </summary>
        Task<StrategicInitiativeDto> CreateFromDecisionModelAsync(Guid decisionModelId, CreateStrategicInitiativeDto input);
    }
}
