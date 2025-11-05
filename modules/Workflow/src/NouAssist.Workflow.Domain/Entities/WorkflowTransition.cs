using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Workflow.Entities
{
    public class WorkflowTransition : Entity<Guid>
    {
        public Guid WorkflowId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public Guid FromStatusId { get; private set; }
        public Guid ToStatusId { get; private set; }

        private readonly List<string> _allowedRoles = new();
        public IReadOnlyCollection<string> AllowedRoles => _allowedRoles.AsReadOnly();

        private readonly List<Guid> _allowedUserIds = new();
        public IReadOnlyCollection<Guid> AllowedUserIds => _allowedUserIds.AsReadOnly();

        public bool RequiresComment { get; set; }
        public bool RequiresResolution { get; set; }
        public bool IsGlobal { get; set; }
        public int OrderIndex { get; set; }
        public string ButtonLabel { get; set; }
        public string ButtonColor { get; set; }

        private readonly List<WorkflowAction> _actions = new();
        public IReadOnlyCollection<WorkflowAction> Actions => _actions.AsReadOnly();

        private Dictionary<string, object> _conditions;
        public IReadOnlyDictionary<string, object> Conditions => 
            _conditions?.AsReadOnly();

        private Dictionary<string, object> _validators;
        public IReadOnlyDictionary<string, object> Validators => 
            _validators?.AsReadOnly();

        protected WorkflowTransition()
        {
            _conditions = new Dictionary<string, object>();
            _validators = new Dictionary<string, object>();
        }

        public WorkflowTransition(
            Guid id,
            Guid workflowId,
            string name,
            Guid fromStatusId,
            Guid toStatusId,
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
            Description = description;
        }

        public void AddAction(WorkflowAction action)
        {
            Check.NotNull(action, nameof(action));
            _actions.Add(action);
        }

        public void AddAllowedRole(string role)
        {
            if (!_allowedRoles.Contains(role))
            {
                _allowedRoles.Add(role);
            }
        }

        public void AddAllowedUser(Guid userId)
        {
            if (!_allowedUserIds.Contains(userId))
            {
                _allowedUserIds.Add(userId);
            }
        }

        public bool CanUserPerform(Guid userId, List<string> userRoles)
        {
            if (!_allowedRoles.Any() && !_allowedUserIds.Any())
                return true;

            if (_allowedUserIds.Contains(userId))
                return true;

            return _allowedRoles.Any(role => userRoles.Contains(role));
        }

        public void SetCondition(string key, object value)
        {
            if (_conditions == null)
                _conditions = new Dictionary<string, object>();
            
            _conditions[key] = value;
        }

        public void SetValidator(string key, object value)
        {
            if (_validators == null)
                _validators = new Dictionary<string, object>();
            
            _validators[key] = value;
        }

        public bool HasConditions => _conditions?.Any() ?? false;
        public bool HasValidators => _validators?.Any() ?? false;

        public bool HasRestrictions => _allowedRoles.Any() || _allowedUserIds.Any();
    }
}
