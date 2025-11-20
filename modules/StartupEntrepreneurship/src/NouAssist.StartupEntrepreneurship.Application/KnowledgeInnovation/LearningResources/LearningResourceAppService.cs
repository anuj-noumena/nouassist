using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Permissions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Services;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources
{
    [Authorize(KnowledgeInnovationPermissions.LearningResource.Default)]
    public class LearningResourceAppService : ApplicationService, ILearningResourceAppService
    {
        private readonly ILearningResourceRepository _learningResourceRepository;
        private readonly ILearningManagementService _learningManagementService;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;

        public LearningResourceAppService(
            ILearningResourceRepository learningResourceRepository,
            ILearningManagementService learningManagementService,
            ICurrentUser currentUser,
            IClock clock)
        {
            _learningResourceRepository = learningResourceRepository;
            _learningManagementService = learningManagementService;
            _currentUser = currentUser;
            _clock = clock;
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<LearningResourceDto> GetAsync(Guid id)
        {
            try
            {
                var learningResource = await _learningResourceRepository.GetAsync(id);
                var learningResourceDto = ObjectMapper.Map<LearningResource, LearningResourceDto>(learningResource);

                // If user is logged in, add user progress data
                if (_currentUser.Id.HasValue)
                {
                    var userProgress = learningResource.GetUserProgress(_currentUser.Id.Value);
                    if (userProgress != null)
                    {
                        learningResourceDto.UserProgress = ObjectMapper.Map<UserProgress, UserProgressDto>(userProgress);
                    }
                }

                return learningResourceDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<PagedResultDto<LearningResourceListDto>> GetListAsync(GetLearningResourceListDto input)
        {
            try
            {
                var query = await _learningResourceRepository.GetQueryableAsync();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    query = query.Where(x => x.Title.Contains(input.Filter));
                }

                if (input.ResourceType.HasValue)
                {
                    query = query.Where(x => x.ResourceType == input.ResourceType.Value);
                }

                if (!string.IsNullOrWhiteSpace(input.Tag))
                {
                    query = query.Where(x => x.Tags.Contains(input.Tag));
                }

                if (input.OnlyPublished == true)
                {
                    query = query.Where(x => x.IsPublished);
                }

                if (input.OnlyFeatured == true)
                {
                    query = query.Where(x => x.IsPublished && x.IsFeatured);
                }

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);

                // Apply sorting
                query = query.OrderBy(input.Sorting ?? nameof(LearningResource.Title));

                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

                // Execute query and map results
                var learningResources = await AsyncExecuter.ToListAsync(query);
                var learningResourceDtos = ObjectMapper.Map<List<LearningResource>, List<LearningResourceListDto>>(learningResources);

                // Add user completion status if user is logged in
                if (_currentUser.Id.HasValue)
                {
                    foreach (var dto in learningResourceDtos)
                    {
                        var resource = learningResources.First(r => r.Id == dto.Id);
                        var userProgress = resource.GetUserProgress(_currentUser.Id.Value);
                        
                        if (userProgress != null)
                        {
                            dto.UserCompletionStatus = userProgress.Status;
                        }
                    }
                }

                // Filter by completion status if specified
                if (input.CompletionStatus.HasValue && _currentUser.Id.HasValue)
                {
                    learningResourceDtos = learningResourceDtos
                        .Where(r => r.UserCompletionStatus == input.CompletionStatus.Value)
                        .ToList();
                }

                return new PagedResultDto<LearningResourceListDto>(totalCount, learningResourceDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.Create)]
        public virtual async Task<LearningResourceDto> CreateAsync(CreateLearningResourceDto input)
        {
            try
            {
                var currentUserId = _currentUser.GetId();
                
                // Create learning module description value object
                var description = new LearningModuleDescription(
                    input.Description.Overview,
                    input.Description.LearningObjectives,
                    input.Description.PrerequisiteKnowledge,
                    input.Description.EstimatedDurationMinutes
                );

                // Create learning resource
                var learningResource = LearningResource.Create(
                    GuidGenerator.Create(),
                    input.Title,
                    input.ResourceType,
                    input.ResourceUrl,
                    description,
                    input.Tags,
                    input.IsPublished,
                    input.IsFeatured,
                    CurrentTenant.Id
                );

                await _learningResourceRepository.InsertAsync(learningResource, autoSave: true);

                return ObjectMapper.Map<LearningResource, LearningResourceDto>(learningResource);
            }
            catch (BusinessRuleViolationException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code, ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.Update)]
        public virtual async Task<LearningResourceDto> UpdateAsync(Guid id, UpdateLearningResourceDto input)
        {
            try
            {
                var learningResource = await _learningResourceRepository.GetAsync(id);

                // Create learning module description value object
                var description = new LearningModuleDescription(
                    input.Description.Overview,
                    input.Description.LearningObjectives,
                    input.Description.PrerequisiteKnowledge,
                    input.Description.EstimatedDurationMinutes
                );

                learningResource.UpdateDetails(
                    input.Title,
                    input.ResourceType,
                    input.ResourceUrl,
                    description,
                    input.Tags
                );

                await _learningResourceRepository.UpdateAsync(learningResource, autoSave: true);

                return ObjectMapper.Map<LearningResource, LearningResourceDto>(learningResource);
            }
            catch (BusinessRuleViolationException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code, ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _learningResourceRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.Publish)]
        public virtual async Task<LearningResourceDto> PublishAsync(Guid id)
        {
            try
            {
                var learningResource = await _learningResourceRepository.GetAsync(id);
                
                learningResource.Publish();
                
                await _learningResourceRepository.UpdateAsync(learningResource, autoSave: true);
                
                return ObjectMapper.Map<LearningResource, LearningResourceDto>(learningResource);
            }
            catch (BusinessRuleViolationException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code, ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.Publish)]
        public virtual async Task<LearningResourceDto> UnpublishAsync(Guid id)
        {
            try
            {
                var learningResource = await _learningResourceRepository.GetAsync(id);
                
                learningResource.Unpublish();
                
                await _learningResourceRepository.UpdateAsync(learningResource, autoSave: true);
                
                return ObjectMapper.Map<LearningResource, LearningResourceDto>(learningResource);
            }
            catch (BusinessRuleViolationException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code, ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.Feature)]
        public virtual async Task<LearningResourceDto> SetFeaturedAsync(Guid id, bool isFeatured)
        {
            try
            {
                var learningResource = await _learningResourceRepository.GetAsync(id);
                
                learningResource.SetFeatured(isFeatured);
                
                await _learningResourceRepository.UpdateAsync(learningResource, autoSave: true);
                
                return ObjectMapper.Map<LearningResource, LearningResourceDto>(learningResource);
            }
            catch (BusinessRuleViolationException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code, ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<UserProgressDto> UpdateProgressAsync(Guid id, UpdateProgressDto input)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    throw new BusinessRuleViolationException("User must be logged in to update progress");
                }
                
                var userProgress = await _learningManagementService.TrackProgressAsync(
                    id,
                    _currentUser.Id.Value,
                    input.ProgressPercentage,
                    _clock.Now
                );
                
                return ObjectMapper.Map<UserProgress, UserProgressDto>(userProgress);
            }
            catch (BusinessRuleViolationException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code, ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<UserProgressDto> MarkCompletedAsync(Guid id)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    throw new BusinessRuleViolationException("User must be logged in to mark resource as completed");
                }
                
                var userProgress = await _learningManagementService.MarkResourceCompletedAsync(
                    id,
                    _currentUser.Id.Value,
                    _clock.Now
                );
                
                return ObjectMapper.Map<UserProgress, UserProgressDto>(userProgress);
            }
            catch (BusinessRuleViolationException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code, ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<UserProgressDto> MarkAbandonedAsync(Guid id)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    throw new BusinessRuleViolationException("User must be logged in to mark resource as abandoned");
                }
                
                await _learningManagementService.MarkResourceAbandonedAsync(
                    id,
                    _currentUser.Id.Value,
                    _clock.Now
                );
                
                var learningResource = await _learningResourceRepository.GetAsync(id);
                var userProgress = learningResource.GetUserProgress(_currentUser.Id.Value);
                
                return ObjectMapper.Map<UserProgress, UserProgressDto>(userProgress);
            }
            catch (BusinessRuleViolationException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code, ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Learning Resource"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<List<LearningResourceListDto>> GetCompletedResourcesAsync()
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    return new List<LearningResourceListDto>();
                }
                
                var completedResources = await _learningManagementService.GetCompletedResourcesAsync(_currentUser.Id.Value);
                var completedResourceDtos = ObjectMapper.Map<List<LearningResource>, List<LearningResourceListDto>>(completedResources);
                
                // Set completion status
                foreach (var dto in completedResourceDtos)
                {
                    dto.UserCompletionStatus = CompletionStatus.Completed;
                }
                
                return completedResourceDtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<List<LearningResourceListDto>> GetInProgressResourcesAsync()
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    return new List<LearningResourceListDto>();
                }
                
                var inProgressResources = await _learningManagementService.GetInProgressResourcesAsync(_currentUser.Id.Value);
                var inProgressResourceDtos = ObjectMapper.Map<List<LearningResource>, List<LearningResourceListDto>>(inProgressResources);
                
                // Set completion status
                foreach (var dto in inProgressResourceDtos)
                {
                    dto.UserCompletionStatus = CompletionStatus.InProgress;
                }
                
                return inProgressResourceDtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<List<LearningResourceListDto>> GetRecommendedResourcesAsync(int maxResults = 5)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    // Return some featured resources for anonymous users
                    var featuredResources = await _learningResourceRepository.GetFeaturedAsync();
                    return ObjectMapper.Map<List<LearningResource>, List<LearningResourceListDto>>(
                        featuredResources.Take(maxResults).ToList());
                }
                
                var recommendedResources = await _learningManagementService.GetRecommendedResourcesAsync(
                    _currentUser.Id.Value,
                    maxResults);
                
                return ObjectMapper.Map<List<LearningResource>, List<LearningResourceListDto>>(recommendedResources);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<LearningStatsDto> GetLearningStatsAsync()
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    return new LearningStatsDto();
                }
                
                var stats = await _learningManagementService.GetUserLearningStatsAsync(_currentUser.Id.Value);
                return new LearningStatsDto
                {
                    TotalResourcesStarted = stats.TotalResourcesStarted,
                    ResourcesCompleted = stats.ResourcesCompleted,
                    ResourcesInProgress = stats.ResourcesInProgress,
                    ResourcesAbandoned = stats.ResourcesAbandoned,
                    EstimatedLearningTime = stats.EstimatedLearningTime,
                    TopLearningCategories = stats.TopLearningCategories
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in LearningResourceAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
