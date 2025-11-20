using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Analytics.Interfaces.Services;
using NouAssist.Analytics.Repositories;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace NouAssist.Analytics.Jobs
{
    public class ScheduledReportGenerationJob : AsyncBackgroundJob<ScheduledReportGenerationJobArgs>, ITransientDependency
    {
        private readonly IReportManager _reportManager;
        private readonly IReportConfigurationRepository _reportRepository;
        private readonly IClock _clock;
        private readonly ILogger<ScheduledReportGenerationJob> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public ScheduledReportGenerationJob(
            IReportManager reportManager,
            IReportConfigurationRepository reportRepository,
            IClock clock,
            ILogger<ScheduledReportGenerationJob> logger,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _reportManager = reportManager;
            _reportRepository = reportRepository;
            _clock = clock;
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }
        
        public override async Task ExecuteAsync(ScheduledReportGenerationJobArgs args)
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    var report = await _reportRepository.GetAsync(args.ReportConfigurationId);
                    
                    _logger.LogInformation("Generating scheduled report: {ReportName} (ID: {ReportId})",
                        report.Name, report.Id);
                    
                    // Generate the report
                    var storagePath = await _reportManager.GenerateReportAsync(
                        report.Id,
                        args.ParameterValues,
                        null, // Use default format
                        null  // System-generated report
                    );
                    
                    _logger.LogInformation("Scheduled report generated successfully: {ReportName}, stored at: {StoragePath}",
                        report.Name, storagePath);
                    
                    // Update the next run time based on the cron expression
                    // In a real implementation, this would calculate the next run time based on the cron expression
                    // For simplicity, we'll just add a day
                    var newSchedule = report.Schedule;
                    newSchedule.RecordRun(_clock.Now);
                    await _reportManager.UpdateReportScheduleAsync(report.Id, newSchedule);
                    
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating scheduled report: {ReportId}", args.ReportConfigurationId);
                throw;
            }
        }
    }
    
    public class ScheduledReportGenerationJobArgs
    {
        public Guid ReportConfigurationId { get; set; }
        
        public Dictionary<string, string> ParameterValues { get; set; }
    }
}
