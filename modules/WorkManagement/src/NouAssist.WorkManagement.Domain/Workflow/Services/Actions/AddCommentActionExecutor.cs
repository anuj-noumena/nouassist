using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Executes comment addition actions.
    /// Follows Single Responsibility Principle - only handles comment additions
    /// Note: This increments the comment count. Actual comment creation should be handled
    /// by a separate comment service/entity that stores the comment text.
    /// </summary>
    public class AddCommentActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.AddComment;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var commentText = action.Configuration.GetValueOrDefault("comment")?.ToString();

                if (string.IsNullOrWhiteSpace(commentText))
                {
                    return ActionExecutionResult.Failure("Comment text is required");
                }

                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                // Increment comment count - actual comment storage would be handled by a comment service
                taskInstance.IncrementCommentsCount();

                await taskInstanceRepo.UpdateAsync(taskInstance);

                Logger.LogInformation(
                    "Added workflow comment to task instance {TaskInstanceId}. Total comments: {CommentCount}",
                    context.TaskInstanceId, taskInstance.CommentsCount);

                var resultData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["commentText"] = commentText,
                    ["totalComments"] = taskInstance.CommentsCount,
                    ["addedAt"] = Clock.Now,
                    ["addedBy"] = context.ExecutedBy
                };

                // Note: In a complete implementation, you would also:
                // 1. Store the actual comment in a Comment entity/table
                // 2. Link it to the task instance
                // 3. Optionally trigger notifications to watchers

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during comment addition");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing comment addition action");
                return ActionExecutionResult.Failure($"Comment addition failed: {ex.Message}");
            }
        }
    }
}
