using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Permissions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ResourceLibrary.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Services;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ResourceLibrary
{
    [Authorize]
    public class ResourceLibraryAppService : ApplicationService, IResourceLibraryAppService
    {
        private readonly IResourceLibraryService _resourceLibraryService;
        private readonly ICurrentUser _currentUser;

        public ResourceLibraryAppService(
            IResourceLibraryService resourceLibraryService,
            ICurrentUser currentUser)
        {
            _resourceLibraryService = resourceLibraryService;
            _currentUser = currentUser;
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.View)]
        public virtual async Task<List<KnowledgeAssetListDto>> SearchKnowledgeAssetsAsync(SearchResourcesDto input)
        {
            try
            {
                var assets = await _resourceLibraryService.SearchKnowledgeAssetsAsync(
                    input.SearchText,
                    input.AssetType,
                    input.Tags,
                    input.MaxResults);
                
                return ObjectMapper.Map<List<KnowledgeAsset>, List<KnowledgeAssetListDto>>(assets);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.LearningResource.View)]
        public virtual async Task<List<LearningResourceListDto>> SearchLearningResourcesAsync(SearchResourcesDto input)
        {
            try
            {
                var resources = await _resourceLibraryService.SearchLearningResourcesAsync(
                    input.SearchText,
                    input.ResourceType,
                    input.Tags,
                    input.MaxResults);
                
                return ObjectMapper.Map<List<LearningResource>, List<LearningResourceListDto>>(resources);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize]
        public virtual async Task<List<string>> AddTagsAsync(string entityType, Guid entityId, List<string> tags)
        {
            try
            {
                return await _resourceLibraryService.AddTagsAsync(entityType, entityId, tags);
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
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", entityType]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [AllowAnonymous]
        public virtual async Task<List<string>> GetAllTagsAsync()
        {
            try
            {
                return await _resourceLibraryService.GetAllTagsAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [AllowAnonymous]
        public virtual async Task<List<string>> GetPopularTagsAsync(int maxTags = 20)
        {
            try
            {
                return await _resourceLibraryService.GetPopularTagsAsync(maxTags);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in ResourceLibraryAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
