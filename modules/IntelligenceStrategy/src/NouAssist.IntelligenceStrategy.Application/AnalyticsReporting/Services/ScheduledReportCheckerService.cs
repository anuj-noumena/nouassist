using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.Analytics.Jobs;
using NouAssist.Analytics.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace NouAssist.Analytics.Services
{
    public class ScheduledReportCheckerService : ITransientDependency
    {
        private readonly IReportConfigurationRepository _reportRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IClock _clock;
        private readonly ILogger<ScheduledReportCheckerService> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public ScheduledReportCheckerService(
            IReportConfigurationRepository reportRepository,
            IBackgroundJobManager backgroundJobManager,
            IClock clock,
            ILogger<ScheduledReportCheckerService> logger,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _reportRepository = reportRepository;
            _backgroundJobManager = backgroundJobManager;
            _clock = clock;
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }
        
        /// <summary>
        /// Checks for reports that are due to be generated and queues them as background jobs
        /// </summary>
        public async Task CheckAndQueueScheduledReportsAsync()
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    var now = _clock.Now;
                    var reports = await _reportRepository.GetScheduledReportsAsync(now);
                    
                    _logger.LogInformation("Found {ReportCount} scheduled reports due for generation", reports.Length);
                    
                    foreach (var report in reports)
                    {
                        _logger.LogInformation("Queueing scheduled report: {ReportName} (ID: {ReportId})",
                            report.Name, report.Id);
                        
                        // Create default parameter values from report parameters
                        var parameterValues = report.Parameters
                            .ToDictionary(
                                p => p.Name,
                                p => p.DefaultValue ?? string.Empty
                            );
                        
                        // Queue background job to generate the report
                        await _backgroundJobManager.EnqueueAsync(
                            new ScheduledReportGenerationJobArgs
                            {
                                ReportConfigurationId = report.Id,
                                ParameterValues = parameterValues
                            }
                        );
                    }
                    
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking and queueing scheduled reports");
            }
        }
    }
}
