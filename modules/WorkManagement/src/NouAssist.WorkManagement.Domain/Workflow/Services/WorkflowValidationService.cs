// src/NouAssist.Workflow.Domain/Services/WorkflowValidationService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services
{
    public class WorkflowValidationService : DomainService
    {
        /// <summary>
        /// Validates that a workflow configuration is valid before activation
        /// </summary>
        public WorkflowValidationResult ValidateWorkflow(Aggregates.Workflow workflow)
        {
            var result = new WorkflowValidationResult { IsValid = true };

            // Must have at least one status
            if (!workflow.Statuses.Any())
            {
                result.IsValid = false;
                result.Errors.Add("Workflow must have at least one status.");
                return result;
            }

            // Must have exactly one initial status
            var initialStatuses = workflow.Statuses.Where(s => s.IsInitial).ToList();
            if (initialStatuses.Count == 0)
            {
                result.IsValid = false;
                result.Errors.Add("Workflow must have an initial status.");
            }
            else if (initialStatuses.Count > 1)
            {
                result.IsValid = false;
                result.Errors.Add("Workflow can only have one initial status.");
            }

            // Initial status must match workflow's InitialStatusId
            if (workflow.InitialStatusId != Guid.Empty)
            {
                var initialStatus = workflow.Statuses.FirstOrDefault(s => s.Id == workflow.InitialStatusId);
                if (initialStatus == null)
                {
                    result.IsValid = false;
                    result.Errors.Add("Workflow's initial status ID does not match any status.");
                }
            }

            // Must have at least one terminal status (Done or Cancelled)
            var terminalStatuses = workflow.Statuses.Where(s => s.IsTerminal).ToList();
            if (!terminalStatuses.Any())
            {
                result.AddWarning("Workflow should have at least one terminal status (Done/Cancelled).");
            }

            // All statuses must have at least one incoming or outgoing transition (except initial)
            foreach (var status in workflow.Statuses.Where(s => !s.IsInitial))
            {
                var hasIncoming = workflow.Transitions.Any(t => t.ToStatusId == status.Id);
                if (!hasIncoming)
                {
                    result.AddWarning($"Status '{status.Name}' has no incoming transitions and is not initial.");
                }
            }

            // Check for unreachable statuses
            var reachableStatuses = GetReachableStatuses(workflow);
            var unreachable = workflow.Statuses
                .Where(s => !reachableStatuses.Contains(s.Id))
                .ToList();

            foreach (var status in unreachable)
            {
                result.AddWarning($"Status '{status.Name}' is unreachable from the initial status.");
            }

            // Validate transitions
            foreach (var transition in workflow.Transitions)
            {
                // FromStatus must exist
                if (!workflow.Statuses.Any(s => s.Id == transition.FromStatusId))
                {
                    result.IsValid = false;
                    result.Errors.Add($"Transition '{transition.Name}' has invalid from status.");
                }

                // ToStatus must exist
                if (!workflow.Statuses.Any(s => s.Id == transition.ToStatusId))
                {
                    result.IsValid = false;
                    result.Errors.Add($"Transition '{transition.Name}' has invalid to status.");
                }

                // Validate actions
                foreach (var action in transition.Actions)
                {
                    if (!action.IsEnabled)
                    {
                        continue;
                    }

                    // Check for required configuration
                    if (action.ActionType.RequiresConfiguration() && 
                        (action.Configuration == null || !action.Configuration.Any()))
                    {
                        result.AddWarning($"Action '{action.Name}' requires configuration.");
                    }
                }
            }

            return result;
        }

        private HashSet<Guid> GetReachableStatuses(Aggregates.Workflow workflow)
        {
            var reachable = new HashSet<Guid>();
            var queue = new Queue<Guid>();

            queue.Enqueue(workflow.InitialStatusId);
            reachable.Add(workflow.InitialStatusId);

            while (queue.Any())
            {
                var current = queue.Dequeue();
                var outgoing = workflow.Transitions.Where(t => t.FromStatusId == current);

                foreach (var transition in outgoing)
                {
                    if (!reachable.Contains(transition.ToStatusId))
                    {
                        reachable.Add(transition.ToStatusId);
                        queue.Enqueue(transition.ToStatusId);
                    }
                }
            }

            return reachable;
        }

        /// <summary>
        /// Validates a workflow scheme configuration
        /// </summary>
        public SchemeValidationResult ValidateWorkflowScheme(WorkflowScheme scheme, List<Aggregates.Workflow> workflows)
        {
            var result = new SchemeValidationResult { IsValid = true };

            // Must have default workflow or at least one mapping
            if (!scheme.DefaultWorkflowId.HasValue && !scheme.Mappings.Any())
            {
                result.IsValid = false;
                result.Errors.Add("Workflow scheme must have a default workflow or at least one task type mapping.");
            }

            // Validate default workflow exists
            if (scheme.DefaultWorkflowId.HasValue)
            {
                var defaultWorkflow = workflows.FirstOrDefault(w => w.Id == scheme.DefaultWorkflowId.Value);
                if (defaultWorkflow == null)
                {
                    result.IsValid = false;
                    result.Errors.Add("Default workflow does not exist.");
                }
                else if (!defaultWorkflow.IsActive)
                {
                    result.AddWarning("Default workflow is not active.");
                }
            }

            // Validate all mapped workflows exist and are active
            foreach (var mapping in scheme.Mappings)
            {
                var workflow = workflows.FirstOrDefault(w => w.Id == mapping.WorkflowId);
                if (workflow == null)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Mapped workflow {mapping.WorkflowId} does not exist.");
                }
                else if (!workflow.IsActive)
                {
                    result.AddWarning($"Mapped workflow for task type {mapping.TaskTypeId} is not active.");
                }
            }

            return result;
        }
    }

    public class WorkflowValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();

        public void AddWarning(string warning) => Warnings.Add(warning);
    }

    public class SchemeValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();

        public void AddWarning(string warning) => Warnings.Add(warning);
    }
}