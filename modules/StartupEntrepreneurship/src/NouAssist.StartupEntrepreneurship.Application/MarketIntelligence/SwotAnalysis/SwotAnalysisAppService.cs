using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Permissions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis
{
    [Authorize(MarketIntelligencePermissions.SwotAnalysis.Default)]
    public class SwotAnalysisAppService : ApplicationService, ISwotAnalysisAppService
    {
        private readonly ISwotAnalysisRepository _swotRepository;
        private readonly ISwotAnalysisService _swotService;
        private readonly ILogger<SwotAnalysisAppService> _logger;

        public SwotAnalysisAppService(
            ISwotAnalysisRepository swotRepository,
            ISwotAnalysisService swotService,
            ILogger<SwotAnalysisAppService> logger)
        {
            _swotRepository = swotRepository;
            _swotService = swotService;
            _logger = logger;
            
            LocalizationResource = typeof(MarketIntelligenceResource);
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.View)]
        public virtual async Task<SwotAnalysisDto> GetAsync(Guid id)
        {
            try
            {
                var swotAnalysis = await _swotRepository.GetAsync(id);
                return ObjectMapper.Map<Aggregates.SwotAnalysis, SwotAnalysisDto>(swotAnalysis);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:SwotAnalysisNotFound"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.View)]
        public virtual async Task<PagedResultDto<SwotAnalysisDto>> GetListAsync(GetSwotAnalysisListDto input)
        {
            try
            {
                var query = await _swotRepository.GetQueryableAsync();
                
                // Apply filters
                query = query
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), 
                        x => x.Title.Contains(input.Filter) || 
                             x.Description.Contains(input.Filter))
                    .WhereIf(input.IsCompleted.HasValue, x => x.IsCompleted == input.IsCompleted.Value)
                    .WhereIf(input.CreatedAfter.HasValue, x => x.AnalysisDate >= input.CreatedAfter.Value)
                    .WhereIf(input.CreatedBefore.HasValue, x => x.AnalysisDate <= input.CreatedBefore.Value);

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = ApplySorting(query, input);
                
                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

                // Include items if requested
                if (input.IncludeItems)
                {
                    query = query.Include(x => x.Items);
                }
                
                // Execute query
                var swotAnalyses = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var swotAnalysisDtos = ObjectMapper.Map<List<Aggregates.SwotAnalysis>, List<SwotAnalysisDto>>(swotAnalyses);
                
                return new PagedResultDto<SwotAnalysisDto>(totalCount, swotAnalysisDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.Create)]
        public virtual async Task<SwotAnalysisDto> CreateAsync(CreateSwotAnalysisDto input)
        {
            try
            {
                var swotAnalysis = await _swotService.CreateSwotAnalysisAsync(
                    input.Title,
                    input.Description);
                
                // Set analysis notes if provided
                if (!string.IsNullOrEmpty(input.AnalysisNotes))
                {
                    swotAnalysis.UpdateAnalysis(
                        swotAnalysis.Title,
                        swotAnalysis.Description,
                        input.AnalysisNotes);
                    
                    await _swotRepository.UpdateAsync(swotAnalysis);
                }
                
                return ObjectMapper.Map<Aggregates.SwotAnalysis, SwotAnalysisDto>(swotAnalysis);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.Update)]
        public virtual async Task<SwotAnalysisDto> UpdateAsync(Guid id, UpdateSwotAnalysisDto input)
        {
            try
            {
                var swotAnalysis = await _swotService.UpdateSwotAnalysisAsync(
                    id,
                    input.Title,
                    input.Description,
                    input.AnalysisNotes);
                
                return ObjectMapper.Map<Aggregates.SwotAnalysis, SwotAnalysisDto>(swotAnalysis);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _swotRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.ManageItems)]
        public virtual async Task<SwotItemDto> AddItemAsync(Guid id, CreateSwotItemDto input)
        {
            try
            {
                await _swotService.AddSwotItemAsync(
                    id,
                    input.Category,
                    input.Title,
                    input.Description,
                    input.Impact,
                    input.Priority,
                    input.ActionItems,
                    input.Tags);
                
                // Get the updated SWOT analysis to retrieve the newest item
                var swotAnalysis = await _swotRepository.GetAsync(id);
                var latestItem = swotAnalysis.Items
                    .Where(i => i.Category == input.Category && i.Title == input.Title)
                    .OrderByDescending(i => i.Id)
                    .FirstOrDefault();
                
                if (latestItem == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:SwotItemNotFound"]);
                }
                
                return ObjectMapper.Map<Entities.SwotItem, SwotItemDto>(latestItem);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.ManageItems)]
        public virtual async Task<SwotItemDto> UpdateItemAsync(
            Guid analysisId, 
            Guid itemId, 
            CreateSwotItemDto input)
        {
            try
            {
                var swotAnalysis = await _swotRepository.GetAsync(analysisId);
                
                if (swotAnalysis.IsCompleted)
                {
                    throw new UserFriendlyException(L["ErrorMessage:CannotModifyCompletedAnalysis"]);
                }
                
                swotAnalysis.UpdateItem(
                    itemId,
                    input.Title,
                    input.Description,
                    input.Impact,
                    input.Priority,
                    input.ActionItems,
                    input.Tags);
                
                await _swotRepository.UpdateAsync(swotAnalysis);
                
                var updatedItem = swotAnalysis.Items.FirstOrDefault(i => i.Id == itemId);
                
                if (updatedItem == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:SwotItemNotFound"]);
                }
                
                return ObjectMapper.Map<Entities.SwotItem, SwotItemDto>(updatedItem);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.ManageItems)]
        public virtual async Task DeleteItemAsync(Guid analysisId, Guid itemId)
        {
            try
            {
                var swotAnalysis = await _swotRepository.GetAsync(analysisId);
                
                if (swotAnalysis.IsCompleted)
                {
                    throw new UserFriendlyException(L["ErrorMessage:CannotModifyCompletedAnalysis"]);
                }
                
                swotAnalysis.RemoveItem(itemId);
                await _swotRepository.UpdateAsync(swotAnalysis);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.Complete)]
        public virtual async Task<SwotAnalysisDto> CompleteAsync(Guid id, CompleteSwotAnalysisDto input)
        {
            try
            {
                var swotAnalysis = await _swotService.CompleteSwotAnalysisAsync(
                    id,
                    input.StrategicRecommendation);
                
                return ObjectMapper.Map<Aggregates.SwotAnalysis, SwotAnalysisDto>(swotAnalysis);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.Update)]
        public virtual async Task<SwotAnalysisDto> ReopenAsync(Guid id)
        {
            try
            {
                var swotAnalysis = await _swotRepository.GetAsync(id);
                swotAnalysis.ReopenAnalysis();
                await _swotRepository.UpdateAsync(swotAnalysis);
                
                return ObjectMapper.Map<Aggregates.SwotAnalysis, SwotAnalysisDto>(swotAnalysis);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.View)]
        public virtual async Task<List<SwotAnalysisDto>> GetRecentAsync(int maxCount = 5)
        {
            try
            {
                var swotAnalyses = await _swotService.GetRecentSwotAnalysesAsync(maxCount);
                return ObjectMapper.Map<List<Aggregates.SwotAnalysis>, List<SwotAnalysisDto>>(swotAnalyses);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.View)]
        public virtual async Task<List<SwotAnalysisDto>> GetCompletedAsync()
        {
            try
            {
                var swotAnalyses = await _swotService.GetCompletedSwotAnalysesAsync();
                return ObjectMapper.Map<List<Aggregates.SwotAnalysis>, List<SwotAnalysisDto>>(swotAnalyses);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.View)]
        public virtual async Task<SwotAnalysisQualityDto> EvaluateQualityAsync(Guid id)
        {
            try
            {
                var qualityResult = await _swotService.EvaluateSwotAnalysisQualityAsync(id);
                return ObjectMapper.Map<Services.SwotAnalysisQualityResult, SwotAnalysisQualityDto>(qualityResult);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.SwotAnalysis.View)]
        public virtual async Task<Dictionary<SwotCategory, List<SwotItemDto>>> GetItemsByCategoryAsync(Guid id)
        {
            try
            {
                var itemsByCategory = await _swotService.GetItemsByCategoryAsync(id);
                
                var result = new Dictionary<SwotCategory, List<SwotItemDto>>();
                
                foreach (var category in itemsByCategory.Keys)
                {
                    result[category] = ObjectMapper.Map<List<Entities.SwotItem>, List<SwotItemDto>>(
                        itemsByCategory[category]);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        // Helper method for sorting
        private static IQueryable<Aggregates.SwotAnalysis> ApplySorting(
            IQueryable<Aggregates.SwotAnalysis> query, 
            GetSwotAnalysisListDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                // Default sorting by analysis date (descending)
                return query.OrderByDescending(x => x.AnalysisDate);
            }
            
            return query.OrderBy(input.Sorting);
        }

        // Error handling helper
        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in SwotAnalysisAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
