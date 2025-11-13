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
using Volo.Abp.Users;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// IMPROVED: UpdateFieldActionExecutor with type safety
    /// </summary>
    public class UpdateFieldActionExecutor : WorkflowActionExecutorBase, ITransientDependency
    {
        public override bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.UpdateField ||
                   action?.ActionType == WorkflowActionType.SetCustomField;
        }

        protected override ConfigurationValidationResult ValidateConfiguration(WorkflowAction action)
        {
            if (!action.Configuration.ContainsKey("fieldName"))
            {
                return ConfigurationValidationResult.Invalid("fieldName is required");
            }

            if (!action.Configuration.ContainsKey("fieldValue"))
            {
                return ConfigurationValidationResult.Invalid("fieldValue is required");
            }

            return ConfigurationValidationResult.Valid();
        }

        protected override async Task<ActionExecutionResult> ExecuteInternalAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            var fieldName = GetRequiredConfig<string>(action, "fieldName");
            var fieldValue = action.Configuration["fieldValue"];

            var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
            var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

            var oldValue = taskInstance.CustomFields.GetValue(fieldName);
            taskInstance.SetCustomFieldValue(fieldName, fieldValue);

            await taskInstanceRepo.UpdateAsync(taskInstance);

            return ActionExecutionResult.Success(new Dictionary<string, object>
            {
                ["fieldName"] = fieldName,
                ["oldValue"] = oldValue,
                ["newValue"] = fieldValue,
                ["updatedAt"] = Clock.Now
            });
        }
    }
}
