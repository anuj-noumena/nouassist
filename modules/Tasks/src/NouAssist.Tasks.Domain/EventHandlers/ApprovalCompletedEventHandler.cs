using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Approval.Events;
using NouAssist.Tasks.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.Tasks.EventHandlers
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

                if (taskInstance.Approval == null)
                {
                    _logger.LogWarning(
                        "Task instance {TaskInstanceId} received approval completion but has no approval tracking",
                        eventData.EntityId);
                    return;
                }

                // Update approval status
                if (eventData.IsApproved)
                {
                    taskInstance.Approval = taskInstance.Approval.MarkAsApproved(
                        eventData.CompletedAt,
                        eventData.RequestId);

                    _logger.LogInformation(
                        "Task instance {TaskInstanceId} approval {RequestId} was approved",
                        eventData.EntityId, eventData.RequestId);
                }
                else
                {
                    taskInstance.Approval = taskInstance.Approval.MarkAsRejected(
                        eventData.CompletedAt,
                        eventData.RequestId);

                    _logger.LogInformation(
                        "Task instance {TaskInstanceId} approval {RequestId} was rejected",
                        eventData.EntityId, eventData.RequestId);
                }

                // Save the updated task instance
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
