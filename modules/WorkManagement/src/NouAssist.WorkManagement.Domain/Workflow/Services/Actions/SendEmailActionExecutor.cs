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
    /// Executes email sending actions.
    /// Follows Single Responsibility Principle - only handles sending emails
    /// Note: This is a base implementation that logs email intent.
    /// Integrate with your email service (e.g., ABP Email Sender, SendGrid, etc.) as needed.
    /// </summary>
    public class SendEmailActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.SendEmail;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var subject = action.Configuration.GetValueOrDefault("subject")?.ToString();
                var body = action.Configuration.GetValueOrDefault("body")?.ToString();
                var recipientsObj = action.Configuration.GetValueOrDefault("recipients");

                if (string.IsNullOrWhiteSpace(subject))
                {
                    return ActionExecutionResult.Failure("Email subject is required");
                }

                if (string.IsNullOrWhiteSpace(body))
                {
                    return ActionExecutionResult.Failure("Email body is required");
                }

                // Extract recipient email addresses
                List<string> recipients = new List<string>();

                if (recipientsObj is IEnumerable<object> recipientList)
                {
                    recipients = recipientList
                        .Select(r => r.ToString())
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .ToList();
                }
                else if (recipientsObj != null)
                {
                    // Single recipient
                    recipients.Add(recipientsObj.ToString());
                }

                if (!recipients.Any())
                {
                    return ActionExecutionResult.Failure("At least one recipient email address is required");
                }

                // Optional: Get template name if using email templates
                var templateName = action.Configuration.GetValueOrDefault("template")?.ToString();

                // Optional: Get CC and BCC
                var ccObj = action.Configuration.GetValueOrDefault("cc");
                var bccObj = action.Configuration.GetValueOrDefault("bcc");

                List<string> cc = new List<string>();
                List<string> bcc = new List<string>();

                if (ccObj is IEnumerable<object> ccList)
                {
                    cc = ccList.Select(c => c.ToString()).Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
                }

                if (bccObj is IEnumerable<object> bccList)
                {
                    bcc = bccList.Select(b => b.ToString()).Where(b => !string.IsNullOrWhiteSpace(b)).ToList();
                }

                // TODO: Integrate with actual email service
                // Example using ABP:
                // var emailSender = LazyServiceProvider.LazyGetRequiredService<IEmailSender>();
                // await emailSender.SendAsync(recipients.First(), subject, body, isBodyHtml: true);

                Logger.LogInformation(
                    "Workflow email for task {TaskInstanceId}: Sending '{Subject}' to {RecipientCount} recipients",
                    context.TaskInstanceId, subject, recipients.Count);

                var resultData = new Dictionary<string, object>
                {
                    ["recipients"] = recipients,
                    ["subject"] = subject,
                    ["bodyPreview"] = body.Length > 100 ? body.Substring(0, 100) + "..." : body,
                    ["template"] = templateName,
                    ["cc"] = cc,
                    ["bcc"] = bcc,
                    ["sentAt"] = Clock.Now,
                    ["sentBy"] = context.ExecutedBy
                };

                // For now, return success indicating the email was queued
                // In production, you would integrate with a real email service
                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during email sending");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing email sending action");
                return ActionExecutionResult.Failure($"Email sending failed: {ex.Message}");
            }
        }
    }
}
