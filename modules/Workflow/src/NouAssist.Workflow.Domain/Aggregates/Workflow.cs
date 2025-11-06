using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Workflow.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.Workflow.Aggregates
{
    public class Workflow : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        public Guid InitialStatusId { get; private set; }
        public bool IsDefault { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<WorkflowStatus> _statuses = new();
        public IReadOnlyCollection<WorkflowStatus> Statuses => _statuses.AsReadOnly();

        private readonly List<WorkflowTransition> _transitions = new();
        public IReadOnlyCollection<WorkflowTransition> Transitions => _transitions.AsReadOnly();

        private Dictionary<string, object> _properties;
        public IReadOnlyDictionary<string, object> Properties => 
            _properties?.AsReadOnly();

        protected Workflow()
        {
            _properties = new Dictionary<string, object>();
        }

        public Workflow(
            Guid id,
            string name,
            Guid initialStatusId,
            string description = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            InitialStatusId = initialStatusId;
            Description = description;
            IsActive = true;
        }

        public void AddStatus(WorkflowStatus status)
        {
            Check.NotNull(status, nameof(status));

            if (_statuses.Any(s => s.Name == status.Name))
            {
                throw new BusinessException("Workflow:DuplicateStatus", 
                    $"Status '{status.Name}' already exists.");
            }

            _statuses.Add(status);
        }

        public void AddTransition(WorkflowTransition transition)
        {
            Check.NotNull(transition, nameof(transition));

            // Validate status exists
            if (!_statuses.Any(s => s.Id == transition.FromStatusId))
            {
                throw new BusinessException("Workflow:InvalidFromStatus");
            }

            if (!_statuses.Any(s => s.Id == transition.ToStatusId))
            {
                throw new BusinessException("Workflow:InvalidToStatus");
            }

            _transitions.Add(transition);
        }

        public WorkflowStatus GetStatus(Guid statusId)
        {
            var status = _statuses.FirstOrDefault(s => s.Id == statusId);
            if (status == null)
            {
                throw new BusinessException("Workflow:StatusNotFound");
            }
            return status;
        }

        public List<WorkflowTransition> GetAvailableTransitions(Guid currentStatusId)
        {
            return _transitions.Where(t => t.FromStatusId == currentStatusId).ToList();
        }

        public void SetAsDefault()
        {
            IsDefault = true;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            if (IsDefault)
            {
                throw new BusinessException("Workflow:CannotDeactivateDefault");
            }
            IsActive = false;
        }
        
        public void SetProperty(string key, object value)
        {
            if (_properties == null)
            {
                _properties = new Dictionary<string, object>();
            }

            _properties[key] = value;
        }

        public object GetProperty(string key)
        {
            return _properties?.GetValueOrDefault(key);
        }
    }
}
