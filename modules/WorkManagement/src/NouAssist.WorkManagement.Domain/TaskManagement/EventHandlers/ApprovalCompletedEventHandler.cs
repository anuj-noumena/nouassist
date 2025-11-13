using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.Approval.Events;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.WorkManagement.TaskManagement.EventHandlers
{
    /// <summary>
    /// Handles approval workflow completion events and updates task instances.
    /// This completes the bidirectional integration between tasks and approvals.
    /// </summary>
    public class ApprovalCompletedEventHandler :
        IDistributedEventHandler<ApprovalWorkflowCompletedEto>,
        ITransientDependency
    {
        private readonly ITaskInstanceRepository _taskInstanceRepository;
        private readonly ILogger<ApprovalCompletedEventHandler> _logger;

        public ApprovalCompletedEventHandler(
            ITaskInstanceRepository taskInstanceRepository,
            ILogger<ApprovalCompletedEventHandler> logger)
        {
            _taskInstanceRepository = taskInstanceRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public async Task HandleEventAsync(ApprovalWorkflowCompletedEto eventData)
        {
            try
            {
                // Only process if the approval is for a TaskInstance
                if (eventData.EntityType != "TaskInstance")
                {
                    _logger.LogDebug(
                        "Skipping approval completion for entity type {EntityType}",
                        eventData.EntityType);
                    return;
                }

                // Load the task instance
                var taskInstance = await _taskInstanceRepository.GetAsync(eventData.EntityId);

                // Verify this task has the approval request linked
                if (taskInstance.ApprovalRequestId != eventData.RequestId)
                {
                    _logger.LogWarning(
                        "Task instance {TaskInstanceId} approval request ID mismatch. Expected {Expected}, got {Actual}",
                        eventData.EntityId,
                        taskInstance.ApprovalRequestId,
                        eventData.RequestId);
                    return;
                }

                _logger.LogInformation(
                    "Task instance {TaskInstanceId} approval {RequestId} was {Status}",
                    eventData.EntityId, 
                    eventData.RequestId,
                    eventData.IsApproved ? "approved" : "rejected");

                // If rejected, you might want to clear the approval link or handle differently
                if (!eventData.IsApproved)
                {
                    // Optional: Clear the approval link if rejected
                    // taskInstance.ClearApprovalLink();
                    
                    _logger.LogInformation(
                        "Task instance {TaskInstanceId} approval was rejected. Manual intervention may be required.",
                        eventData.EntityId);
                }

                // The approval status is tracked in the Approval module
                // TaskInstance only maintains the link via ApprovalRequestId
                // No need to update TaskInstance unless you want to add more behavior

                await _taskInstanceRepository.UpdateAsync(taskInstance);

                _logger.LogInformation(
                    "Successfully processed approval completion for task instance {TaskInstanceId}",
                    eventData.EntityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling approval completion event for entity {EntityId}",
                    eventData.EntityId);
                throw;
            }
        }
    }
}