using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Executes notification sending actions.
    /// Follows Single Responsibility Principle - only handles sending notifications
    /// Note: This is a base implementation that logs notification intent.
    /// Integrate with your notification service (e.g., SignalR, push notifications) as needed.
    /// </summary>
    public class SendNotificationActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.SendNotification;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var message = action.Configuration.GetValueOrDefault("message")?.ToString();
                if (string.IsNullOrWhiteSpace(message))
                {
                    return ActionExecutionResult.Failure("Notification message is required");
                }

                // Extract recipient user IDs
                var userIdsObj = action.Configuration.GetValueOrDefault("userIds");
                List<Guid> recipientUserIds = new List<Guid>();

                if (userIdsObj is IEnumerable<object> userIdList)
                {
                    recipientUserIds = userIdList
                        .Select(id => Guid.Parse(id.ToString()))
                        .ToList();
                }
                else if (userIdsObj != null)
                {
                    // Single user ID
                    recipientUserIds.Add(Guid.Parse(userIdsObj.ToString()));
                }

                if (!recipientUserIds.Any())
                {
                    return ActionExecutionResult.Failure("At least one recipient user ID is required");
                }

                // Optional: Get notification type/severity
                var notificationType = action.Configuration.GetValueOrDefault("type")?.ToString() ?? "Info";

                // Optional: Get notification title
                var title = action.Configuration.GetValueOrDefault("title")?.ToString()
                    ?? "Workflow Notification";

                // TODO: Integrate with actual notification service
                // Example: await _notificationService.SendAsync(recipientUserIds, title, message, notificationType);

                Logger.LogInformation(
                    "Workflow notification for task {TaskInstanceId}: Sending '{Title}' to {RecipientCount} users: {Message}",
                    context.TaskInstanceId, title, recipientUserIds.Count, message);

                var resultData = new Dictionary<string, object>
                {
                    ["recipientUserIds"] = recipientUserIds,
                    ["message"] = message,
                    ["title"] = title,
                    ["type"] = notificationType,
                    ["sentAt"] = Clock.Now,
                    ["sentBy"] = context.ExecutedBy
                };

                // For now, return success indicating the notification was queued
                // In production, you would integrate with a real notification service
                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during notification sending");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing notification sending action");
                return ActionExecutionResult.Failure($"Notification sending failed: {ex.Message}");
            }
        }
    }
}
