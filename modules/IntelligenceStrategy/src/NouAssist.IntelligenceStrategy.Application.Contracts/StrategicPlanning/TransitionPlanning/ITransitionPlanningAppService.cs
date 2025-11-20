using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.TransitionPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.TransitionPlanning
{
    /// <summary>
    /// Application service interface for Transition Planning
    /// </summary>
    public interface ITransitionPlanningAppService : IApplicationService
    {
        Task<TransitionPlanDto> GetAsync(Guid id);
        Task<PagedResultDto<TransitionPlanDto>> GetListAsync(PagedResultRequestDto input);
        Task<TransitionPlanDto> CreateAsync(CreateTransitionPlanDto input);
        Task<TransitionPlanDto> UpdateAsync(Guid id, CreateTransitionPlanDto input);
        Task DeleteAsync(Guid id);

        // Capability Management
        Task<TransitionPlanDto> AddCapabilityRoadmapItemAsync(Guid planId, AddCapabilityRoadmapItemDto input);
        Task<TransitionPlanDto> UpdateCapabilityProgressAsync(Guid planId, Guid roadmapItemId, CapabilityLevel newLevel);

        // Milestone Management
        Task<TransitionPlanDto> AddMilestoneAsync(Guid planId, string milestoneDescription);

        // Plan Operations
        Task<TransitionPlanDto> ActivateAsync(Guid id);
        Task<TransitionPlanDto> CompleteAsync(Guid id);
        Task<TransitionPlanDto> PauseAsync(Guid id);
        Task<TransitionPlanDto> UpdateCurrentPhaseAsync(Guid id, string newPhase);

        // Reporting
        Task<ListResultDto<TransitionPlanDto>> GetActivePlansAsync();
        Task<ListResultDto<TransitionPlanDto>> GetPlansByTypeAsync(TransitionType type);
        Task<ListResultDto<TransitionPlanDto>> GetPlansRequiringAttentionAsync();
        Task<ListResultDto<CapabilityRoadmapItemDto>> GetItemsBehindScheduleAsync(Guid planId);
        Task<decimal> CalculateTransitionReadinessAsync(Guid planId);
    }
}
