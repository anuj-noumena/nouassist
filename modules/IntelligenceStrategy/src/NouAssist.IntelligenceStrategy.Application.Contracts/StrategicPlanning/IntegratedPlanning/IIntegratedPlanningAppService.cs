using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.IntegratedPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.IntegratedPlanning
{
    /// <summary>
    /// Application service interface for Integrated Horizon Planning
    /// </summary>
    public interface IIntegratedPlanningAppService : IApplicationService
    {
        Task<IntegratedHorizonPlanDto> GetAsync(Guid id);
        Task<PagedResultDto<IntegratedHorizonPlanDto>> GetListAsync(PagedResultRequestDto input);
        Task<IntegratedHorizonPlanDto> CreateAsync(CreateIntegratedHorizonPlanDto input);
        Task<IntegratedHorizonPlanDto> UpdateAsync(Guid id, CreateIntegratedHorizonPlanDto input);
        Task DeleteAsync(Guid id);

        // Timeline Management
        Task<IntegratedHorizonPlanDto> AddTimelineEventAsync(Guid planId, AddTimelineEventDto input);
        Task<IntegratedHorizonPlanDto> LinkEventToGoalAsync(Guid planId, Guid eventId, Guid goalId);

        // Planning Operations
        Task<ListResultDto<TimelineEventMarkerDto>> GenerateBackwardPlanAsync(Guid planId);
        Task<IntegratedHorizonPlanDto> AlignWithStrategicGoalsAsync(Guid planId, List<Guid> goalIds);
        Task<IntegratedHorizonPlanDto> AddFutureSelfDialogueAsync(Guid planId, string dialogue);

        // Plan Operations
        Task<IntegratedHorizonPlanDto> ActivateAsync(Guid id);
        Task<IntegratedHorizonPlanDto> DeactivateAsync(Guid id);

        // Reporting
        Task<ListResultDto<IntegratedHorizonPlanDto>> GetActivePlansAsync();
        Task<ListResultDto<IntegratedHorizonPlanDto>> GetPlansByHorizonAsync(PlanningHorizon horizon);
        Task<ListResultDto<TimelineEventMarkerDto>> GetEventsByHorizonAsync(Guid planId, PlanningHorizon horizon);
    }
}
