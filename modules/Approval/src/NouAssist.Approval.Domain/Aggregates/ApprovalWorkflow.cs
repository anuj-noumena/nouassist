using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Approval.Entities;
using NouAssist.Approval.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Approval.Aggregates
{
    public class ApprovalWorkflow : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ApprovalWorkflowType WorkflowType { get; private set; }
        public ApprovalStrategy Strategy { get; private set; }
        public bool IsActive { get; private set; }
        public bool AllowReassign { get; private set; }
        public bool AllowDelegate { get; private set; }
        public int? TimeoutDays { get; private set; }
        public string TimeoutAction { get; private set; }
        public Dictionary<string, object> CustomFieldValues { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<ApprovalStep> _steps = new();
        public IReadOnlyCollection<ApprovalStep> Steps => _steps.AsReadOnly();

        protected ApprovalWorkflow() { /* ORM */ }

        public ApprovalWorkflow(
            Guid id,
            string name,
            ApprovalWorkflowType workflowType,
            ApprovalStrategy strategy,
            Guid? tenantId,
            string description = null
        ) : base(id)
        {
            SetName(name);
            WorkflowType = workflowType;
            Strategy = strategy;
            Description = description;
            TenantId = tenantId;
            IsActive = true;
            AllowReassign = false;
            AllowDelegate = false;
            CustomFieldValues = new Dictionary<string, object>();
            Metadata = new Dictionary<string, object>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
        }

        public void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 2000)
            {
                throw new BusinessException("Approval:DescriptionTooLong")
                    .WithData("MaxLength", 2000);
            }
            Description = description;
        }

        public void Activate()
        {
            if (!_steps.Any())
            {
                throw new BusinessException("Approval:CannotActivateWithoutSteps")
                    .WithData("WorkflowId", Id);
            }
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void EnableReassignment()
        {
            AllowReassign = true;
        }

        public void EnableDelegation()
        {
            AllowDelegate = true;
        }

        public void SetTimeout(int days, string action)
        {
            if (days <= 0)
            {
                throw new BusinessException("Approval:InvalidTimeoutDays");
            }
            TimeoutDays = days;
            TimeoutAction = action;
        }

        public ApprovalStep AddStep(
            string name,
            int orderIndex,
            ApprovalRequirement requirement,
            string description = null
        )
        {
            if (!IsActive)
            {
                throw new BusinessException("Approval:CannotModifyActiveWorkflow");
            }

            if (_steps.Any(s => s.OrderIndex == orderIndex))
            {
                throw new BusinessException("Approval:DuplicateStepOrder")
                    .WithData("OrderIndex", orderIndex);
            }

            var step = new ApprovalStep(
                Guid.NewGuid(),
                Id,
                name,
                orderIndex,
                requirement,
                description
            );

            _steps.Add(step);
            return step;
        }

        public void RemoveStep(Guid stepId)
        {
            if (!IsActive)
            {
                throw new BusinessException("Approval:CannotModifyActiveWorkflow");
            }

            var step = _steps.FirstOrDefault(s => s.Id == stepId);
            if (step != null)
            {
                _steps.Remove(step);
                ReorderSteps();
            }
        }

        public void ReorderSteps()
        {
            var orderedSteps = _steps.OrderBy(s => s.OrderIndex).ToList();
            for (int i = 0; i < orderedSteps.Count; i++)
            {
                orderedSteps[i].SetOrderIndex(i + 1);
            }
        }

        public ApprovalStep GetStep(Guid stepId)
        {
            return _steps.FirstOrDefault(s => s.Id == stepId)
                ?? throw new BusinessException("Approval:StepNotFound")
                    .WithData("StepId", stepId);
        }

        public ApprovalStep GetNextStep(Guid currentStepId)
        {
            var currentStep = GetStep(currentStepId);
            return _steps
                .Where(s => s.OrderIndex > currentStep.OrderIndex)
                .OrderBy(s => s.OrderIndex)
                .FirstOrDefault();
        }

        public bool IsLastStep(Guid stepId)
        {
            var step = GetStep(stepId);
            return step.OrderIndex == _steps.Max(s => s.OrderIndex);
        }

        public int GetTotalSteps() => _steps.Count;

        public void SetCustomFieldValue(string key, object value)
        {
            CustomFieldValues[key] = value;
        }

        public object GetCustomFieldValue(string key)
        {
            return CustomFieldValues.TryGetValue(key, out var value) ? value : null;
        }
    }
}
