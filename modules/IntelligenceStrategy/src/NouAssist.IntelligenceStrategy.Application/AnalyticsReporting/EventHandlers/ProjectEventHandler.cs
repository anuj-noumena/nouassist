using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.Analytics.EventHandlers
{
    public class ProjectEventHandler : 
        IDistributedEventHandler<TaskStatusUpdatedEto>,
        IDistributedEventHandler<ProjectProgressUpdatedEto>,
        ITransientDependency
    {
        private readonly ILogger<ProjectEventHandler> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public ProjectEventHandler(
            ILogger<ProjectEventHandler> logger,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }
        
        public async Task HandleEventAsync(TaskStatusUpdatedEto eventData)
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    _logger.LogInformation("Processing task status updated event: {TaskId} - New status: {Status}", 
                        eventData.TaskId, eventData.Status);
                    
                    // Here we would update the relevant aggregated data view
                    // This would involve:
                    // 1. Find the data source that consumes project events
                    // 2. Find the relevant aggregated view
                    // 3. Update the view with the new task status data
                    
                    // For demonstration purposes, we'll just log the event
                    
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing task status updated event: {TaskId}", eventData.TaskId);
            }
        }
        
        public async Task HandleEventAsync(ProjectProgressUpdatedEto eventData)
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    _logger.LogInformation("Processing project progress updated event: {ProjectId} - Progress: {Progress}%", 
                        eventData.ProjectId, eventData.ProgressPercentage);
                    
                    // Here we would update the relevant KPIs and aggregated data
                    // This would involve:
                    // 1. Find KPIs related to project progress
                    // 2. Update KPI values
                    // 3. Update relevant aggregated views
                    
                    // For demonstration purposes, we'll just log the event
                    
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing project progress updated event: {ProjectId}", eventData.ProjectId);
            }
        }
    }
}
