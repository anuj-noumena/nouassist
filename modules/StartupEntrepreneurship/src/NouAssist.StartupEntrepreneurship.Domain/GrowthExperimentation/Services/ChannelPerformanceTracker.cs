namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services
{
    /// <summary>
    /// Implementation of the channel performance tracker service
    /// </summary>
    public class ChannelPerformanceTracker : DomainService, IChannelPerformanceTracker
    {
        private readonly IMarketingChannelPerformanceRepository _channelRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<ChannelPerformanceTracker> _logger;

        public ChannelPerformanceTracker(
            IMarketingChannelPerformanceRepository channelRepository,
            IGuidGenerator guidGenerator,
            ILogger<ChannelPerformanceTracker> logger)
        {
            _channelRepository = channelRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<MarketingChannelPerformance> CreateChannelAsync(
            string name,
            string description,
            MarketingChannelType type,
            ChannelDetails details)
        {
            try
            {
                var channel = MarketingChannelPerformance.Create(
                    _guidGenerator.Create(),
                    name,
                    description,
                    type,
                    details,
                    CurrentTenant.Id);

                await _channelRepository.InsertAsync(channel);

                _logger.LogInformation(
                    "Created new marketing channel {ChannelId} with name {ChannelName}",
                    channel.Id,
                    channel.Name);

                return channel;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create marketing channel with name {ChannelName}",
                    name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ChannelMetric> AddMetricAsync(
            Guid channelId,
            string name,
            MetricType type,
            double initialValue,
            double targetValue)
        {
            try
            {
                var channel = await _channelRepository.GetAsync(channelId);

                if (channel == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:ChannelNotFound")
                        .WithData("channelId", channelId);
                }

                var metric = channel.AddMetric(
                    _guidGenerator.Create(),
                    name,
                    type,
                    initialValue,
                    targetValue,
                    Clock.Now);

                await _channelRepository.UpdateAsync(channel);

                _logger.LogInformation(
                    "Added metric {MetricName} to channel {ChannelId}",
                    name,
                    channelId);

                return metric;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:ChannelNotFound")
                    .WithData("channelId", channelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to add metric {MetricName} to channel {ChannelId}",
                    name,
                    channelId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateMetricValueAsync(
            Guid channelId,
            Guid metricId,
            double newValue)
        {
            try
            {
                var channel = await _channelRepository.GetWithMetricsAsync(channelId);

                if (channel == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:ChannelNotFound")
                        .WithData("channelId", channelId);
                }

                channel.UpdateMetricValue(metricId, newValue, Clock.Now);
                await _channelRepository.UpdateAsync(channel);

                _logger.LogInformation(
                    "Updated metric {MetricId} value to {Value} for channel {ChannelId}",
                    metricId,
                    newValue,
                    channelId);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:ChannelNotFound")
                    .WithData("channelId", channelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update metric {MetricId} for channel {ChannelId}",
                    metricId,
                    channelId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateCostPerAcquisitionAsync(
            Guid channelId,
            decimal cost,
            int acquisitions,
            DateTime periodStart,
            DateTime periodEnd)
        {
            try
            {
                var channel = await _channelRepository.GetAsync(channelId);

                if (channel == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:ChannelNotFound")
                        .WithData("channelId", channelId);
                }

                var cpa = new CostPerAcquisition(cost, acquisitions, periodStart, periodEnd);
                channel.UpdateCostPerAcquisition(cpa);
                await _channelRepository.UpdateAsync(channel);

                _logger.LogInformation(
                    "Updated CPA for channel {ChannelId} to {Value}",
                    channelId,
                    cpa.Value);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:ChannelNotFound")
                    .WithData("channelId", channelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update CPA for channel {ChannelId}",
                    channelId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateReturnOnAdSpendAsync(
            Guid channelId,
            decimal adSpend,
            decimal revenue,
            DateTime periodStart,
            DateTime periodEnd)
        {
            try
            {
                var channel = await _channelRepository.GetAsync(channelId);

                if (channel == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:ChannelNotFound")
                        .WithData("channelId", channelId);
                }

                var roas = new ReturnOnAdSpend(adSpend, revenue, periodStart, periodEnd);
                channel.UpdateReturnOnAdSpend(roas);
                await _channelRepository.UpdateAsync(channel);

                _logger.LogInformation(
                    "Updated ROAS for channel {ChannelId} to {Value}",
                    channelId,
                    roas.Value);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:ChannelNotFound")
                    .WithData("channelId", channelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update ROAS for channel {ChannelId}",
                    channelId);
                throw;
            }
        }
    }
}
