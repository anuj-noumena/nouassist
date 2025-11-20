using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels
{
    public interface IMarketingChannelAppService : IApplicationService
    {
        // Basic CRUD operations
        Task<MarketingChannelDto> GetAsync(Guid id);
        Task<PagedResultDto<MarketingChannelDto>> GetListAsync(GetMarketingChannelListDto input);
        Task<MarketingChannelDto> CreateAsync(CreateMarketingChannelDto input);
        Task<MarketingChannelDto> UpdateAsync(Guid id, UpdateMarketingChannelDto input);
        Task DeleteAsync(Guid id);
        
        // Channel state operations
        Task<MarketingChannelDto> SetActiveAsync(Guid id, bool isActive);
        
        // Metrics operations
        Task<ChannelMetricDto> AddMetricAsync(Guid channelId, CreateChannelMetricDto input);
        Task<ChannelMetricDto> UpdateMetricValueAsync(Guid channelId, Guid metricId, UpdateChannelMetricDto input);
        Task<List<ChannelMetricDto>> GetChannelMetricsAsync(Guid channelId);
        Task<List<ChannelMetricDto>> GetMetricsByNameAsync(string metricName);
        
        // Financial metrics operations
        Task<MarketingChannelDto> UpdateCostPerAcquisitionAsync(Guid channelId, UpdateCostPerAcquisitionDto input);
        Task<MarketingChannelDto> UpdateReturnOnAdSpendAsync(Guid channelId, UpdateReturnOnAdSpendDto input);
        
        // Channel analytics
        Task<List<MarketingChannelDto>> GetChannelsByTypeAsync(MarketingChannelType type);
        Task<List<MarketingChannelDto>> GetActiveChannelsAsync();
    }
}
