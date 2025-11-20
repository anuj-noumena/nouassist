namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.BackgroundServices
{
    /// <summary>
    /// Background service for Product Discovery maintenance tasks
    /// </summary>
    public class ProductDiscoveryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProductDiscoveryBackgroundService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromHours(6); // Run every 6 hours

        public ProductDiscoveryBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<ProductDiscoveryBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWorkAsync();
                    await Task.Delay(_period, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing background service");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        private async Task DoWorkAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            
            try
            {
                // Update stale interview statuses
                await UpdateStaleInterviewStatusesAsync(scope.ServiceProvider);

                // Process unprocessed transcripts
                await ProcessUnprocessedTranscriptsAsync(scope.ServiceProvider);

                // Update customer segment sample sizes
                await UpdateCustomerSegmentSampleSizesAsync(scope.ServiceProvider);

                // Clean up old validation attempts
                await CleanupOldValidationAttemptsAsync(scope.ServiceProvider);

                _logger.LogInformation("Product Discovery background maintenance completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Product Discovery background maintenance");
            }
        }

        private async Task UpdateStaleInterviewStatusesAsync(IServiceProvider serviceProvider)
        {
            var repository = serviceProvider.GetRequiredService<ICustomerInterviewRepository>();
            var cutoffDate = DateTime.UtcNow.AddHours(-2);

            // Find interviews that should have started but are still scheduled
            var staleScheduled = await repository.GetListAsync(
                i => i.Status == InterviewStatus.Scheduled && i.ScheduledDate <= cutoffDate);

            foreach (var interview in staleScheduled)
            {
                // Auto-cancel interviews that are more than 2 hours past scheduled time
                interview.CancelInterview();
                await repository.UpdateAsync(interview);
                _logger.LogInformation("Auto-cancelled stale interview: {InterviewId}", interview.Id);
            }
        }

        private async Task ProcessUnprocessedTranscriptsAsync(IServiceProvider serviceProvider)
        {
            // This would involve AI/ML processing of transcripts to extract insights
            // For now, just mark them as processed if they're old enough
            _logger.LogDebug("Transcript processing would be implemented here");
        }

        private async Task UpdateCustomerSegmentSampleSizesAsync(IServiceProvider serviceProvider)
        {
            var segmentRepository = serviceProvider.GetRequiredService<ICustomerSegmentRepository>();
            var interviewRepository = serviceProvider.GetRequiredService<ICustomerInterviewRepository>();

            var segments = await segmentRepository.GetListAsync(s => true);

            foreach (var segment in segments)
            {
                var completedInterviews = await interviewRepository.CountAsync(
                    i => i.CustomerSegmentId == segment.Id && i.Status == InterviewStatus.Completed);

                if (segment.ActualSampleSize != completedInterviews)
                {
                    // This would require adding a method to update actual sample size
                    // For now, just log the discrepancy
                    _logger.LogWarning("Sample size mismatch for segment {SegmentId}: Expected {Expected}, Actual {Actual}",
                        segment.Id, segment.ActualSampleSize, completedInterviews);
                }
            }
        }

        private async Task CleanupOldValidationAttemptsAsync(IServiceProvider serviceProvider)
        {
            // Clean up validation attempts that have been stale for too long
            _logger.LogDebug("Validation attempt cleanup would be implemented here");
            await Task.CompletedTask;
        }
    }
}
