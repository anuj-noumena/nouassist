using System;
using System.Threading.Tasks;
using NouAssist.CoreProductivity.HabitSystems.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.HabitSystems.Habits
{
    public interface IHabitAppService : IApplicationService
    {
        Task<HabitDto> GetAsync(Guid id);
        Task<PagedResultDto<HabitDto>> GetListAsync(GetHabitListDto input);
        Task<HabitDto> CreateAsync(CreateHabitDto input);
        Task<HabitDto> UpdateAsync(Guid id, UpdateHabitDto input);
        Task DeleteAsync(Guid id);
        
        Task<HabitDto> ActivateAsync(Guid id);
        Task<HabitDto> DeactivateAsync(Guid id);
        
        Task<HabitDto> AddCueAsync(Guid id, AddHabitCueDto input);
        Task<HabitDto> AddResponseAsync(Guid id, AddHabitResponseDto input);
        Task<HabitDto> AddRewardAsync(Guid id, AddHabitRewardDto input);
        Task<HabitDto> AddStrategyAsync(Guid id, AddHabitStrategyDto input);
        
        Task<HabitDto> RemoveCueAsync(Guid habitId, Guid cueId);
        
        Task<HabitTrackingDto> CompleteHabitAsync(Guid habitId, CompleteHabitDto input);
        Task<HabitTrackingDto> UncompleteHabitAsync(Guid habitId, DateTime logDate);
        Task<HabitMetricsDto> GetMetricsAsync(Guid habitId);
        Task<PagedResultDto<HabitTrackingDto>> GetTrackingHistoryAsync(
            Guid habitId, 
            DateTime? startDate, 
            DateTime? endDate,
            int skipCount = 0,
            int maxResultCount = 50);
    }
}
