using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Core.Enums;
using NouAssist.Core.ValueObjects;
using NouAssist.CoreProductivity.GoalTracking.Aggregates;
using NouAssist.CoreProductivity.GoalTracking.Data;
using NouAssist.CoreProductivity.GoalTracking.Dtos;
using NouAssist.CoreProductivity.GoalTracking.Entities;
using NouAssist.CoreProductivity.GoalTracking.Permissions;
using NouAssist.CoreProductivity.GoalTracking.Services;
using NouAssist.CoreProductivity.GoalTracking.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.CoreProductivity.GoalTracking.Goals
{
    [Authorize(GoalsPermissions.Goals.Default)]
    public class GoalAppService : ApplicationService, IGoalAppService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly GoalProgressCalculationService _progressCalculationService;

        public GoalAppService(
            IGoalRepository goalRepository,
            GoalProgressCalculationService progressCalculationService)
        {
            _goalRepository = goalRepository;
            _progressCalculationService = progressCalculationService;
        }

        public async Task<GoalDto> GetAsync(Guid id)
        {
            var goal = await _goalRepository.GetWithMilestonesAsync(id);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        public async Task<PagedResultDto<GoalDto>> GetListAsync(GetGoalListDto input)
        {
            var userId = CurrentUser.GetId();
            var spec = CreateListSpecification(userId, input);

            var goals = await _goalRepository.GetListAsync(spec);
            var totalCount = await _goalRepository.LongCountAsync(spec);

            var goalDtos = ObjectMapper.Map<List<Goal>, List<GoalDto>>(goals);

            return new PagedResultDto<GoalDto>(
                totalCount,
                goalDtos
            );
        }

        [Authorize(GoalsPermissions.Goals.Create)]
        public async Task<GoalDto> CreateAsync(CreateGoalDto input)
        {
            var userId = CurrentUser.GetId();
            
            var importance = input.ImportanceScore.HasValue 
                ? new ImportanceScore(input.ImportanceScore.Value) 
                : ImportanceScore.High();

            var urgency = input.UrgencyScore.HasValue 
                ? new UrgencyScore(input.UrgencyScore.Value) 
                : UrgencyScore.Low();

            var goal = new Goal(
                GuidGenerator.Create(),
                userId,
                input.Title,
                importance,
                urgency,
                CurrentTenant.Id
            );

            if (!string.IsNullOrWhiteSpace(input.Description))
                goal.Description = input.Description;

            if (input.AreaId.HasValue)
                goal.AreaId = input.AreaId;

            if (input.ParentGoalId.HasValue)
                goal.ParentGoalId = input.ParentGoalId;

            if (input.TargetDate.HasValue)
                goal.TargetDate = input.TargetDate;

            if (!string.IsNullOrWhiteSpace(input.SmartDescription))
                goal.SmartDescription = input.SmartDescription;

            await _goalRepository.InsertAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        [Authorize(GoalsPermissions.Goals.Update)]
        public async Task<GoalDto> UpdateAsync(Guid id, UpdateGoalDto input)
        {
            var goal = await _goalRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Title))
                goal.Title = input.Title;

            if (input.Description != null)
                goal.Description = input.Description;

            if (input.AreaId.HasValue)
                goal.AreaId = input.AreaId;

            if (input.TargetDate.HasValue)
                goal.TargetDate = input.TargetDate;

            if (!string.IsNullOrWhiteSpace(input.SmartDescription))
                goal.SmartDescription = input.SmartDescription;

            if (input.ImportanceScore.HasValue)
                goal.UpdateImportance(new ImportanceScore(input.ImportanceScore.Value));

            if (input.UrgencyScore.HasValue)
                goal.UpdateUrgency(new UrgencyScore(input.UrgencyScore.Value));

            await _goalRepository.UpdateAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        [Authorize(GoalsPermissions.Goals.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _goalRepository.DeleteAsync(id);
        }

        [Authorize(GoalsPermissions.Goals.UpdateProgress)]
        public async Task<GoalDto> UpdateProgressAsync(Guid id, UpdateGoalProgressDto input)
        {
            var goal = await _goalRepository.GetAsync(id);
            goal.UpdateProgress(input.Progress);
            await _goalRepository.UpdateAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        [Authorize(GoalsPermissions.Goals.Achieve)]
        public async Task<GoalDto> AchieveAsync(Guid id, AchieveGoalDto input)
        {
            var goal = await _goalRepository.GetAsync(id);
            goal.Achieve(input.CompletionNotes);
            await _goalRepository.UpdateAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        [Authorize(GoalsPermissions.Goals.Update)]
        public async Task<GoalDto> PauseAsync(Guid id)
        {
            var goal = await _goalRepository.GetAsync(id);
            goal.Pause();
            await _goalRepository.UpdateAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        [Authorize(GoalsPermissions.Goals.Update)]
        public async Task<GoalDto> ResumeAsync(Guid id)
        {
            var goal = await _goalRepository.GetAsync(id);
            goal.Resume();
            await _goalRepository.UpdateAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        [Authorize(GoalsPermissions.Goals.Archive)]
        public async Task<GoalDto> ArchiveAsync(Guid id, ArchiveGoalDto input)
        {
            var goal = await _goalRepository.GetAsync(id);
            goal.Archive(input.ArchiveReason);
            await _goalRepository.UpdateAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        [Authorize(GoalsPermissions.Goals.Update)]
        public async Task<GoalDto> AddMilestoneAsync(Guid goalId, AddGoalMilestoneDto input)
        {
            var goal = await _goalRepository.GetWithMilestonesAsync(goalId);
            
            var milestone = new GoalMilestone(
                GuidGenerator.Create(),
                goalId,
                input.Title
            );

            if (!string.IsNullOrWhiteSpace(input.Description))
                milestone.Description = input.Description;

            if (input.TargetDate.HasValue)
                milestone.TargetDate = input.TargetDate;

            milestone.OrderIndex = goal.Milestones.Count;

            goal.AddMilestone(milestone);
            await _goalRepository.UpdateAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        [Authorize(GoalsPermissions.Goals.Update)]
        public async Task RemoveMilestoneAsync(Guid goalId, Guid milestoneId)
        {
            var goal = await _goalRepository.GetWithMilestonesAsync(goalId);
            goal.RemoveMilestone(milestoneId);
            await _goalRepository.UpdateAsync(goal);
        }

        [Authorize(GoalsPermissions.Goals.Update)]
        public async Task<GoalDto> CompleteMilestoneAsync(Guid goalId, Guid milestoneId)
        {
            var goal = await _goalRepository.GetWithMilestonesAsync(goalId);
            var milestone = goal.Milestones.FirstOrDefault(m => m.Id == milestoneId);

            if (milestone == null)
            {
                throw new UserFriendlyException("Milestone not found.");
            }

            milestone.Complete();

            // Auto-update goal progress based on milestones
            var calculatedProgress = _progressCalculationService.CalculateProgressFromMilestones(goal);
            if (_progressCalculationService.ShouldAutoUpdateProgress(goal, calculatedProgress))
            {
                goal.UpdateProgress(calculatedProgress);
            }

            await _goalRepository.UpdateAsync(goal);
            return ObjectMapper.Map<Goal, GoalDto>(goal);
        }

        public async Task<GoalStatisticsDto> GetMyStatisticsAsync()
        {
            var userId = CurrentUser.GetId();
            var goals = await _goalRepository.GetByUserAsync(userId);

            return new GoalStatisticsDto
            {
                TotalGoals = goals.Count,
                ActiveGoals = goals.Count(g => g.IsActive),
                AchievedGoals = goals.Count(g => g.IsAchieved),
                OnHoldGoals = goals.Count(g => g.IsOnHold),
                OverdueGoals = goals.Count(g => g.IsOverdue),
                AverageProgress = goals.Any() ? goals.Average(g => g.Progress) : 0,
                CompletionRate = goals.Count > 0 
                    ? (double)goals.Count(g => g.IsAchieved) / goals.Count 
                    : 0
            };
        }

        private ISpecification<Goal> CreateListSpecification(Guid userId, GetGoalListDto input)
        {
            var spec = Specification.Create<Goal>(g => g.OwnerId == userId);

            if (input.AreaId.HasValue)
            {
                spec = spec.And(new GoalsByAreaSpecification(input.AreaId.Value));
            }

            if (input.Status.HasValue)
            {
                spec = spec.And(Specification.Create<Goal>(g => g.Status == input.Status.Value));
            }

            if (input.IsOverdue == true)
            {
                spec = spec.And(new OverdueGoalsSpecification());
            }

            if (input.FromDate.HasValue)
            {
                spec = spec.And(Specification.Create<Goal>(
                    g => g.TargetDate.HasValue && g.TargetDate.Value >= input.FromDate.Value));
            }

            if (input.ToDate.HasValue)
            {
                spec = spec.And(Specification.Create<Goal>(
                    g => g.TargetDate.HasValue && g.TargetDate.Value <= input.ToDate.Value));
            }

            return spec;
        }
    }
}
