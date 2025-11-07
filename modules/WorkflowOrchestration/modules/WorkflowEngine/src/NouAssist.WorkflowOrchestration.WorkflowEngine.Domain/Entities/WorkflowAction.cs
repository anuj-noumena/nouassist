using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Entities
{
    public class WorkflowAction : Entity<Guid>
    {
        public Guid TransitionId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public WorkflowActionType ActionType { get; private set; }
        public Dictionary<string, object> Configuration { get; private set; }
        public int OrderIndex { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsAsync { get; set; }
        public bool ContinueOnError { get; set; }
        
        private Dictionary<string, object> _conditions;
        public IReadOnlyDictionary<string, object> Conditions => 
            _conditions?.AsReadOnly();

        protected WorkflowAction()
        {
            _conditions = new Dictionary<string, object>();
        }

        public WorkflowAction(
            Guid id,
            Guid transitionId,
            string name,
            WorkflowActionType actionType,
            Dictionary<string, object> configuration,
            string description = null) : base(id)
        {
            TransitionId = transitionId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ActionType = actionType;
            Configuration = configuration ?? new Dictionary<string, object>();
            Description = description;
            IsEnabled = true;
        }

        public void UpdateConfiguration(Dictionary<string, object> newConfiguration)
        {
            Configuration = newConfiguration ?? throw new ArgumentNullException(nameof(newConfiguration));
        }
        
        public void SetCondition(string key, object value)
        {
            if (_conditions == null)
            {
                _conditions = new Dictionary<string, object>();
            }

            _conditions[key] = value;
        }

        public bool HasConditions => _conditions?.Any() ?? false;

    }
}
