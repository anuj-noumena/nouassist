using System;
using System.Threading.Tasks;
using NouAssist.DecisionSupport.Dtos.StrategicInitiative;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DecisionSupport.Services
{
    /// <summary>
    /// Application service for strategic initiative operations
    /// </summary>
    public interface IStrategicInitiativeAppService :
        ICrudAppService<
            StrategicInitiativeDto,
            Guid,
            GetStrategicInitiativesInput,
            CreateStrategicInitiativeDto,
            UpdateStrategicInitiativeDto>
    {
        /// <summary>
        /// Approves a strategic initiative
        /// </summary>
        Task<StrategicInitiativeDto> ApproveAsync(Guid id, ApproveInitiativeDto input);
        
        /// <summary>
        /// Rejects a strategic initiative
        /// </summary>
        Task<StrategicInitiativeDto> RejectAsync(Guid id, RejectInitiativeDto input);
        
        /// <summary>
        /// Gets strategic initiatives by decision model
        /// </summary>
        Task<PagedResultDto<StrategicInitiativeDto>> GetByDecisionModelAsync(Guid decisionModelId, PagedAndSortedResultRequestDto input);
    }
}
