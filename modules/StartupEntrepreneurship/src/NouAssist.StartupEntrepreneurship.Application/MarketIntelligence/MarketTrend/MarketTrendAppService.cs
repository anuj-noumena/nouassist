using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Localization;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Permissions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend
{
    [Authorize(MarketIntelligencePermissions.MarketTrend.Default)]
    public class MarketTrendAppService : ApplicationService, IMarketTrendAppService
    {
        private readonly IMarketTrendRepository _marketTrendRepository;
        private readonly ITrendAnalysisService _trendAnalysisService;
        private readonly ILogger<MarketTrendAppService> _logger;

        public MarketTrendAppService(
            IMarketTrendRepository marketTrendRepository,
            ITrendAnalysisService trendAnalysisService,
            ILogger<MarketTrendAppService> logger)
        {
            _marketTrendRepository = marketTrendRepository;
            _trendAnalysisService = trendAnalysisService;
            _logger = logger;
            
            LocalizationResource = typeof(MarketIntelligenceResource);
        }

        [Authorize(MarketIntelligencePermissions.MarketTrend.View)]
        public virtual async Task<MarketTrendDto> GetAsync(Guid id)
        {
            try
            {
                var marketTrend = await _marketTrendRepository.GetAsync(id);
                return ObjectMapper.Map<Aggregates.MarketTrend, MarketTrendDto>(marketTrend);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:MarketTrendNotFound"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.MarketTrend.View)]
        public virtual async Task<PagedResultDto<MarketTrendDto>> GetListAsync(GetMarketTrendListDto input)
        {
            try
            {
                var query = await _marketTrendRepository.GetQueryableAsync();
                
                // Apply filters
                query = query
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), 
                        x => EF.Property<string>(x, "TrendDescription_Title").Contains(input.Filter) || 
                             EF.Property<string>(x, "TrendDescription_Description").Contains(input.Filter) ||
                             EF.Property<string>(x, "TrendDescription_Keywords").Contains(input.Filter) ||
                             x.Tags.Contains(input.Filter))
                    .WhereIf(input.Category.HasValue, x => x.Category == input.Category.Value)
                    .WhereIf(input.ImpactLevel.HasValue, 
                        x => EF.Property<string>(x, "ImpactAssessment_Level") == input.ImpactLevel.ToString())
                    .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value)
                    .WhereIf(input.MinConfidenceScore.HasValue, 
                        x => EF.Property<decimal>(x, "ImpactAssessment_ConfidenceScore") >= input.MinConfidenceScore.Value)
                    .WhereIf(input.ValidAfterDate.HasValue, 
                        x => !x.ExpiryDate.HasValue || x.ExpiryDate.Value >= input.ValidAfterDate.Value);

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = ApplySorting(query, input);
                
                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

                // Include data sources if requested
                if (input.IncludeDataSources)
                {
                    query = query.Include(x => x.DataSources);
                }
                
                // Execute query
                var marketTrends = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var marketTrendDtos = ObjectMapper.Map<List<Aggregates.MarketTrend>, List<MarketTrendDto>>(marketTrends);
                
                return new PagedResultDto<MarketTrendDto>(totalCount, marketTrendDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.MarketTrend.Create)]
        public virtual async Task<MarketTrendDto> CreateAsync(CreateMarketTrendDto input)
        {
            try
            {
                var trendDescription = new TrendDescription(
                    input.Title,
                    input.Description,
                    input.Keywords,
                    input.SourceUrl);
                
                var impactAssessment = new ImpactAssessment(
                    input.ImpactLevel,
                    input.ImpactDescription,
                    input.ConfidenceScore,
                    input.RecommendedActions);
                
                var marketTrend = await _trendAnalysisService.CreateMarketTrendAsync(
                    trendDescription,
                    input.Category,
                    impactAssessment,
                    input.ExpiryDate);
                
                // Set tags if provided
                if (!string.IsNullOrEmpty(input.Tags))
                {
                    marketTrend.Tags = input.Tags;
                    await _marketTrendRepository.UpdateAsync(marketTrend);
                }
                
                return ObjectMapper.Map<Aggregates.MarketTrend, MarketTrendDto>(marketTrend);
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

        [Authorize(MarketIntelligencePermissions.MarketTrend.Update)]
        public virtual async Task<MarketTrendDto> UpdateAsync(Guid id, UpdateMarketTrendDto input)
        {
            try
            {
                var trendDescription = new TrendDescription(
                    input.Title,
                    input.Description,
                    input.Keywords,
                    input.SourceUrl);
                
                var impactAssessment = new ImpactAssessment(
                    input.ImpactLevel,
                    input.ImpactDescription,
                    input.ConfidenceScore,
                    input.RecommendedActions);
                
                var marketTrend = await _trendAnalysisService.UpdateMarketTrendAsync(
                    id,
                    trendDescription,
                    impactAssessment,
                    input.Tags);
                
                // Update expiry date if provided
                if (input.ExpiryDate != null && 
                    (marketTrend.ExpiryDate == null || marketTrend.ExpiryDate.Value != input.ExpiryDate.Value))
                {
                    if (input.ExpiryDate.Value <= Clock.Now)
                    {
                        throw new UserFriendlyException(L["ErrorMessage:InvalidExpiryDate"]);
                    }
                    
                    marketTrend.ExtendTrend(input.ExpiryDate.Value);
                    await _marketTrendRepository.UpdateAsync(marketTrend);
                }
                
                return ObjectMapper.Map<Aggregates.MarketTrend, MarketTrendDto>(marketTrend);
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

        [Authorize(MarketIntelligencePermissions.MarketTrend.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _marketTrendRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.MarketTrend.ManageDataSources)]
        public virtual async Task<TrendDataSourceDto> AddDataSourceAsync(Guid id, CreateTrendDataSourceDto input)
        {
            try
            {
                await _trendAnalysisService.AddTrendDataSourceAsync(
                    id,
                    input.SourceName,
                    input.SourceUrl,
                    input.SourceType,
                    input.Reliability,
                    input.DataQuality,
                    input.ExtractedData);
                
                // Get the updated market trend to retrieve the newest data source
                var marketTrend = await _marketTrendRepository.GetAsync(id);
                var latestDataSource = marketTrend.DataSources.OrderByDescending(ds => ds.CollectedAt).FirstOrDefault();
                
                if (latestDataSource == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DataSourceNotFound"]);
                }
                
                return ObjectMapper.Map<Entities.TrendDataSource, TrendDataSourceDto>(latestDataSource);
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

        [Authorize(MarketIntelligencePermissions.MarketTrend.ManageDataSources)]
        public virtual async Task DeleteDataSourceAsync(Guid trendId, Guid dataSourceId)
        {
            try
            {
                var marketTrend = await _marketTrendRepository.GetAsync(trendId);
                marketTrend.RemoveDataSource(dataSourceId);
                await _marketTrendRepository.UpdateAsync(marketTrend);
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

        [Authorize(MarketIntelligencePermissions.MarketTrend.Update)]
        public virtual async Task<MarketTrendDto> DeactivateAsync(Guid id, string reason = null)
        {
            try
            {
                var marketTrend = await _marketTrendRepository.GetAsync(id);
                marketTrend.DeactivateTrend(reason);
                await _marketTrendRepository.UpdateAsync(marketTrend);
                
                return ObjectMapper.Map<Aggregates.MarketTrend, MarketTrendDto>(marketTrend);
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

        [Authorize(MarketIntelligencePermissions.MarketTrend.Update)]
        public virtual async Task<MarketTrendDto> ReactivateAsync(Guid id)
        {
            try
            {
                var marketTrend = await _marketTrendRepository.GetAsync(id);
                marketTrend.ReactivateTrend();
                await _marketTrendRepository.UpdateAsync(marketTrend);
                
                return ObjectMapper.Map<Aggregates.MarketTrend, MarketTrendDto>(marketTrend);
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

        [Authorize(MarketIntelligencePermissions.MarketTrend.Update)]
        public virtual async Task<MarketTrendDto> ExtendExpiryDateAsync(Guid id, DateTime newExpiryDate)
        {
            try
            {
                var marketTrend = await _marketTrendRepository.GetAsync(id);
                marketTrend.ExtendTrend(newExpiryDate);
                await _marketTrendRepository.UpdateAsync(marketTrend);
                
                return ObjectMapper.Map<Aggregates.MarketTrend, MarketTrendDto>(marketTrend);
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

        [Authorize(MarketIntelligencePermissions.MarketTrend.View)]
        public virtual async Task<List<MarketTrendDto>> GetActiveByCategoryAsync(TrendCategory category)
        {
            try
            {
                var marketTrends = await _trendAnalysisService.GetActiveTrendsByCategoryAsync(category);
                return ObjectMapper.Map<List<Aggregates.MarketTrend>, List<MarketTrendDto>>(marketTrends);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.MarketTrend.View)]
        public virtual async Task<List<MarketTrendDto>> GetHighImpactTrendsAsync()
        {
            try
            {
                var marketTrends = await _trendAnalysisService.GetHighImpactTrendsAsync();
                return ObjectMapper.Map<List<Aggregates.MarketTrend>, List<MarketTrendDto>>(marketTrends);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.MarketTrend.View)]
        public virtual async Task<List<OpportunityAlertDto>> GetMarketOpportunitiesAsync()
        {
            try
            {
                var opportunities = await _trendAnalysisService.EvaluateMarketOpportunitiesAsync();
                return opportunities.Select(o => new OpportunityAlertDto
                {
                    Id = o.Id,
                    SourceId = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    ImpactLevel = o.ImpactLevel,
                    ConfidenceScore = o.ConfidenceScore,
                    RecommendedActions = o.RecommendedActions,
                    Category = o.Category,
                    AlertType = "Trend-Based Opportunity",
                    CreatedAt = Clock.Now
                }).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        // Helper method for sorting
        private static IQueryable<Aggregates.MarketTrend> ApplySorting(
            IQueryable<Aggregates.MarketTrend> query, 
            GetMarketTrendListDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                // Default sorting by impact level and confidence score (both descending)
                return query.OrderByDescending(x => EF.Property<string>(x, "ImpactAssessment_Level"))
                           .ThenByDescending(x => EF.Property<decimal>(x, "ImpactAssessment_ConfidenceScore"));
            }
            
            return query.OrderBy(input.Sorting);
        }

        // Error handling helper
        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in MarketTrendAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
