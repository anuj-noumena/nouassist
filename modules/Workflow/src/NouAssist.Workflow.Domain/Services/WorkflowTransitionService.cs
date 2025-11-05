using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.Events;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Workflow.Services
{
    public class WorkflowTransitionService : DomainService
    {
        public TransitionValidationResult ValidateTransition(
            Aggregates.Workflow workflow,
            WorkflowTransition transition,
            Guid currentUserId,
            List<string> userRoles,
            string comment = null)
        {
            var result = new TransitionValidationResult { IsValid = true };

            // Validate user permissions
            if (!transition.CanUserPerform(currentUserId, userRoles))
            {
                result.IsValid = false;
                result.Errors.Add("User does not have permission to perform this transition.");
            }

            // Validate comment requirement
            if (transition.RequiresComment && string.IsNullOrWhiteSpace(comment))
            {
                result.IsValid = false;
                result.Errors.Add("This transition requires a comment.");
            }

            // Validate status exists
            try
            {
                workflow.GetStatus(transition.ToStatusId);
            }
            catch (BusinessException)
            {
                result.IsValid = false;
                result.Errors.Add("Target status does not exist.");
            }

            return result;
        }

        public WorkflowTransitionExecutedEto CreateTransitionEvent(
            Guid taskId,
            Aggregates.Workflow workflow,
            WorkflowTransition transition,
            Guid executedBy,
            Guid? tenantId,
            string comment = null)
        {
            return new WorkflowTransitionExecutedEto
            {
                TaskId = taskId,
                WorkflowId = workflow.Id,
                TransitionId = transition.Id,
                FromStatusId = transition.FromStatusId,
                ToStatusId = transition.ToStatusId,
                ExecutedBy = executedBy,
                TenantId = tenantId,
                ExecutedAt = Clock.Now,
                Comment = comment
            };
        }

        public List<WorkflowAction> GetOrderedActions(WorkflowTransition transition)
        {
            return transition.Actions
                .Where(a => a.IsEnabled)
                .OrderBy(a => a.OrderIndex)
                .ToList();
        }
    }

    public class TransitionValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
