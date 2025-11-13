using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// IMPROVED: AssignUserActionExecutor with better validation
    /// </summary>
    public class AssignUserActionExecutor : WorkflowActionExecutorBase, ITransientDependency
    {
        public override bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.AssignToUser ||
                   action?.ActionType == WorkflowActionType.AssignToReporter ||
                   action?.ActionType == WorkflowActionType.AssignToCurrentUser ||
                   action?.ActionType == WorkflowActionType.ClearAssignee;
        }

        protected override ConfigurationValidationResult ValidateConfiguration(WorkflowAction action)
        {
            if (action.ActionType == WorkflowActionType.AssignToUser)
            {
                if (!action.Configuration.ContainsKey("userId"))
                {
                    return ConfigurationValidationResult.Invalid(
                        "AssignToUser requires 'userId' configuration");
                }
            }

            return ConfigurationValidationResult.Valid();
        }

        protected override async Task<ActionExecutionResult> ExecuteInternalAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
            var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

            Guid? targetUserId = action.ActionType switch
            {
                WorkflowActionType.AssignToUser => GetRequiredConfig<Guid>(action, "userId"),
                WorkflowActionType.AssignToReporter => taskInstance.ReporterId,
                WorkflowActionType.AssignToCurrentUser => context.ExecutedBy,
                WorkflowActionType.ClearAssignee => null,
                _ => throw new BusinessException("Invalid assignment action type")
            };

            if (action.ActionType == WorkflowActionType.ClearAssignee)
            {
                taskInstance.Unassign();
            }
            else if (targetUserId.HasValue)
            {
                taskInstance.Assign(targetUserId.Value);
            }
            else
            {
                return ActionExecutionResult.Failure("No target user ID found for assignment");
            }

            await taskInstanceRepo.UpdateAsync(taskInstance);

            return ActionExecutionResult.Success(new Dictionary<string, object>
            {
                ["assignedTo"] = targetUserId,
                ["assignedAt"] = Clock.Now
            });
        }
    }

   

    

    
}