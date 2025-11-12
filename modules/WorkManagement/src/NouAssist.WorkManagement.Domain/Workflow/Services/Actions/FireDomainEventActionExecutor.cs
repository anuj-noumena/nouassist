using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Executes domain event firing actions.
    /// Follows Single Responsibility Principle - only handles domain event firing
    /// Uses ABP's event bus to publish domain events for other parts of the system to react to.
    /// </summary>
    public class FireDomainEventActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.FireDomainEvent;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var eventType = action.Configuration.GetValueOrDefault("eventType")?.ToString();
                if (string.IsNullOrWhiteSpace(eventType))
                {
                    return ActionExecutionResult.Failure("Event type is required");
                }

                // Get event data (optional)
                var eventDataObj = action.Configuration.GetValueOrDefault("eventData");
                Dictionary<string, object> eventData = new Dictionary<string, object>
                {
                    ["taskInstanceId"] = context.TaskInstanceId,
                    ["workflowId"] = context.WorkflowId,
                    ["transitionId"] = context.TransitionId,
                    ["executedBy"] = context.ExecutedBy,
                    ["triggeredAt"] = Clock.Now,
                    ["actionName"] = action.Name
                };

                if (eventDataObj is Dictionary<string, object> customData)
                {
                    foreach (var kvp in customData)
                    {
                        eventData[kvp.Key] = kvp.Value;
                    }
                }

                // Determine if this should be a local or distributed event
                var isDistributedObj = action.Configuration.GetValueOrDefault("isDistributed");
                bool isDistributed = isDistributedObj != null && Convert.ToBoolean(isDistributedObj);

                // Create a generic event object
                var genericEvent = new WorkflowTriggeredDomainEvent
                {
                    EventType = eventType,
                    TaskInstanceId = context.TaskInstanceId,
                    WorkflowId = context.WorkflowId,
                    ExecutedBy = context.ExecutedBy,
                    TenantId = CurrentTenant.Id,
                    EventData = eventData,
                    TriggeredAt = Clock.Now
                };

                if (isDistributed)
                {
                    // Publish distributed event (across services/microservices)
                    var distributedEventBus = LazyServiceProvider.LazyGetRequiredService<IDistributedEventBus>();
                    await distributedEventBus.PublishAsync(genericEvent);

                    Logger.LogInformation(
                        "Published distributed domain event '{EventType}' for task {TaskInstanceId}",
                        eventType, context.TaskInstanceId);
                }
                else
                {
                    // Publish local event (within same process)
                    var localEventBus = LazyServiceProvider.LazyGetRequiredService<ILocalEventBus>();
                    await localEventBus.PublishAsync(genericEvent);

                    Logger.LogInformation(
                        "Published local domain event '{EventType}' for task {TaskInstanceId}",
                        eventType, context.TaskInstanceId);
                }

                var resultData = new Dictionary<string, object>
                {
                    ["eventType"] = eventType,
                    ["isDistributed"] = isDistributed,
                    ["eventData"] = eventData,
                    ["publishedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during domain event firing");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing domain event firing action");
                return ActionExecutionResult.Failure($"Domain event firing failed: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Generic domain event triggered by workflow actions
    /// </summary>
    public class WorkflowTriggeredDomainEvent
    {
        public string EventType { get; set; }
        public Guid TaskInstanceId { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid ExecutedBy { get; set; }
        public Guid? TenantId { get; set; }
        public Dictionary<string, object> EventData { get; set; }
        public DateTime TriggeredAt { get; set; }
    }
}
