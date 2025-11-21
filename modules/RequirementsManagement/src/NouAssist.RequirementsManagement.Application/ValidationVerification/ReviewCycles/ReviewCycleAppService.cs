using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using NouAssist.RequirementsManagement.ValidationVerification.Services;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.Localization;
using NouAssist.RequirementsManagement.ValidationVerification.Permissions;
using NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles
{
    /// <summary>
    /// Application service for managing requirement review cycles
    /// </summary>
    [Authorize(ValidationVerificationPermissions.ReviewCycles.Default)]
    public class ReviewCycleAppService : ApplicationService, IReviewCycleAppService
    {
        private readonly IRepository<RequirementReviewCycle, Guid> _reviewCycleRepository;
        private readonly IReviewWorkflowManager _reviewWorkflowManager;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public ReviewCycleAppService(
            IRepository<RequirementReviewCycle, Guid> reviewCycleRepository,
            IReviewWorkflowManager reviewWorkflowManager,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _reviewCycleRepository = reviewCycleRepository;
            _reviewWorkflowManager = reviewWorkflowManager;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(ValidationVerificationResource);
        }

        public virtual async Task<ReviewCycleDto> GetAsync(Guid id)
        {
            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(id);
                
                // Check if current user can view this review cycle
                await CheckViewPermissionAsync(reviewCycle);

                var dto = ObjectMapper.Map<RequirementReviewCycle, ReviewCycleDto>(reviewCycle);
                
                // Set completion status for reviewer assignments
                foreach (var assignment in dto.ReviewerAssignments)
                {
                    assignment.IsCompleted = dto.FeedbackItems.Any(f => f.ReviewerId == assignment.ReviewerId);
                }

                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        public virtual async Task<PagedResultDto<ReviewCycleSummaryDto>> GetListAsync(GetReviewCycleListDto input)
        {
            try
            {
                var queryable = await _reviewCycleRepository.GetQueryableAsync();

                // Apply filters
                queryable = ApplyFilters(queryable, input);

                // Apply authorization filters
                if (!await AuthorizationService.IsGrantedAsync(ValidationVerificationPermissions.ReviewCycles.ViewAll))
                {
                    // Only show review cycles where user is a reviewer
                    var currentUserId = _currentUser.Id;
                    if (currentUserId.HasValue)
                    {
                        queryable = queryable.Where(rc => 
                            rc.ReviewerAssignments.Any(ra => ra.ReviewerId == currentUserId.Value));
                    }
                    else
                    {
                        // Anonymous user shouldn't see any review cycles
                        return new PagedResultDto<ReviewCycleSummaryDto>(0, new List<ReviewCycleSummaryDto>());
                    }
                }

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(RequirementReviewCycle.CreationTime) + " desc")
                        .PageBy(input)
                );

                return new PagedResultDto<ReviewCycleSummaryDto>(
                    totalCount,
                    ObjectMapper.Map<List<RequirementReviewCycle>, List<ReviewCycleSummaryDto>>(entities)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(ValidationVerificationPermissions.ReviewCycles.Create)]
        public virtual async Task<ReviewCycleDto> CreateAsync(CreateReviewCycleDto input)
        {
            try
            {
                var currentTime = _clock.Now;
                var currentUserId = _currentUser.Id;
                var currentTenantId = _currentTenant.Id;

                // Create workflow definition
                var workflowDefinition = new WorkflowDefinitionRef(
                    input.WorkflowDefinition.DefinitionId,
                    input.WorkflowDefinition.Name,
                    input.WorkflowDefinition.Version
                );

                // Create reviewer assignments
                var reviewerAssignments = input.ReviewerAssignments.Select(ra =>
                    new ReviewerAssignment(
                        ra.ReviewerId,
                        ra.ReviewerName,
                        ra.ReviewerEmail,
                        currentTime,
                        ra.DueDate,
                        ra.IsRequired
                    )).ToList();

                // Use domain service to create review cycle
                var reviewCycle = await _reviewWorkflowManager.CreateReviewCycleAsync(
                    input.Name,
                    input.Description,
                    input.RequirementId,
                    workflowDefinition,
                    reviewerAssignments,
                    input.TargetCompletionDate,
                    currentTenantId
                );

                return ObjectMapper.Map<RequirementReviewCycle, ReviewCycleDto>(reviewCycle);
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

        [Authorize(ValidationVerificationPermissions.ReviewCycles.Update)]
        public virtual async Task<ReviewCycleDto> UpdateAsync(Guid id, UpdateReviewCycleDto input)
        {
            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(id);

                // Check authorization
                await CheckManagePermissionAsync(reviewCycle);

                // Update properties
                reviewCycle.UpdateName(input.Name);
                reviewCycle.UpdateDescription(input.Description);
                reviewCycle.UpdateTargetCompletionDate(input.TargetCompletionDate);

                await _reviewCycleRepository.UpdateAsync(reviewCycle);

                return ObjectMapper.Map<RequirementReviewCycle, ReviewCycleDto>(reviewCycle);
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

        [Authorize(ValidationVerificationPermissions.ReviewCycles.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(id);

                // Check authorization
                await CheckManagePermissionAsync(reviewCycle);

                // Only allow deletion of draft cycles
                if (reviewCycle.Status != ReviewCycleStatus.Draft)
                {
                    throw new UserFriendlyException(L["ValidationVerification:ErrorMessage:CannotDeleteStartedReview"]);
                }

                await _reviewCycleRepository.DeleteAsync(id);
            }
            catch (UserFriendlyException) { throw; }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.ReviewCycles.Start)]
        public virtual async Task<ReviewCycleDto> StartAsync(Guid id)
        {
            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(id);

                // Check authorization
                await CheckManagePermissionAsync(reviewCycle);

                var currentTime = _clock.Now;

                // Use domain service to start review cycle
                await _reviewWorkflowManager.StartReviewCycleAsync(id, currentTime);

                // Reload the updated entity
                await _reviewCycleRepository.GetAsync(id);

                return ObjectMapper.Map<RequirementReviewCycle, ReviewCycleDto>(reviewCycle);
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

        [Authorize(ValidationVerificationPermissions.ReviewCycles.SubmitReview)]
        public virtual async Task<ReviewCycleDto> SubmitReviewDecisionAsync(Guid id, SubmitReviewDecisionDto input)
        {
            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(id);
                var currentUserId = _currentUser.Id;
                var currentTime = _clock.Now;

                if (!currentUserId.HasValue)
                {
                    throw new UserFriendlyException(L["ValidationVerification:ErrorMessage:UserNotAuthenticated"]);
                }

                // Check if user is assigned as a reviewer
                if (!reviewCycle.ReviewerAssignments.Any(ra => ra.ReviewerId == currentUserId.Value))
                {
                    throw new UserFriendlyException(L["ValidationVerification:ErrorMessage:ReviewerNotAssigned"]);
                }

                // Use domain service to submit review decision
                await _reviewWorkflowManager.SubmitReviewDecisionAsync(
                    id,
                    currentUserId.Value,
                    input.DecisionType,
                    input.Reason,
                    input.Comment,
                    currentTime
                );

                // Reload the updated entity
                await _reviewCycleRepository.GetAsync(id);

                return ObjectMapper.Map<RequirementReviewCycle, ReviewCycleDto>(reviewCycle);
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

        [Authorize(ValidationVerificationPermissions.ReviewCycles.Cancel)]
        public virtual async Task<ReviewCycleDto> CancelAsync(Guid id, string reason)
        {
            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(id);

                // Check authorization
                await CheckManagePermissionAsync(reviewCycle);

                var currentTime = _clock.Now;

                // Use domain service to cancel review cycle
                await _reviewWorkflowManager.CancelReviewCycleAsync(id, reason, currentTime);

                // Reload the updated entity
                await _reviewCycleRepository.GetAsync(id);

                return ObjectMapper.Map<RequirementReviewCycle, ReviewCycleDto>(reviewCycle);
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

        [Authorize(ValidationVerificationPermissions.ReviewCycles.ManageReviewers)]
        public virtual async Task<ReviewCycleDto> UpdateReviewerAssignmentsAsync(
            Guid id, UpdateReviewerAssignmentsDto input)
        {
            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(id);

                // Check authorization
                await CheckManagePermissionAsync(reviewCycle);

                var currentTime = _clock.Now;

                // Create reviewer assignments
                var reviewerAssignments = input.ReviewerAssignments.Select(ra =>
                    new ReviewerAssignment(
                        ra.ReviewerId,
                        ra.ReviewerName,
                        ra.ReviewerEmail,
                        currentTime,
                        ra.DueDate,
                        ra.IsRequired
                    )).ToList();

                // Use domain service to update reviewer assignments
                await _reviewWorkflowManager.UpdateReviewerAssignmentsAsync(id, reviewerAssignments);

                // Reload the updated entity
                await _reviewCycleRepository.GetAsync(id);

                return ObjectMapper.Map<RequirementReviewCycle, ReviewCycleDto>(reviewCycle);
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

        public virtual async Task<PagedResultDto<ReviewCycleSummaryDto>> GetReviewCyclesForReviewerAsync(
            Guid reviewerId, GetReviewCycleListDto input)
        {
            try
            {
                var queryable = await _reviewCycleRepository.GetQueryableAsync();

                // Filter by reviewer
                queryable = queryable.Where(rc => 
                    rc.ReviewerAssignments.Any(ra => ra.ReviewerId == reviewerId));

                // Apply other filters
                queryable = ApplyFilters(queryable, input);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(RequirementReviewCycle.CreationTime) + " desc")
                        .PageBy(input)
                );

                return new PagedResultDto<ReviewCycleSummaryDto>(
                    totalCount,
                    ObjectMapper.Map<List<RequirementReviewCycle>, List<ReviewCycleSummaryDto>>(entities)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        public virtual async Task<PagedResultDto<ReviewCycleSummaryDto>> GetOverdueReviewCyclesAsync(
            GetReviewCycleListDto input)
        {
            try
            {
                var currentTime = _clock.Now;
                var queryable = await _reviewCycleRepository.GetQueryableAsync();

                // Filter overdue cycles
                queryable = queryable.Where(rc => 
                    rc.Status == ReviewCycleStatus.InProgress &&
                    rc.TargetCompletionDate.HasValue &&
                    rc.TargetCompletionDate.Value < currentTime);

                // Apply other filters
                queryable = ApplyFilters(queryable, input);

                // Apply authorization filters
                if (!await AuthorizationService.IsGrantedAsync(ValidationVerificationPermissions.ReviewCycles.ViewAll))
                {
                    var currentUserId = _currentUser.Id;
                    if (currentUserId.HasValue)
                    {
                        queryable = queryable.Where(rc => 
                            rc.ReviewerAssignments.Any(ra => ra.ReviewerId == currentUserId.Value));
                    }
                    else
                    {
                        return new PagedResultDto<ReviewCycleSummaryDto>(0, new List<ReviewCycleSummaryDto>());
                    }
                }

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(RequirementReviewCycle.TargetCompletionDate))
                        .PageBy(input)
                );

                return new PagedResultDto<ReviewCycleSummaryDto>(
                    totalCount,
                    ObjectMapper.Map<List<RequirementReviewCycle>, List<ReviewCycleSummaryDto>>(entities)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        public virtual async Task<PagedResultDto<ReviewCycleSummaryDto>> GetReviewCyclesForRequirementAsync(
            Guid requirementId, GetReviewCycleListDto input)
        {
            try
            {
                var queryable = await _reviewCycleRepository.GetQueryableAsync();

                // Filter by requirement
                queryable = queryable.Where(rc => rc.RequirementId == requirementId);

                // Apply other filters
                queryable = ApplyFilters(queryable, input);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(RequirementReviewCycle.CreationTime) + " desc")
                        .PageBy(input)
                );

                return new PagedResultDto<ReviewCycleSummaryDto>(
                    totalCount,
                    ObjectMapper.Map<List<RequirementReviewCycle>, List<ReviewCycleSummaryDto>>(entities)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        // Helper methods

        private IQueryable<RequirementReviewCycle> ApplyFilters(
            IQueryable<RequirementReviewCycle> queryable, 
            GetReviewCycleListDto input)
        {
            if (!input.Filter.IsNullOrWhiteSpace())
            {
                queryable = queryable.Where(rc => 
                    rc.Name.Contains(input.Filter) || rc.Description.Contains(input.Filter));
            }

            if (input.RequirementId.HasValue)
            {
                queryable = queryable.Where(rc => rc.RequirementId == input.RequirementId.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(rc => rc.Status == input.Status.Value);
            }

            if (input.ReviewerId.HasValue)
            {
                queryable = queryable.Where(rc => 
                    rc.ReviewerAssignments.Any(ra => ra.ReviewerId == input.ReviewerId.Value));
            }

            if (input.IsOverdue.HasValue)
            {
                var currentTime = _clock.Now;
                if (input.IsOverdue.Value)
                {
                    queryable = queryable.Where(rc => 
                        rc.Status == ReviewCycleStatus.InProgress &&
                        rc.TargetCompletionDate.HasValue &&
                        rc.TargetCompletionDate.Value < currentTime);
                }
                else
                {
                    queryable = queryable.Where(rc => 
                        rc.Status != ReviewCycleStatus.InProgress ||
                        !rc.TargetCompletionDate.HasValue ||
                        rc.TargetCompletionDate.Value >= currentTime);
                }
            }

            if (input.CreatedAfter.HasValue)
            {
                queryable = queryable.Where(rc => rc.CreationTime >= input.CreatedAfter.Value);
            }

            if (input.CreatedBefore.HasValue)
            {
                queryable = queryable.Where(rc => rc.CreationTime <= input.CreatedBefore.Value);
            }

            return queryable;
        }

        private async Task CheckViewPermissionAsync(RequirementReviewCycle reviewCycle)
        {
            if (await AuthorizationService.IsGrantedAsync(ValidationVerificationPermissions.ReviewCycles.ViewAll))
            {
                return; // User can view all review cycles
            }

            // Check if user is assigned as a reviewer
            var currentUserId = _currentUser.Id;
            if (currentUserId.HasValue && 
                reviewCycle.ReviewerAssignments.Any(ra => ra.ReviewerId == currentUserId.Value))
            {
                return; // User is a reviewer for this cycle
            }

            throw new AbpAuthorizationException(L["ValidationVerification:ErrorMessage:AccessDenied"]);
        }

        private async Task CheckManagePermissionAsync(RequirementReviewCycle reviewCycle)
        {
            // Check if user has management permission OR is the creator
            if (await AuthorizationService.IsGrantedAsync(ValidationVerificationPermissions.ReviewCycles.Update) ||
                reviewCycle.CreatorId == _currentUser.Id)
            {
                return;
            }

            throw new AbpAuthorizationException(L["ValidationVerification:ErrorMessage:AccessDenied"]);
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in ReviewCycleAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
