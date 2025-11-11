using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using NouAssist.PersonalDevelopment.Habits.Permissions;
using NouAssist.PersonalDevelopment.Habits.Routines.Dtos;
using NouAssist.PersonalDevelopment.Habits.Services;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.Habits.Routines
{
    [Authorize(HabitsPermissions.Routines.Default)]
    public class RoutineAppService : ApplicationService, IRoutineAppService
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly IRoutineCompletionRepository _completionRepository;
        private readonly IHabitRepository _habitRepository;
        private readonly RoutineSchedulingService _schedulingService;

        public RoutineAppService(
            IRoutineRepository routineRepository,
            IRoutineCompletionRepository completionRepository,
            IHabitRepository habitRepository,
            RoutineSchedulingService schedulingService)
        {
            _routineRepository = routineRepository;
            _completionRepository = completionRepository;
            _habitRepository = habitRepository;
            _schedulingService = schedulingService;
        }

        public async Task<RoutineDto> GetAsync(Guid id)
        {
            var routine = await _routineRepository.GetWithStepsAsync(id);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to view this routine");
            }

            return await MapToGetOutputDtoAsync(routine);
        }

        public async Task<PagedResultDto<RoutineDto>> GetListAsync(GetRoutineListDto input)
        {
            var ownerId = CurrentUser.GetId();
            var allRoutines = await _routineRepository.GetActiveRoutinesAsync(ownerId);

            // Apply filters
            var filteredRoutines = allRoutines.AsQueryable();

            if (input.IsActive.HasValue)
            {
                filteredRoutines = filteredRoutines.Where(r => r.IsActive == input.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.TimeOfDay))
            {
                filteredRoutines = filteredRoutines.Where(r => r.TimeOfDay == input.TimeOfDay);
            }

            if (input.ScheduledForDate.HasValue)
            {
                filteredRoutines = filteredRoutines
                    .Where(r => _schedulingService.IsScheduledForDate(r, input.ScheduledForDate.Value));
            }

            var totalCount = filteredRoutines.Count();

            var pagedRoutines = filteredRoutines
                .OrderBy(r => r.TimeOfDay)
                .ThenBy(r => r.Title)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var routineDtos = new List<RoutineDto>();
            foreach (var routine in pagedRoutines)
            {
                routineDtos.Add(await MapToGetOutputDtoAsync(routine));
            }

            return new PagedResultDto<RoutineDto>(totalCount, routineDtos);
        }

        [Authorize(HabitsPermissions.Routines.Create)]
        public async Task<RoutineDto> CreateAsync(CreateRoutineDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var routine = new Routine(
                GuidGenerator.Create(),
                ownerId,
                input.Title,
                CurrentTenant.Id
            );

            routine.Description = input.Description;
            routine.TriggerDescription = input.TriggerDescription;
            routine.TimeOfDay = input.TimeOfDay;

            if (input.ScheduledDays?.Any() == true)
            {
                routine.UpdateSchedule(input.ScheduledDays);
            }

            await _routineRepository.InsertAsync(routine);

            return await MapToGetOutputDtoAsync(routine);
        }

        [Authorize(HabitsPermissions.Routines.Edit)]
        public async Task<RoutineDto> UpdateAsync(Guid id, UpdateRoutineDto input)
        {
            var routine = await _routineRepository.GetAsync(id);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this routine");
            }

            routine.UpdateTitle(input.Title);
            routine.Description = input.Description;
            routine.TriggerDescription = input.TriggerDescription;
            routine.TimeOfDay = input.TimeOfDay;

            if (input.ScheduledDays != null)
            {
                routine.UpdateSchedule(input.ScheduledDays);
            }

            await _routineRepository.UpdateAsync(routine);

            return await MapToGetOutputDtoAsync(routine);
        }

        [Authorize(HabitsPermissions.Routines.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var routine = await _routineRepository.GetAsync(id);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to delete this routine");
            }

            await _routineRepository.DeleteAsync(id);
        }

        [Authorize(HabitsPermissions.Routines.Edit)]
        public async Task<RoutineDto> ActivateAsync(Guid id)
        {
            var routine = await _routineRepository.GetAsync(id);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this routine");
            }

            routine.Activate();
            await _routineRepository.UpdateAsync(routine);

            return await MapToGetOutputDtoAsync(routine);
        }

        [Authorize(HabitsPermissions.Routines.Edit)]
        public async Task<RoutineDto> DeactivateAsync(Guid id)
        {
            var routine = await _routineRepository.GetAsync(id);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this routine");
            }

            routine.Deactivate();
            await _routineRepository.UpdateAsync(routine);

            return await MapToGetOutputDtoAsync(routine);
        }

        [Authorize(HabitsPermissions.Routines.Edit)]
        public async Task<RoutineDto> AddStepAsync(
            Guid id,
            Guid? habitId,
            string adhocTaskDescription,
            int? estimatedDurationMinutes)
        {
            var routine = await _routineRepository.GetWithStepsAsync(id);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this routine");
            }

            var stepNumber = routine.Steps.Any() ? routine.Steps.Max(s => s.StepNumber) + 1 : 1;

            routine.AddStep(
                GuidGenerator.Create(),
                stepNumber,
                habitId,
                adhocTaskDescription,
                estimatedDurationMinutes
            );

            await _routineRepository.UpdateAsync(routine);

            return await MapToGetOutputDtoAsync(routine);
        }

        [Authorize(HabitsPermissions.Routines.Edit)]
        public async Task<RoutineDto> RemoveStepAsync(Guid routineId, Guid stepId)
        {
            var routine = await _routineRepository.GetWithStepsAsync(routineId);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this routine");
            }

            routine.RemoveStep(stepId);
            await _routineRepository.UpdateAsync(routine);

            return await MapToGetOutputDtoAsync(routine);
        }

        [Authorize(HabitsPermissions.Routines.Edit)]
        public async Task<RoutineDto> ReorderStepsAsync(Guid id, Dictionary<Guid, int> newOrder)
        {
            var routine = await _routineRepository.GetWithStepsAsync(id);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this routine");
            }

            routine.ReorderSteps(newOrder);
            await _routineRepository.UpdateAsync(routine);

            return await MapToGetOutputDtoAsync(routine);
        }

        [Authorize(HabitsPermissions.Routines.Complete)]
        public async Task<RoutineCompletionDto> CompleteRoutineAsync(Guid routineId, CompleteRoutineDto input)
        {
            var routine = await _routineRepository.GetWithStepsAsync(routineId);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to complete this routine");
            }

            var ownerId = CurrentUser.GetId();
            
            var existingCompletion = await _completionRepository.GetByRoutineAndDateAsync(routineId, input.LogDate);
            
            RoutineCompletion completion;
            if (existingCompletion != null)
            {
                completion = existingCompletion;
            }
            else
            {
                completion = new RoutineCompletion(
                    GuidGenerator.Create(),
                    ownerId,
                    routineId,
                    input.LogDate,
                    routine.TotalSteps,
                    CurrentTenant.Id
                );
            }

            // Complete steps
            foreach (var stepId in input.CompletedStepIds)
            {
                completion.CompleteStep(stepId);
            }

            if (!string.IsNullOrWhiteSpace(input.Notes))
            {
                completion.AddNotes(input.Notes);
            }

            if (existingCompletion != null)
            {
                await _completionRepository.UpdateAsync(completion);
            }
            else
            {
                await _completionRepository.InsertAsync(completion);
            }

            return ObjectMapper.Map<RoutineCompletion, RoutineCompletionDto>(completion);
        }

        public async Task<RoutineCompletionDto> GetCompletionAsync(Guid routineId, DateTime date)
        {
            var routine = await _routineRepository.GetAsync(routineId);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to view this routine");
            }

            var completion = await _completionRepository.GetByRoutineAndDateAsync(routineId, date);
            
            if (completion == null)
            {
                return null;
            }

            return ObjectMapper.Map<RoutineCompletion, RoutineCompletionDto>(completion);
        }

        [Authorize(HabitsPermissions.Analytics.ViewOwn)]
        public async Task<double> GetCompletionRateAsync(Guid routineId, DateTime startDate, DateTime endDate)
        {
            var routine = await _routineRepository.GetAsync(routineId);
            
            if (routine.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to view this routine");
            }

            return await _completionRepository.GetCompletionRateAsync(routineId, startDate, endDate);
        }

        // Private helper methods
        private async Task<RoutineDto> MapToGetOutputDtoAsync(Routine routine)
        {
            var dto = ObjectMapper.Map<Routine, RoutineDto>(routine);
            dto.EstimatedDurationMinutes = _schedulingService.CalculateEstimatedDuration(routine);

            // Map habit titles for steps
            var habitIds = routine.Steps.Where(s => s.HabitId.HasValue).Select(s => s.HabitId.Value).ToList();
            if (habitIds.Any())
            {
                var habits = await _habitRepository.GetListAsync(new Specification<Habit>(h => habitIds.Contains(h.Id)));
                var habitDict = habits.ToDictionary(h => h.Id, h => h.Title);

                foreach (var step in dto.Steps.Where(s => s.HabitId.HasValue))
                {
                    if (habitDict.TryGetValue(step.HabitId.Value, out var title))
                    {
                        step.HabitTitle = title;
                    }
                }
            }

            return dto;
        }
    }
}
