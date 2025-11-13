using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Executes task linking/dependency actions.
    /// Follows Single Responsibility Principle - only handles task linking
    /// </summary>
    public class LinkTaskActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.LinkTask;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var linkedTaskIdObj = action.Configuration.GetValueOrDefault("taskId");
                if (linkedTaskIdObj == null)
                {
                    return ActionExecutionResult.Failure("Linked task ID is required");
                }

                var linkedTaskId = Guid.Parse(linkedTaskIdObj.ToString());

                // Get dependency type, default to FinishToStart
                var dependencyTypeObj = action.Configuration.GetValueOrDefault("dependencyType");
                DependencyType dependencyType = DependencyType.FinishToStart;

                if (dependencyTypeObj != null)
                {
                    if (Enum.TryParse<DependencyType>(dependencyTypeObj.ToString(), out var parsedType))
                    {
                        dependencyType = parsedType;
                    }
                    else if (int.TryParse(dependencyTypeObj.ToString(), out var typeInt))
                    {
                        dependencyType = (DependencyType)typeInt;
                    }
                }

                // Get lag minutes (optional)
                var lagMinutesObj = action.Configuration.GetValueOrDefault("lagMinutes");
                int lagMinutes = lagMinutesObj != null ? Convert.ToInt32(lagMinutesObj) : 0;

                // Get blocking flag (optional, defaults to true)
                var isBlockingObj = action.Configuration.GetValueOrDefault("isBlocking");
                bool isBlocking = isBlockingObj != null ? Convert.ToBoolean(isBlockingObj) : true;

                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                // Verify the linked task exists using proper AnyAsync signature
                var linkedTaskExists = await taskInstanceRepo.AnyAsync(t => t.Id == linkedTaskId);
                if (!linkedTaskExists)
                {
                    return ActionExecutionResult.Failure($"Linked task instance {linkedTaskId} does not exist");
                }

                // Add the dependency
                taskInstance.AddDependency(linkedTaskId, dependencyType, lagMinutes, isBlocking);

                await taskInstanceRepo.UpdateAsync(taskInstance);

                Logger.LogInformation(
                    "Linked task instance {TaskInstanceId} to {LinkedTaskId} with dependency type {DependencyType}",
                    context.TaskInstanceId, linkedTaskId, dependencyType);

                var resultData = new Dictionary<string, object>
                {
                    ["linkedTaskId"] = linkedTaskId,
                    ["dependencyType"] = dependencyType.ToString(),
                    ["lagMinutes"] = lagMinutes,
                    ["isBlocking"] = isBlocking,
                    ["linkedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during task linking");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing task linking action");
                return ActionExecutionResult.Failure($"Task linking failed: {ex.Message}");
            }
        }
    }
}