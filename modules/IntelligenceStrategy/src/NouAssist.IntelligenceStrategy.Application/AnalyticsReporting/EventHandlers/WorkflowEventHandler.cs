using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.Analytics.EventHandlers
{
    public class WorkflowEventHandler : 
        IDistributedEventHandler<WorkflowInstanceCompletedEto>,
        IDistributedEventHandler<WorkflowActivityCompletedEto>,
        ITransientDependency
    {
        private readonly ILogger<WorkflowEventHandler> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public WorkflowEventHandler(
            ILogger<WorkflowEventHandler> logger,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }
        
        public async Task HandleEventAsync(WorkflowInstanceCompletedEto eventData)
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    _logger.LogInformation("Processing workflow instance completed event: {WorkflowId} - Duration: {DurationSeconds}s", 
                        eventData.WorkflowId, eventData.DurationInSeconds);
                    
                    // Here we would update the relevant aggregated data view
                    // This would involve:
                    // 1. Find the data source that consumes workflow events
                    // 2. Find the relevant aggregated view
                    // 3. Update the view with the workflow completion data
                    
                    // For demonstration purposes, we'll just log the event
                    
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing workflow instance completed event: {WorkflowId}", eventData.WorkflowId);
            }
        }
        
        public async Task HandleEventAsync(WorkflowActivityCompletedEto eventData)
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    _logger.LogInformation("Processing workflow activity completed event: {ActivityId} in workflow {WorkflowId} - Duration: {DurationSeconds}s", 
                        eventData.ActivityId, eventData.WorkflowId, eventData.DurationInSeconds);
                    
                    // Here we would update bottleneck analysis data
                    // This would involve:
                    // 1. Find the data source that consumes workflow events
                    // 2. Find the relevant aggregated view for bottleneck analysis
                    // 3. Update the view with the activity completion data
                    
                    // For demonstration purposes, we'll just log the event
                    
                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing workflow activity completed event: {ActivityId}", eventData.ActivityId);
            }
        }
    }
}
