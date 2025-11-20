using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Permissions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor
{
    [Authorize(MarketIntelligencePermissions.Competitor.Default)]
    public class CompetitorAppService : ApplicationService, ICompetitorAppService
    {
        private readonly ICompetitorRepository _competitorRepository;
        private readonly ICompetitorMonitoringService _competitorMonitoringService;
        private readonly ILogger<CompetitorAppService> _logger;

        public CompetitorAppService(
            ICompetitorRepository competitorRepository,
            ICompetitorMonitoringService competitorMonitoringService,
            ILogger<CompetitorAppService> logger)
        {
            _competitorRepository = competitorRepository;
            _competitorMonitoringService = competitorMonitoringService;
            _logger = logger;
            
            LocalizationResource = typeof(MarketIntelligenceResource);
        }

        [Authorize(MarketIntelligencePermissions.Competitor.View)]
        public virtual async Task<CompetitorDto> GetAsync(Guid id)
        {
            try
            {
                var competitor = await _competitorRepository.GetAsync(id);
                
                var competitorDto = ObjectMapper.Map<Aggregates.Competitor, CompetitorDto>(competitor);
                competitorDto.RecentUpdateLogs = ObjectMapper.Map<List<Entities.CompetitorUpdateLog>, List<CompetitorUpdateLogDto>>(
                    competitor.GetRecentUpdates().ToList());
                
                return competitorDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:CompetitorNotFound"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.Competitor.View)]
        public virtual async Task<PagedResultDto<CompetitorDto>> GetListAsync(GetCompetitorListDto input)
        {
            try
            {
                var query = await _competitorRepository.GetQueryableAsync();
                
                // Apply filters
                query = query
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), 
                        x => x.Name.Contains(input.Filter) || 
                             x.Description.Contains(input.Filter) ||
                             x.Tags.Contains(input.Filter))
                    .WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value)
                    .WhereIf(input.ThreatLevel.HasValue, x => x.ThreatLevel == input.ThreatLevel.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Industry), 
                        x => EF.Property<string>(x, "CompetitorInfo_Industry").Contains(input.Industry));

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = ApplySorting(query, input);
                
                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                
                // Execute query
                var competitors = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var competitorDtos = ObjectMapper.Map<List<Aggregates.Competitor>, List<CompetitorDto>>(competitors);
                
                // Add recent updates to each competitor
                foreach (var dto in competitorDtos)
                {
                    var competitor = competitors.First(c => c.Id == dto.Id);
                    dto.RecentUpdateLogs = ObjectMapper.Map<List<Entities.CompetitorUpdateLog>, List<CompetitorUpdateLogDto>>(
                        competitor.GetRecentUpdates(input.MaxRecentUpdates).ToList());
                }
                
                return new PagedResultDto<CompetitorDto>(totalCount, competitorDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.Competitor.Create)]
        public virtual async Task<CompetitorDto> CreateAsync(CreateCompetitorDto input)
        {
            try
            {
                var competitorInfo = ObjectMapper.Map<CreateCompetitorInfoDto, CompetitorInfo>(input.CompetitorInfo);
                
                var competitor = await _competitorMonitoringService.CreateCompetitorAsync(
                    input.Name,
                    input.Description,
                    competitorInfo,
                    input.ThreatLevel);
                
                // Set notes and tags if provided
                if (!string.IsNullOrEmpty(input.Notes) || !string.IsNullOrEmpty(input.Tags))
                {
                    competitor.Notes = input.Notes;
                    competitor.Tags = input.Tags;
                    await _competitorRepository.UpdateAsync(competitor);
                }
                
                var competitorDto = ObjectMapper.Map<Aggregates.Competitor, CompetitorDto>(competitor);
                competitorDto.RecentUpdateLogs = new List<CompetitorUpdateLogDto>();
                
                return competitorDto;
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

        [Authorize(MarketIntelligencePermissions.Competitor.Update)]
        public virtual async Task<CompetitorDto> UpdateAsync(Guid id, UpdateCompetitorDto input)
        {
            try
            {
                var competitorInfo = ObjectMapper.Map<CreateCompetitorInfoDto, CompetitorInfo>(input.CompetitorInfo);
                
                var competitor = await _competitorMonitoringService.UpdateCompetitorInfoAsync(
                    id,
                    input.Name,
                    input.Description,
                    competitorInfo);
                
                // Update notes and tags
                competitor.Notes = input.Notes;
                competitor.Tags = input.Tags;
                await _competitorRepository.UpdateAsync(competitor);
                
                var competitorDto = ObjectMapper.Map<Aggregates.Competitor, CompetitorDto>(competitor);
                competitorDto.RecentUpdateLogs = ObjectMapper.Map<List<Entities.CompetitorUpdateLog>, List<CompetitorUpdateLogDto>>(
                    competitor.GetRecentUpdates().ToList());
                
                return competitorDto;
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

        [Authorize(MarketIntelligencePermissions.Competitor.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _competitorRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.Competitor.Update)]
        public virtual async Task<CompetitorUpdateLogDto> AddUpdateAsync(Guid id, CreateCompetitorUpdateDto input)
        {
            try
            {
                await _competitorMonitoringService.AddCompetitorUpdateAsync(
                    id,
                    input.Type,
                    input.Title,
                    input.Description,
                    input.ImpactLevel,
                    input.Source,
                    input.Tags);
                
                // Get the updated competitor to retrieve the newest log
                var competitor = await _competitorRepository.GetAsync(id);
                var latestLog = competitor.GetRecentUpdates(1).FirstOrDefault();
                
                if (latestLog == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:UpdateLogNotFound"]);
                }
                
                return ObjectMapper.Map<Entities.CompetitorUpdateLog, CompetitorUpdateLogDto>(latestLog);
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

        [Authorize(MarketIntelligencePermissions.Competitor.ManageThreatLevel)]
        public virtual async Task<CompetitorDto> UpdateThreatLevelAsync(Guid id, UpdateThreatLevelDto input)
        {
            try
            {
                await _competitorMonitoringService.ChangeCompetitorThreatLevelAsync(
                    id,
                    input.ThreatLevel,
                    input.Reason);
                
                var competitor = await _competitorRepository.GetAsync(id);
                
                var competitorDto = ObjectMapper.Map<Aggregates.Competitor, CompetitorDto>(competitor);
                competitorDto.RecentUpdateLogs = ObjectMapper.Map<List<Entities.CompetitorUpdateLog>, List<CompetitorUpdateLogDto>>(
                    competitor.GetRecentUpdates().ToList());
                
                return competitorDto;
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

        [Authorize(MarketIntelligencePermissions.Competitor.ManageStatus)]
        public virtual async Task<CompetitorDto> ArchiveAsync(Guid id)
        {
            try
            {
                await _competitorMonitoringService.ArchiveCompetitorAsync(id);
                
                var competitor = await _competitorRepository.GetAsync(id);
                
                var competitorDto = ObjectMapper.Map<Aggregates.Competitor, CompetitorDto>(competitor);
                competitorDto.RecentUpdateLogs = ObjectMapper.Map<List<Entities.CompetitorUpdateLog>, List<CompetitorUpdateLogDto>>(
                    competitor.GetRecentUpdates().ToList());
                
                return competitorDto;
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

        [Authorize(MarketIntelligencePermissions.Competitor.View)]
        public virtual async Task<List<CompetitorDto>> GetByThreatLevelAsync(CompetitorThreatLevel threatLevel)
        {
            try
            {
                var competitors = await _competitorRepository.GetByThreatLevelAsync(threatLevel);
                
                var competitorDtos = ObjectMapper.Map<List<Aggregates.Competitor>, List<CompetitorDto>>(competitors);
                
                // Add recent updates to each competitor
                foreach (var dto in competitorDtos)
                {
                    var competitor = competitors.First(c => c.Id == dto.Id);
                    dto.RecentUpdateLogs = ObjectMapper.Map<List<Entities.CompetitorUpdateLog>, List<CompetitorUpdateLogDto>>(
                        competitor.GetRecentUpdates(3).ToList());
                }
                
                return competitorDtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.Competitor.View)]
        public virtual async Task<List<CompetitorDto>> GetWithRecentUpdatesAsync(int days = 30, int maxCount = 10)
        {
            try
            {
                var competitors = await _competitorRepository.GetWithRecentUpdatesAsync(days, maxCount);
                
                var competitorDtos = ObjectMapper.Map<List<Aggregates.Competitor>, List<CompetitorDto>>(competitors);
                
                // Add recent updates to each competitor
                foreach (var dto in competitorDtos)
                {
                    var competitor = competitors.First(c => c.Id == dto.Id);
                    dto.RecentUpdateLogs = ObjectMapper.Map<List<Entities.CompetitorUpdateLog>, List<CompetitorUpdateLogDto>>(
                        competitor.GetRecentUpdates(3).ToList());
                }
                
                return competitorDtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        // Helper method for sorting
        private static IQueryable<Aggregates.Competitor> ApplySorting(
            IQueryable<Aggregates.Competitor> query, 
            GetCompetitorListDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                // Default sorting by threat level (descending) and then by name
                return query.OrderByDescending(x => x.ThreatLevel).ThenBy(x => x.Name);
            }
            
            return query.OrderBy(input.Sorting);
        }

        // Error handling helper
        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in CompetitorAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
