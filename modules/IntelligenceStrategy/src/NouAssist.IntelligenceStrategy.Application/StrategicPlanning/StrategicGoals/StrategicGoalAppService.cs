using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Permissions;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Services;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Specifications;
using NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals
{
    /// <summary>
    /// Application service for Strategic Goal management
    /// </summary>
    [Authorize(StrategicPlanningPermissions.StrategicGoals.Default)]
    public class StrategicGoalAppService : ApplicationService, IStrategicGoalAppService
    {
        private readonly IStrategicGoalRepository _strategicGoalRepository;
        private readonly IStrategicGoalManager _strategicGoalManager;
        private readonly ICurrentUser _currentUser;

        public StrategicGoalAppService(
            IStrategicGoalRepository strategicGoalRepository,
            IStrategicGoalManager strategicGoalManager,
            ICurrentUser currentUser)
        {
            _strategicGoalRepository = strategicGoalRepository;
            _strategicGoalManager = strategicGoalManager;
            _currentUser = currentUser;
            
            LocalizationResource = typeof(StrategicPlanningResource);
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.View)]
        public virtual async Task<StrategicGoalDto> GetAsync(Guid id)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id, includeDetails: true);
                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["StrategicPlanning:ErrorMessage:GoalNotFound"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.View)]
        public virtual async Task<PagedResultDto<StrategicGoalDto>> GetListAsync(GetStrategicGoalListDto input)
        {
            try
            {
                var queryable = await _strategicGoalRepository.GetQueryableAsync();

                // Apply filters
                queryable = queryable.WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    g => g.Description.Title.Contains(input.Filter) || 
                         g.Description.Description.Contains(input.Filter));

                queryable = queryable.WhereIf(
                    input.Status.HasValue,
                    g => g.Status == input.Status.Value);

                queryable = queryable.WhereIf(
                    input.Priority.HasValue,
                    g => g.Priority == input.Priority.Value);

                queryable = queryable.WhereIf(
                    input.Horizon.HasValue,
                    g => g.Horizon == input.Horizon.Value);

                queryable = queryable.WhereIf(
                    input.OwnerId.HasValue,
                    g => g.OwnerId == input.OwnerId.Value);

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);

                // Apply sorting and paging
                var goals = await AsyncExecuter.ToListAsync(
                    queryable
                        .OrderBy(input.Sorting ?? "Priority desc, TargetDate")
                        .PageBy(input)
                );

                return new PagedResultDto<StrategicGoalDto>(
                    totalCount,
                    ObjectMapper.Map<List<StrategicGoalDefinition>, List<StrategicGoalDto>>(goals)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Create)]
        public virtual async Task<StrategicGoalDto> CreateAsync(CreateStrategicGoalDto input)
        {
            try
            {
                var goal = await _strategicGoalManager.CreateAsync(
                    input.Title,
                    input.Description,
                    input.Priority,
                    input.Horizon,
                    input.TargetDate,
                    input.OwnerId ?? _currentUser.Id
                );

                if (!input.Notes.IsNullOrEmpty())
                {
                    goal.UpdateNotes(input.Notes);
                }

                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Update)]
        public virtual async Task<StrategicGoalDto> UpdateAsync(Guid id, UpdateStrategicGoalDto input)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id);

                // Check ownership or permissions
                await CheckGoalAccessAsync(goal);

                // Update goal properties (would need methods in domain aggregate)
                // For this example, assuming we have update methods
                goal.UpdatePriority(input.Priority);
                if (input.OwnerId.HasValue)
                {
                    goal.AssignOwner(input.OwnerId.Value);
                }
                if (!input.Notes.IsNullOrEmpty())
                {
                    goal.UpdateNotes(input.Notes);
                }

                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id);
                await CheckGoalAccessAsync(goal);
                await _strategicGoalRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Update)]
        public virtual async Task<StrategicGoalDto> SubmitForReviewAsync(Guid id)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id);
                await CheckGoalAccessAsync(goal);

                goal.SubmitForReview();
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Approve)]
        public virtual async Task<StrategicGoalDto> ApproveAsync(Guid id)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id);
                
                await _strategicGoalManager.ValidateGoalTargetsAsync(goal);
                goal.Approve();
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Activate)]
        public virtual async Task<StrategicGoalDto> ActivateAsync(Guid id)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id);
                
                goal.Activate(Clock.Now);
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Update)]
        public virtual async Task<StrategicGoalDto> CompleteAsync(Guid id)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id);
                await CheckGoalAccessAsync(goal);

                goal.Complete(Clock.Now);
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Update)]
        public virtual async Task<StrategicGoalDto> PutOnHoldAsync(Guid id, string reason)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id);
                await CheckGoalAccessAsync(goal);

                goal.PutOnHold(reason);
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Update)]
        public virtual async Task<StrategicGoalDto> ResumeAsync(Guid id)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(id);
                await CheckGoalAccessAsync(goal);

                goal.Resume();
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.ManageTargets)]
        public virtual async Task<StrategicGoalDto> AddTargetAsync(Guid goalId, AddGoalTargetDto input)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(goalId);
                await CheckGoalAccessAsync(goal);

                goal.AddTarget(input.MetricName, input.TargetValue, input.Unit, input.TargetDate, input.Notes);
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.ManageTargets)]
        public virtual async Task<StrategicGoalDto> UpdateTargetProgressAsync(Guid goalId, Guid targetId, UpdateTargetProgressDto input)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(goalId);
                await CheckGoalAccessAsync(goal);

                goal.UpdateTargetProgress(targetId, input.CurrentValue, input.MeasurementDate);
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Update)]
        public virtual async Task<StrategicGoalDto> AddTagAsync(Guid goalId, string tag)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(goalId);
                await CheckGoalAccessAsync(goal);

                goal.AddTag(tag);
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.Update)]
        public virtual async Task<StrategicGoalDto> RemoveTagAsync(Guid goalId, string tag)
        {
            try
            {
                var goal = await _strategicGoalRepository.GetAsync(goalId);
                await CheckGoalAccessAsync(goal);

                // Would need RemoveTag method in domain aggregate
                goal.Tags.Remove(tag.ToLowerInvariant());
                await _strategicGoalRepository.UpdateAsync(goal);

                return ObjectMapper.Map<StrategicGoalDefinition, StrategicGoalDto>(goal);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.View)]
        public virtual async Task<ListResultDto<StrategicGoalDto>> GetGoalsRequiringAttentionAsync()
        {
            try
            {
                var goals = await _strategicGoalManager.GetGoalsRequiringAttentionAsync();
                return new ListResultDto<StrategicGoalDto>(
                    ObjectMapper.Map<List<StrategicGoalDefinition>, List<StrategicGoalDto>>(goals)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.StrategicGoals.View)]
        public virtual async Task<ListResultDto<StrategicGoalDto>> GetGoalsByOwnerAsync(Guid ownerId)
        {
            try
            {
                var goals = await _strategicGoalRepository.GetGoalsByOwnerAsync(ownerId);
                return new ListResultDto<StrategicGoalDto>(
                    ObjectMapper.Map<List<StrategicGoalDefinition>, List<StrategicGoalDto>>(goals)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        private async Task CheckGoalAccessAsync(StrategicGoalDefinition goal)
        {
            // Check if user is owner or has administrative permissions
            if (goal.OwnerId != _currentUser.Id && 
                !await AuthorizationService.IsGrantedAsync(StrategicPlanningPermissions.StrategicGoals.Update))
            {
                throw new AbpAuthorizationException("Insufficient permissions to access this goal");
            }
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in {AppServiceName}: {ErrorMessage}", 
                GetType().FullName, exception.Message);
            await Task.CompletedTask;
        }
    }
}
