using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Permissions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Services;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets
{
    [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.Default)]
    public class KnowledgeAssetAppService : ApplicationService, IKnowledgeAssetAppService
    {
        private readonly IKnowledgeAssetRepository _knowledgeAssetRepository;
        private readonly IResourceLibraryService _resourceLibraryService;
        private readonly ICurrentUser _currentUser;

        public KnowledgeAssetAppService(
            IKnowledgeAssetRepository knowledgeAssetRepository,
            IResourceLibraryService resourceLibraryService,
            ICurrentUser currentUser)
        {
            _knowledgeAssetRepository = knowledgeAssetRepository;
            _resourceLibraryService = resourceLibraryService;
            _currentUser = currentUser;
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.View)]
        public virtual async Task<KnowledgeAssetDto> GetAsync(Guid id)
        {
            try
            {
                var knowledgeAsset = await _knowledgeAssetRepository.GetAsync(id);
                return ObjectMapper.Map<KnowledgeAsset, KnowledgeAssetDto>(knowledgeAsset);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Knowledge Asset"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.View)]
        public virtual async Task<PagedResultDto<KnowledgeAssetListDto>> GetListAsync(GetKnowledgeAssetListDto input)
        {
            try
            {
                var query = await _knowledgeAssetRepository.GetQueryableAsync();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    query = query.Where(x => x.Title.Contains(input.Filter) || 
                                          x.Description.Contains(input.Filter));
                }

                if (input.AssetType.HasValue)
                {
                    query = query.Where(x => x.AssetType == input.AssetType.Value);
                }

                if (!string.IsNullOrWhiteSpace(input.Tag))
                {
                    query = query.Where(x => x.Tags.Contains(input.Tag));
                }

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);

                // Apply sorting
                query = query.OrderBy(input.Sorting ?? nameof(KnowledgeAsset.Title));

                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

                // Execute query and map results
                var knowledgeAssets = await AsyncExecuter.ToListAsync(query);
                var knowledgeAssetDtos = ObjectMapper.Map<List<KnowledgeAsset>, List<KnowledgeAssetListDto>>(knowledgeAssets);

                return new PagedResultDto<KnowledgeAssetListDto>(totalCount, knowledgeAssetDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.Create)]
        public virtual async Task<KnowledgeAssetDto> CreateAsync(CreateKnowledgeAssetDto input)
        {
            try
            {
                var currentUserId = _currentUser.GetId();
                
                // Create new knowledge asset
                var knowledgeAsset = KnowledgeAsset.Create(
                    GuidGenerator.Create(),
                    input.Title,
                    input.Description,
                    input.AssetType,
                    new AuthorInfo(
                        input.Author.UserId,
                        input.Author.Name,
                        input.Author.Title,
                        input.Author.AvatarUrl
                    ),
                    input.Content,
                    input.ContentType,
                    input.Tags,
                    CurrentTenant.Id
                );

                await _knowledgeAssetRepository.InsertAsync(knowledgeAsset, autoSave: true);

                return ObjectMapper.Map<KnowledgeAsset, KnowledgeAssetDto>(knowledgeAsset);
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

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.Update)]
        public virtual async Task<KnowledgeAssetDto> UpdateAsync(Guid id, UpdateKnowledgeAssetDto input)
        {
            try
            {
                var knowledgeAsset = await _knowledgeAssetRepository.GetAsync(id);

                knowledgeAsset.UpdateDetails(
                    input.Title,
                    input.Description,
                    input.AssetType,
                    input.Tags
                );

                await _knowledgeAssetRepository.UpdateAsync(knowledgeAsset, autoSave: true);

                return ObjectMapper.Map<KnowledgeAsset, KnowledgeAssetDto>(knowledgeAsset);
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
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Knowledge Asset"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _knowledgeAssetRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Knowledge Asset"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.Update)]
        public virtual async Task<KnowledgeAssetDto> UpdateContentAsync(Guid id, UpdateContentDto input)
        {
            try
            {
                var knowledgeAsset = await _knowledgeAssetRepository.GetAsync(id);

                knowledgeAsset.UpdateCurrentContent(input.Content, input.ChangeLog);

                await _knowledgeAssetRepository.UpdateAsync(knowledgeAsset, autoSave: true);

                return ObjectMapper.Map<KnowledgeAsset, KnowledgeAssetDto>(knowledgeAsset);
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
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Knowledge Asset"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.View)]
        public virtual async Task<AssetContentDto> GetContentVersionAsync(Guid id, int version)
        {
            try
            {
                var knowledgeAsset = await _knowledgeAssetRepository.GetAsync(id);
                var contentVersion = knowledgeAsset.GetContentVersion(version);

                if (contentVersion == null)
                {
                    throw new BusinessRuleViolationException($"Content version {version} not found");
                }

                return ObjectMapper.Map<AssetContent, AssetContentDto>(contentVersion);
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
                    throw new UserFriendlyException(L["KnowledgeInnovation:ErrorMessage:EntityNotFound", "Knowledge Asset"]);
                }
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.View)]
        public virtual async Task<List<KnowledgeAssetListDto>> GetRecommendedAssetsAsync(int maxResults = 5)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    // Return some featured assets for anonymous users
                    var assets = await _knowledgeAssetRepository.GetListAsync(
                        new KnowledgeAssetTypeSpecification(KnowledgeAssetType.MentalModel),
                        maxResults);
                    
                    return ObjectMapper.Map<List<KnowledgeAsset>, List<KnowledgeAssetListDto>>(assets);
                }

                var recommendedAssets = await _resourceLibraryService.GetRecommendedAssetsAsync(
                    _currentUser.Id.Value,
                    maxResults);

                return ObjectMapper.Map<List<KnowledgeAsset>, List<KnowledgeAssetListDto>>(recommendedAssets);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.View)]
        public virtual async Task<List<KnowledgeAssetListDto>> GetByTypeAsync(KnowledgeAssetType assetType)
        {
            try
            {
                var assets = await _knowledgeAssetRepository.GetByTypeAsync(assetType);
                return ObjectMapper.Map<List<KnowledgeAsset>, List<KnowledgeAssetListDto>>(assets);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(KnowledgeInnovationPermissions.KnowledgeAsset.View)]
        public virtual async Task<List<KnowledgeAssetListDto>> GetByTagAsync(string tag)
        {
            try
            {
                var assets = await _knowledgeAssetRepository.GetByTagAsync(tag);
                return ObjectMapper.Map<List<KnowledgeAsset>, List<KnowledgeAssetListDto>>(assets);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in KnowledgeAssetAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
