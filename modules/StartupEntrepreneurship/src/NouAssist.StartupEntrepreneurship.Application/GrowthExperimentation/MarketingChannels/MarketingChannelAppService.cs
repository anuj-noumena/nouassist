using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Permissions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels
{
    [Authorize(GrowthExperimentationPermissions.MarketingChannel.Default)]
    public class MarketingChannelAppService : ApplicationService, IMarketingChannelAppService
    {
        private readonly IMarketingChannelPerformanceRepository _channelRepository;
        private readonly IChannelPerformanceTracker _channelPerformanceTracker;

        public MarketingChannelAppService(
            IMarketingChannelPerformanceRepository channelRepository,
            IChannelPerformanceTracker channelPerformanceTracker)
        {
            _channelRepository = channelRepository;
            _channelPerformanceTracker = channelPerformanceTracker;
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.View)]
        public async Task<MarketingChannelDto> GetAsync(Guid id)
        {
            try
            {
                var channel = await _channelRepository.GetWithMetricsAsync(id);
                return ObjectMapper.Map<MarketingChannelPerformance, MarketingChannelDto>(channel);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ChannelNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting marketing channel with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.View)]
        public async Task<PagedResultDto<MarketingChannelDto>> GetListAsync(GetMarketingChannelListDto input)
        {
            try
            {
                var queryable = await _channelRepository.GetQueryableAsync();

                // Apply filtering
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    queryable = queryable.Where(c => c.Name.Contains(input.Filter) || 
                                                    c.Description.Contains(input.Filter));
                }

                if (input.Type.HasValue)
                {
                    queryable = queryable.Where(c => c.Type == input.Type.Value);
                }

                if (input.IsActive.HasValue)
                {
                    queryable = queryable.Where(c => c.IsActive == input.IsActive.Value);
                }

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(MarketingChannelPerformance.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);

                var channels = await AsyncExecuter.ToListAsync(queryable);
                var channelDtos = ObjectMapper.Map<List<MarketingChannelPerformance>, List<MarketingChannelDto>>(channels);

                return new PagedResultDto<MarketingChannelDto>(totalCount, channelDtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting marketing channel list");
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.Create)]
        public async Task<MarketingChannelDto> CreateAsync(CreateMarketingChannelDto input)
        {
            try
            {
                // Map DTOs to domain objects
                var channelDetails = ObjectMapper.Map<CreateChannelDetailsDto, ChannelDetails>(input.Details);
                
                // Create channel using domain service
                var channel = await _channelPerformanceTracker.CreateChannelAsync(
                    input.Name,
                    input.Description,
                    input.Type,
                    channelDetails);

                return ObjectMapper.Map<MarketingChannelPerformance, MarketingChannelDto>(channel);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error creating channel: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating marketing channel {Name}", input.Name);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.Update)]
        public async Task<MarketingChannelDto> UpdateAsync(Guid id, UpdateMarketingChannelDto input)
        {
            try
            {
                var channel = await _channelRepository.GetAsync(id);
                
                // Update basic properties
                channel.Name = input.Name;
                channel.Description = input.Description;
                channel.Type = input.Type;
                channel.IsActive = input.IsActive;
                
                // Update details if provided
                if (input.Details != null)
                {
                    var channelDetails = ObjectMapper.Map<CreateChannelDetailsDto, ChannelDetails>(input.Details);
                    channel.UpdateDetails(channelDetails);
                }
                
                await _channelRepository.UpdateAsync(channel);
                
                return ObjectMapper.Map<MarketingChannelPerformance, MarketingChannelDto>(channel);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ChannelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error updating channel: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating marketing channel with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _channelRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException)
            {
                // If entity not found, consider it as deleted
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting marketing channel with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.Update)]
        public async Task<MarketingChannelDto> SetActiveAsync(Guid id, bool isActive)
        {
            try
            {
                var channel = await _channelRepository.GetAsync(id);
                
                channel.SetActive(isActive);
                await _channelRepository.UpdateAsync(channel);
                
                return ObjectMapper.Map<MarketingChannelPerformance, MarketingChannelDto>(channel);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ChannelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error setting channel active state: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error setting active state for marketing channel with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.ManageMetrics)]
        public async Task<ChannelMetricDto> AddMetricAsync(Guid channelId, CreateChannelMetricDto input)
        {
            try
            {
                // Add metric using domain service
                var metric = await _channelPerformanceTracker.AddMetricAsync(
                    channelId,
                    input.Name,
                    input.Type,
                    input.InitialValue,
                    input.TargetValue);
                
                return ObjectMapper.Map<ChannelMetric, ChannelMetricDto>(metric);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ChannelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error adding metric: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding metric to channel with ID {ChannelId}", channelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.ManageMetrics)]
        public async Task<ChannelMetricDto> UpdateMetricValueAsync(Guid channelId, Guid metricId, UpdateChannelMetricDto input)
        {
            try
            {
                // Update metric using domain service
                await _channelPerformanceTracker.UpdateMetricValueAsync(
                    channelId,
                    metricId,
                    input.NewValue);
                
                // Get updated metric
                var metric = await _channelRepository.GetMetricAsync(channelId, metricId);
                
                return ObjectMapper.Map<ChannelMetric, ChannelMetricDto>(metric);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:MetricNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error updating metric: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating metric {MetricId} for channel {ChannelId}", metricId, channelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.View)]
        public async Task<List<ChannelMetricDto>> GetChannelMetricsAsync(Guid channelId)
        {
            try
            {
                var channel = await _channelRepository.GetWithMetricsAsync(channelId);
                
                if (channel == null)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ChannelNotFound"]);
                }
                
                return ObjectMapper.Map<List<ChannelMetric>, List<ChannelMetricDto>>(channel.Metrics.ToList());
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ChannelNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting metrics for channel {ChannelId}", channelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.View)]
        public async Task<List<ChannelMetricDto>> GetMetricsByNameAsync(string metricName)
        {
            try
            {
                var metrics = await _channelRepository.GetMetricsByNameAsync(metricName);
                return ObjectMapper.Map<List<ChannelMetric>, List<ChannelMetricDto>>(metrics);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting metrics by name {MetricName}", metricName);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.ManageMetrics)]
        public async Task<MarketingChannelDto> UpdateCostPerAcquisitionAsync(Guid channelId, UpdateCostPerAcquisitionDto input)
        {
            try
            {
                // Update CPA using domain service
                await _channelPerformanceTracker.UpdateCostPerAcquisitionAsync(
                    channelId,
                    input.Cost,
                    input.Acquisitions,
                    input.PeriodStart,
                    input.PeriodEnd);
                
                // Get updated channel
                var channel = await _channelRepository.GetWithMetricsAsync(channelId);
                
                return ObjectMapper.Map<MarketingChannelPerformance, MarketingChannelDto>(channel);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ChannelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error updating CPA: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating CPA for channel {ChannelId}", channelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.ManageMetrics)]
        public async Task<MarketingChannelDto> UpdateReturnOnAdSpendAsync(Guid channelId, UpdateReturnOnAdSpendDto input)
        {
            try
            {
                // Update ROAS using domain service
                await _channelPerformanceTracker.UpdateReturnOnAdSpendAsync(
                    channelId,
                    input.AdSpend,
                    input.Revenue,
                    input.PeriodStart,
                    input.PeriodEnd);
                
                // Get updated channel
                var channel = await _channelRepository.GetWithMetricsAsync(channelId);
                
                return ObjectMapper.Map<MarketingChannelPerformance, MarketingChannelDto>(channel);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ChannelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error updating ROAS: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating ROAS for channel {ChannelId}", channelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.View)]
        public async Task<List<MarketingChannelDto>> GetChannelsByTypeAsync(MarketingChannelType type)
        {
            try
            {
                var channels = await _channelRepository.GetByTypeAsync(type);
                return ObjectMapper.Map<List<MarketingChannelPerformance>, List<MarketingChannelDto>>(channels);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting channels by type {ChannelType}", type);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.MarketingChannel.View)]
        public async Task<List<MarketingChannelDto>> GetActiveChannelsAsync()
        {
            try
            {
                var channels = await _channelRepository.GetActiveChannelsAsync();
                return ObjectMapper.Map<List<MarketingChannelPerformance>, List<MarketingChannelDto>>(channels);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting active channels");
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }
    }
}
