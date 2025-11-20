using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals
{
    /// <summary>
    /// Application service interface for Strategic Goal management
    /// </summary>
    public interface IStrategicGoalAppService : IApplicationService
    {
        Task<StrategicGoalDto> GetAsync(Guid id);
        Task<PagedResultDto<StrategicGoalDto>> GetListAsync(GetStrategicGoalListDto input);
        Task<StrategicGoalDto> CreateAsync(CreateStrategicGoalDto input);
        Task<StrategicGoalDto> UpdateAsync(Guid id, UpdateStrategicGoalDto input);
        Task DeleteAsync(Guid id);

        // Goal lifecycle management
        Task<StrategicGoalDto> SubmitForReviewAsync(Guid id);
        Task<StrategicGoalDto> ApproveAsync(Guid id);
        Task<StrategicGoalDto> ActivateAsync(Guid id);
        Task<StrategicGoalDto> CompleteAsync(Guid id);
        Task<StrategicGoalDto> PutOnHoldAsync(Guid id, string reason);
        Task<StrategicGoalDto> ResumeAsync(Guid id);

        // Target management
        Task<StrategicGoalDto> AddTargetAsync(Guid goalId, AddGoalTargetDto input);
        Task<StrategicGoalDto> UpdateTargetProgressAsync(Guid goalId, Guid targetId, UpdateTargetProgressDto input);

        // Tag management
        Task<StrategicGoalDto> AddTagAsync(Guid goalId, string tag);
        Task<StrategicGoalDto> RemoveTagAsync(Guid goalId, string tag);

        // Reporting
        Task<ListResultDto<StrategicGoalDto>> GetGoalsRequiringAttentionAsync();
        Task<ListResultDto<StrategicGoalDto>> GetGoalsByOwnerAsync(Guid ownerId);
    }
}
