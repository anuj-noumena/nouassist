using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.Workflow.Enums;
using NouAssist.WorkManagement.Workflow.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.WorkManagement.Workflow.Entities
{
    public class WorkflowTransition : Entity<Guid>
    {
        public Guid WorkflowId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public Guid FromStatusId { get; private set; }
        public Guid ToStatusId { get; private set; }
        public TransitionPurpose Purpose { get; private set; } = TransitionPurpose.Standard;  // New: Default to Standard
        
        public bool RequiresComment { get; set; }
        public bool RequiresResolution { get; set; }
        public bool IsGlobal { get; set; }
        public int OrderIndex { get; set; }
        public string ButtonLabel { get; set; }
        public string ButtonColor { get; set; }

        private readonly List<WorkflowAction> _actions = new();
        public IReadOnlyCollection<WorkflowAction> Actions => _actions.AsReadOnly();

        private List<WorkflowCondition> _conditions = new();
        public IReadOnlyCollection<WorkflowCondition> Conditions => 
            _conditions.AsReadOnly();

        private Dictionary<string, object> _validators = new();
        public IReadOnlyDictionary<string, object> Validators => _validators.AsReadOnly();

        protected WorkflowTransition(){}

        public WorkflowTransition(
            Guid id,
            Guid workflowId,
            string name,
            Guid fromStatusId,
            Guid toStatusId,
            TransitionPurpose purpose = TransitionPurpose.Standard,
            string description = null) : base(id)
        {
            if (fromStatusId == toStatusId)
            {
                throw new BusinessException("Workflow:SameFromToStatus");
            }

            WorkflowId = workflowId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            FromStatusId = fromStatusId;
            ToStatusId = toStatusId;
            Purpose = purpose;
            Description = description;
        }

        public void AddAction(WorkflowAction action)
        {
            Check.NotNull(action, nameof(action));
            _actions.Add(action);
        }
        public void SetCondition(WorkflowCondition condition)
        {
            _conditions ??= new List<WorkflowCondition>();

            if (!_conditions.Contains(condition))
            {
                _conditions.Add(condition);
            }
        }

        public void SetValidator(string key, object value)
        {
            _validators ??= new Dictionary<string, object>();

            _validators[key] = value;
        }
        
        public void SetPurpose(TransitionPurpose purpose)
        {
            Purpose = purpose;
        }

        public bool HasConditions => _conditions?.Any() ?? false;
        public bool HasValidators => _validators?.Any() ?? false;
    }
}
