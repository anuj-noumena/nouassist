using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using NouAssist.CoreProductivity.HabitSystems.Data;
using NouAssist.CoreProductivity.HabitSystems.Dtos;
using NouAssist.CoreProductivity.HabitSystems.Entities;
using NouAssist.CoreProductivity.HabitSystems.Permissions;
using NouAssist.CoreProductivity.HabitSystems.Services;
using NouAssist.CoreProductivity.HabitSystems.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.CoreProductivity.HabitSystems.Habits
{
    [Authorize(HabitsPermissions.Habits.Default)]
    public class HabitAppService : ApplicationService, IHabitAppService
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IHabitTrackingRepository _trackingRepository;
        private readonly HabitAnalyticsService _analyticsService;
        private readonly HabitTrackingService _trackingService;

        public HabitAppService(
            IHabitRepository habitRepository,
            IHabitTrackingRepository trackingRepository,
            HabitAnalyticsService analyticsService,
            HabitTrackingService trackingService)
        {
            _habitRepository = habitRepository;
            _trackingRepository = trackingRepository;
            _analyticsService = analyticsService;
            _trackingService = trackingService;
        }

        public async Task<HabitDto> GetAsync(Guid id)
        {
            var habit = await _habitRepository.GetWithDetailsAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to view this habit");
            }

            return await MapToGetOutputDtoAsync(habit);
        }

        public async Task<PagedResultDto<HabitDto>> GetListAsync(GetHabitListDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var specification = CreateListSpecification(input, ownerId);
            
            var habits = await _habitRepository.GetListAsync(specification);
            var totalCount = habits.Count;

            // Apply paging
            var pagedHabits = habits
                .OrderBy(GetSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var habitDtos = new List<HabitDto>();
            foreach (var habit in pagedHabits)
            {
                habitDtos.Add(await MapToGetOutputDtoAsync(habit));
            }

            return new PagedResultDto<HabitDto>(totalCount, habitDtos);
        }

        [Authorize(HabitsPermissions.Habits.Create)]
        public async Task<HabitDto> CreateAsync(CreateHabitDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var habit = new Habit(
                GuidGenerator.Create(),
                ownerId,
                input.Title,
                input.Goal,
                input.HabitType,
                CurrentTenant.Id,
                input.AreaId,
                input.ContextId
            );

            if (input.Description != null)
            {
                habit.Description = input.Description;
            }

            if (input.ScheduledDays?.Any() == true)
            {
                habit.UpdateSchedule(input.ScheduledDays);
            }

            await _habitRepository.InsertAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Edit)]
        public async Task<HabitDto> UpdateAsync(Guid id, UpdateHabitDto input)
        {
            var habit = await _habitRepository.GetAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this habit");
            }

            habit.UpdateTitle(input.Title);
            habit.UpdateGoal(input.Goal);
            habit.Description = input.Description;
            habit.AssignToArea(input.AreaId);
            habit.AssignToContext(input.ContextId);

            if (input.ScheduledDays != null)
            {
                habit.UpdateSchedule(input.ScheduledDays);
            }

            await _habitRepository.UpdateAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var habit = await _habitRepository.GetAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to delete this habit");
            }

            await _habitRepository.DeleteAsync(id);
        }

        [Authorize(HabitsPermissions.Habits.Edit)]
        public async Task<HabitDto> ActivateAsync(Guid id)
        {
            var habit = await _habitRepository.GetAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this habit");
            }

            habit.Activate();
            await _habitRepository.UpdateAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Edit)]
        public async Task<HabitDto> DeactivateAsync(Guid id)
        {
            var habit = await _habitRepository.GetAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this habit");
            }

            habit.Deactivate();
            await _habitRepository.UpdateAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Edit)]
        public async Task<HabitDto> AddCueAsync(Guid id, AddHabitCueDto input)
        {
            var habit = await _habitRepository.GetWithDetailsAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this habit");
            }

            habit.AddCue(GuidGenerator.Create(), input.CueType, input.Description, input.CueValue);
            await _habitRepository.UpdateAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Edit)]
        public async Task<HabitDto> AddResponseAsync(Guid id, AddHabitResponseDto input)
        {
            var habit = await _habitRepository.GetWithDetailsAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this habit");
            }

            habit.AddResponse(GuidGenerator.Create(), input.Description, input.Details, input.DurationMinutes);
            await _habitRepository.UpdateAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Edit)]
        public async Task<HabitDto> AddRewardAsync(Guid id, AddHabitRewardDto input)
        {
            var habit = await _habitRepository.GetWithDetailsAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this habit");
            }

            habit.AddReward(GuidGenerator.Create(), input.Description, input.Craving);
            await _habitRepository.UpdateAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Edit)]
        public async Task<HabitDto> AddStrategyAsync(Guid id, AddHabitStrategyDto input)
        {
            var habit = await _habitRepository.GetWithDetailsAsync(id);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this habit");
            }

            habit.AddStrategy(GuidGenerator.Create(), input.StrategyType, input.Description);
            await _habitRepository.UpdateAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Edit)]
        public async Task<HabitDto> RemoveCueAsync(Guid habitId, Guid cueId)
        {
            var habit = await _habitRepository.GetWithDetailsAsync(habitId);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to edit this habit");
            }

            habit.RemoveCue(cueId);
            await _habitRepository.UpdateAsync(habit);

            return await MapToGetOutputDtoAsync(habit);
        }

        [Authorize(HabitsPermissions.Habits.Track)]
        public async Task<HabitTrackingDto> CompleteHabitAsync(Guid habitId, CompleteHabitDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var entry = await _trackingService.CompleteHabitAsync(
                habitId,
                input.LogDate,
                ownerId,
                input.Notes
            );

            return ObjectMapper.Map<HabitTrackingEntry, HabitTrackingDto>(entry);
        }

        [Authorize(HabitsPermissions.Habits.Track)]
        public async Task<HabitTrackingDto> UncompleteHabitAsync(Guid habitId, DateTime logDate)
        {
            var ownerId = CurrentUser.GetId();
            
            var entry = await _trackingRepository.GetByHabitAndDateAsync(habitId, logDate);
            if (entry == null || entry.OwnerId != ownerId)
            {
                throw new BusinessException("Habits:TrackingNotFound", "Tracking entry not found");
            }

            entry.MarkAsIncomplete();
            await _trackingRepository.UpdateAsync(entry);

            return ObjectMapper.Map<HabitTrackingEntry, HabitTrackingDto>(entry);
        }

        [Authorize(HabitsPermissions.Analytics.ViewOwn)]
        public async Task<HabitMetricsDto> GetMetricsAsync(Guid habitId)
        {
            var habit = await _habitRepository.GetAsync(habitId);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to view this habit");
            }

            var trackingHistory = await _trackingRepository.GetHistoryAsync(habitId);
            var metrics = _analyticsService.CalculateMetrics(habit, trackingHistory);

            return ObjectMapper.Map<HabitMetrics, HabitMetricsDto>(metrics);
        }

        public async Task<PagedResultDto<HabitTrackingDto>> GetTrackingHistoryAsync(
            Guid habitId,
            DateTime? startDate,
            DateTime? endDate,
            int skipCount = 0,
            int maxResultCount = 50)
        {
            var habit = await _habitRepository.GetAsync(habitId);
            
            if (habit.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Habits:NotOwner", "You don't have permission to view this habit");
            }

            var history = await _trackingRepository.GetHistoryAsync(habitId, startDate, endDate);
            var totalCount = history.Count;

            var pagedHistory = history
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToList();

            var dtos = ObjectMapper.Map<List<HabitTrackingEntry>, List<HabitTrackingDto>>(pagedHistory);

            return new PagedResultDto<HabitTrackingDto>(totalCount, dtos);
        }

        // Private helper methods
        private async Task<HabitDto> MapToGetOutputDtoAsync(Habit habit)
        {
            var dto = ObjectMapper.Map<Habit, HabitDto>(habit);
            return dto;
        }

        private ISpecification<Habit> CreateListSpecification(GetHabitListDto input, Guid ownerId)
        {
            var spec = Specification<Habit>.True;

            spec = spec.And(new Specification<Habit>(h => h.OwnerId == ownerId));

            if (input.HabitType.HasValue)
            {
                spec = spec.And(new HabitsByTypeSpecification(input.HabitType.Value));
            }

            if (input.IsActive.HasValue)
            {
                if (input.IsActive.Value)
                {
                    spec = spec.And(new ActiveHabitsSpecification());
                }
                else
                {
                    spec = spec.And(new Specification<Habit>(h => !h.IsActive));
                }
            }

            if (input.AreaId.HasValue)
            {
                spec = spec.And(new HabitsByAreaSpecification(input.AreaId.Value));
            }

            if (input.ContextId.HasValue)
            {
                spec = spec.And(new Specification<Habit>(h => h.ContextId == input.ContextId.Value));
            }

            if (input.ScheduledForDate.HasValue)
            {
                spec = spec.And(new HabitsScheduledForDaySpecification(input.ScheduledForDate.Value));
            }

            return spec;
        }

        private static Func<Habit, object> GetSorting(string sorting)
        {
            return sorting?.ToLowerInvariant() switch
            {
                "title" => h => h.Title,
                "createdat" => h => h.CreationTime,
                _ => h => h.Title
            };
        }
    }
}
