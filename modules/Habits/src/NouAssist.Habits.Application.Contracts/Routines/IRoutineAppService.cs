using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Habits.Routines.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Habits.Routines
{
    public interface IRoutineAppService : IApplicationService
    {
        Task<RoutineDto> GetAsync(Guid id);
        Task<PagedResultDto<RoutineDto>> GetListAsync(GetRoutineListDto input);
        Task<RoutineDto> CreateAsync(CreateRoutineDto input);
        Task<RoutineDto> UpdateAsync(Guid id, UpdateRoutineDto input);
        Task DeleteAsync(Guid id);
        
        Task<RoutineDto> ActivateAsync(Guid id);
        Task<RoutineDto> DeactivateAsync(Guid id);
        
        Task<RoutineDto> AddStepAsync(Guid id, Guid? habitId, string adhocTaskDescription, int? estimatedDurationMinutes);
        Task<RoutineDto> RemoveStepAsync(Guid routineId, Guid stepId);
        Task<RoutineDto> ReorderStepsAsync(Guid id, Dictionary<Guid, int> newOrder);
        
        Task<RoutineCompletionDto> CompleteRoutineAsync(Guid routineId, CompleteRoutineDto input);
        Task<RoutineCompletionDto> GetCompletionAsync(Guid routineId, DateTime date);
        Task<double> GetCompletionRateAsync(Guid routineId, DateTime startDate, DateTime endDate);
    }
}
