using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.Workflow.Entities
{
    /// <summary>
    /// Links a custom field definition to a workflow scheme with context-specific configuration
    /// </summary>
    public class WorkflowCustomField : FullAuditedEntity<Guid>
    {
        public Guid WorkflowSchemeId { get; private set; }
        public Guid CustomFieldDefinitionId { get; private set; }

        // Context-specific configuration
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
        public int DisplayOrder { get; set; }

        // Status-specific visibility
        private readonly List<Guid> _visibleInStatusIds = new();
        public IReadOnlyCollection<Guid> VisibleInStatusIds => _visibleInStatusIds.AsReadOnly();

        private readonly List<Guid> _requiredInStatusIds = new();
        public IReadOnlyCollection<Guid> RequiredInStatusIds => _requiredInStatusIds.AsReadOnly();

        private readonly List<Guid> _readOnlyInStatusIds = new();
        public IReadOnlyCollection<Guid> ReadOnlyInStatusIds => _readOnlyInStatusIds.AsReadOnly();

        // Transition rules
        private readonly List<Guid> _requiredForTransitionIds = new();
        public IReadOnlyCollection<Guid> RequiredForTransitionIds => 
            _requiredForTransitionIds.AsReadOnly();

        public bool IsActive { get; set; }

        protected WorkflowCustomField() { }

        public WorkflowCustomField(
            Guid id,
            Guid workflowSchemeId,
            Guid customFieldDefinitionId,
            bool isRequired = false,
            string defaultValue = null,
            int displayOrder = 0) : base(id)
        {
            WorkflowSchemeId = workflowSchemeId;
            CustomFieldDefinitionId = customFieldDefinitionId;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
            DisplayOrder = displayOrder;
            IsActive = true;
        }

        // Visibility Management
        public bool IsVisibleInStatus(Guid statusId)
        {
            return !_visibleInStatusIds.Any() || _visibleInStatusIds.Contains(statusId);
        }

        public void AddVisibleInStatus(Guid statusId)
        {
            if (!_visibleInStatusIds.Contains(statusId))
            {
                _visibleInStatusIds.Add(statusId);
            }
        }

        public void RemoveVisibleInStatus(Guid statusId)
        {
            _visibleInStatusIds.Remove(statusId);
        }

        // Required Management
        public bool IsRequiredInStatus(Guid statusId)
        {
            return IsRequired || _requiredInStatusIds.Contains(statusId);
        }

        public void MakeRequiredInStatus(Guid statusId)
        {
            if (!_requiredInStatusIds.Contains(statusId))
            {
                _requiredInStatusIds.Add(statusId);
            }
        }

        public void RemoveRequiredInStatus(Guid statusId)
        {
            _requiredInStatusIds.Remove(statusId);
        }

        // Read-Only Management
        public bool IsReadOnlyInStatus(Guid statusId)
        {
            return _readOnlyInStatusIds.Contains(statusId);
        }

        public void MakeReadOnlyInStatus(Guid statusId)
        {
            if (!_readOnlyInStatusIds.Contains(statusId))
            {
                _readOnlyInStatusIds.Add(statusId);
            }
        }

        public void RemoveReadOnlyInStatus(Guid statusId)
        {
            _readOnlyInStatusIds.Remove(statusId);
        }

        // Transition Requirements
        public bool IsRequiredForTransition(Guid transitionId)
        {
            return _requiredForTransitionIds.Contains(transitionId);
        }

        public void RequireForTransition(Guid transitionId)
        {
            if (!_requiredForTransitionIds.Contains(transitionId))
            {
                _requiredForTransitionIds.Add(transitionId);
            }
        }

        public void RemoveRequireForTransition(Guid transitionId)
        {
            _requiredForTransitionIds.Remove(transitionId);
        }
    }
}
