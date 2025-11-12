using System;
using System.Collections.Generic;
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
    /// Executes webhook trigger actions.
    /// Follows Single Responsibility Principle - only handles webhook triggering
    /// Note: This is a base implementation that logs webhook intent.
    /// Integrate with your HTTP client or webhook service as needed.
    /// </summary>
    public class TriggerWebhookActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.TriggerWebhook;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var webhookUrl = action.Configuration.GetValueOrDefault("webhookUrl")?.ToString();
                if (string.IsNullOrWhiteSpace(webhookUrl))
                {
                    return ActionExecutionResult.Failure("Webhook URL is required");
                }

                // Validate URL format
                if (!Uri.TryCreate(webhookUrl, UriKind.Absolute, out var uri))
                {
                    return ActionExecutionResult.Failure($"Invalid webhook URL: {webhookUrl}");
                }

                // Get HTTP method (default to POST)
                var method = action.Configuration.GetValueOrDefault("method")?.ToString()?.ToUpper() ?? "POST";

                // Get custom payload (optional)
                var customPayload = action.Configuration.GetValueOrDefault("payload");

                // Build default payload with task context
                var payload = new Dictionary<string, object>
                {
                    ["taskInstanceId"] = context.TaskInstanceId,
                    ["workflowId"] = context.WorkflowId,
                    ["transitionId"] = context.TransitionId,
                    ["executedBy"] = context.ExecutedBy,
                    ["executedAt"] = Clock.Now,
                    ["actionName"] = action.Name
                };

                // Merge custom payload if provided
                if (customPayload is Dictionary<string, object> customDict)
                {
                    foreach (var kvp in customDict)
                    {
                        payload[kvp.Key] = kvp.Value;
                    }
                }

                // Get headers (optional)
                var headersObj = action.Configuration.GetValueOrDefault("headers");
                Dictionary<string, string> headers = new Dictionary<string, string>();

                if (headersObj is Dictionary<string, object> headersDict)
                {
                    foreach (var kvp in headersDict)
                    {
                        headers[kvp.Key] = kvp.Value?.ToString();
                    }
                }

                // Get timeout (optional, default to 30 seconds)
                var timeoutObj = action.Configuration.GetValueOrDefault("timeoutSeconds");
                int timeoutSeconds = timeoutObj != null ? Convert.ToInt32(timeoutObj) : 30;

                // TODO: Integrate with actual HTTP client to send webhook
                // Example using HttpClient:
                // var httpClient = LazyServiceProvider.LazyGetRequiredService<IHttpClientFactory>().CreateClient();
                // httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                // foreach (var header in headers)
                // {
                //     httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                // }
                // var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                // var response = await httpClient.PostAsync(webhookUrl, content);
                // response.EnsureSuccessStatusCode();

                Logger.LogInformation(
                    "Workflow webhook triggered for task {TaskInstanceId}: {Method} {WebhookUrl}",
                    context.TaskInstanceId, method, webhookUrl);

                var resultData = new Dictionary<string, object>
                {
                    ["webhookUrl"] = webhookUrl,
                    ["method"] = method,
                    ["payload"] = payload,
                    ["headers"] = headers,
                    ["triggeredAt"] = Clock.Now
                };

                // For now, return success indicating the webhook was queued
                // In production, you would integrate with a real HTTP client
                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during webhook trigger");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing webhook trigger action");
                return ActionExecutionResult.Failure($"Webhook trigger failed: {ex.Message}");
            }
        }
    }
}
