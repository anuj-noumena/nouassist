using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.Workflow.Data;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Executes subtask creation actions.
    /// Follows Single Responsibility Principle - only handles subtask creation
    /// </summary>
    public class CreateSubtaskActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.CreateSubtask;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                // Required: Task blueprint ID
                var taskIdObj = action.Configuration.GetValueOrDefault("taskId");
                if (taskIdObj == null)
                {
                    return ActionExecutionResult.Failure("Task ID (blueprint) is required for subtask creation");
                }

                var taskId = Guid.Parse(taskIdObj.ToString());

                // Get parent task instance to inherit properties
                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var parentTaskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                // Get task blueprint to get default workflow
                var taskRepo = LazyServiceProvider.LazyGetRequiredService<ITaskRepository>();
                var taskBlueprint = await taskRepo.GetAsync(taskId);

                // Determine workflow to use
                Guid workflowId;
                if (taskBlueprint.DefaultWorkflowId.HasValue)
                {
                    workflowId = taskBlueprint.DefaultWorkflowId.Value;
                }
                else
                {
                    // Use parent's workflow
                    workflowId = parentTaskInstance.WorkflowId;
                }

                // Get initial workflow status
                var workflowRepo = LazyServiceProvider.LazyGetRequiredService<IWorkflowRepository>();
                var workflow = await workflowRepo.GetAsync(workflowId, includeDetails: true);
                var initialStatus = workflow.Statuses.FirstOrDefault(s => s.IsInitial);
                
                if (initialStatus == null)
                {
                    return ActionExecutionResult.Failure($"Workflow {workflowId} has no initial status defined");
                }

                // Optional: Override assignee (defaults to parent's assignee or specified in config)
                Guid? assigneeId = null;
                var assigneeIdObj = action.Configuration.GetValueOrDefault("assigneeId");
                if (assigneeIdObj != null)
                {
                    assigneeId = Guid.Parse(assigneeIdObj.ToString());
                }
                else
                {
                    assigneeId = parentTaskInstance.AssigneeId;
                }

                // Optional: Custom importance and urgency (0.0-1.0 range)
                var importanceObj = action.Configuration.GetValueOrDefault("importance");
                var urgencyObj = action.Configuration.GetValueOrDefault("urgency");

                double importance = importanceObj != null 
                    ? Convert.ToDouble(importanceObj) 
                    : taskBlueprint.DefaultImportance;
                    
                double urgency = urgencyObj != null 
                    ? Convert.ToDouble(urgencyObj) 
                    : taskBlueprint.DefaultUrgency;

                // Validate importance and urgency range
                if (importance < 0.0 || importance > 1.0)
                {
                    return ActionExecutionResult.Failure("Importance must be between 0.0 and 1.0");
                }
                
                if (urgency < 0.0 || urgency > 1.0)
                {
                    return ActionExecutionResult.Failure("Urgency must be between 0.0 and 1.0");
                }

                // Determine GTD organization (inherit from parent or task blueprint)
                Guid? projectId = parentTaskInstance.ProjectId;
                Guid? areaId = parentTaskInstance.AreaId ?? taskBlueprint.GetEffectiveAreaForInstance();
                Guid? contextId = parentTaskInstance.ContextId ?? taskBlueprint.GetEffectiveContextForInstance();

                // Create the subtask instance
                var guidGenerator = LazyServiceProvider.LazyGetRequiredService<IGuidGenerator>();
                var subtaskInstance = new TaskInstance(
                    id: guidGenerator.Create(),
                    taskId: taskId,
                    workflowId: workflowId,
                    workflowStatusId: initialStatus.Id,
                    ownerId: parentTaskInstance.OwnerId,
                    projectId: projectId,
                    areaId: areaId,
                    contextId: contextId,
                    importance: importance,
                    urgency: urgency,
                    tenantId: CurrentTenant.Id,
                    reporterId: context.ExecutedBy,
                    assigneeId: assigneeId
                );

                // Set hierarchy - this subtask is a child of the parent
                // Note: You'll need to implement hierarchy management on TaskInstance
                // For now, we'll track this in metadata or create a method
                
                // Initialize checklist from task blueprint if it has one
                if (taskBlueprint.HasChecklist)
                {
                    var checklistItemIds = taskBlueprint.ChecklistItems
                        .OrderBy(ci => ci.OrderIndex)
                        .Select(ci => ci.Id)
                        .ToList();
                    subtaskInstance.InitializeChecklist(checklistItemIds);
                }

                await taskInstanceRepo.InsertAsync(subtaskInstance);

                Logger.LogInformation(
                    "Created subtask {SubtaskId} from blueprint {TaskId} for parent task instance {ParentTaskInstanceId}",
                    subtaskInstance.Id, taskId, context.TaskInstanceId);

                var resultData = new Dictionary<string, object>
                {
                    ["subtaskId"] = subtaskInstance.Id,
                    ["parentTaskId"] = context.TaskInstanceId,
                    ["taskBlueprintId"] = taskId,
                    ["workflowId"] = workflowId,
                    ["initialStatusId"] = initialStatus.Id,
                    ["assigneeId"] = subtaskInstance.AssigneeId,
                    ["importance"] = importance,
                    ["urgency"] = urgency,
                    ["quadrant"] = subtaskInstance.Priority.Quadrant.ToString(),
                    ["projectId"] = projectId,
                    ["areaId"] = areaId,
                    ["contextId"] = contextId,
                    ["hasChecklist"] = taskBlueprint.HasChecklist,
                    ["createdAt"] = Clock.Now,
                    ["createdBy"] = context.ExecutedBy
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during subtask creation");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing subtask creation action");
                return ActionExecutionResult.Failure($"Subtask creation failed: {ex.Message}");
            }
        }
    }
}