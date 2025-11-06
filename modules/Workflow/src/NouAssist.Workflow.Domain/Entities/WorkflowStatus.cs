using System;
using System.Collections.Generic;
using NouAssist.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Workflow.Entities
{
    public class WorkflowStatus : Entity<Guid>
    {
        public Guid WorkflowId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public WorkflowStatusCategory Category { get; private set; }
        public string Color { get; set; }
        public int OrderIndex { get; set; }
        public bool IsInitial { get; private set; }
        public bool IsResolution { get; set; }

        private Dictionary<string, object> _properties;
        public IReadOnlyDictionary<string, object> Properties => 
            _properties?.AsReadOnly();

        protected WorkflowStatus()
        {
            _properties = new Dictionary<string, object>();
        }

        public WorkflowStatus(
            Guid id,
            Guid workflowId,
            string name,
            WorkflowStatusCategory category,
            string color = null,
            int orderIndex = 0,
            bool isInitial = false) : base(id)
        {
            WorkflowId = workflowId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Category = category;
            Color = color ?? GetDefaultColorForCategory(category);
            OrderIndex = orderIndex;
            IsInitial = isInitial;
            _properties = new Dictionary<string, object>();
        }

        
        public void SetProperty(string key, object value)
        {
            if (_properties == null)
            {
                _properties = new Dictionary<string, object>();
            }

            _properties[key] = value;
        }

        public bool IsTodo => Category == WorkflowStatusCategory.Todo;
        public bool IsInProgress => Category == WorkflowStatusCategory.InProgress;
        public bool IsDone => Category == WorkflowStatusCategory.Done;
        public bool IsCancelled => Category == WorkflowStatusCategory.Cancelled;
        public bool IsBlocked => Category == WorkflowStatusCategory.Blocked;
        public bool IsTerminal => IsDone || IsCancelled;
        
        private static string GetDefaultColorForCategory(WorkflowStatusCategory category)
        {
            return category switch
            {
                WorkflowStatusCategory.Todo => "#94a3b8",
                WorkflowStatusCategory.InProgress => "#3b82f6",
                WorkflowStatusCategory.Done => "#22c55e",
                WorkflowStatusCategory.Cancelled => "#ef4444",
                WorkflowStatusCategory.Blocked => "#FFBF00",
                _ => "#64748b"
            };
        }
    }
}
