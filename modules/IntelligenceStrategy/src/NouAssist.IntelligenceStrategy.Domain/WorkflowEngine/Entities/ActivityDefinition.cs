using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Entities
{
    /// <summary>
    /// Represents an activity within a workflow definition
    /// </summary>
    public class ActivityDefinition : Entity<Guid>
    {
        /// <summary>
        /// The ID of the workflow definition this activity belongs to
        /// </summary>
        public Guid WorkflowDefinitionId { get; private set; }
        
        /// <summary>
        /// The name of the activity
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The type of the activity
        /// </summary>
        public ActivityType ActivityType { get; private set; }
        
        /// <summary>
        /// The description of the activity
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Additional properties for the activity
        /// </summary>
        public Dictionary<string, string> Properties { get; private set; }

        protected ActivityDefinition() { }

        internal ActivityDefinition(
            Guid id,
            Guid workflowDefinitionId,
            string name,
            ActivityType activityType,
            Dictionary<string, string> properties = null)
            : base(id)
        {
            WorkflowDefinitionId = workflowDefinitionId;
            SetName(name);
            ActivityType = activityType;
            Properties = properties ?? new Dictionary<string, string>();
        }
        
        /// <summary>
        /// Updates the name of the activity
        /// </summary>
        /// <param name="name">The new name</param>
        public void UpdateName(string name)
        {
            SetName(name);
        }
        
        /// <summary>
        /// Updates the description of the activity
        /// </summary>
        /// <param name="description">The new description</param>
        public void UpdateDescription(string description)
        {
            Description = description;
        }
        
        /// <summary>
        /// Sets a property value
        /// </summary>
        /// <param name="key">The property key</param>
        /// <param name="value">The property value</param>
        public void SetProperty(string key, string value)
        {
            if (Properties == null)
            {
                Properties = new Dictionary<string, string>();
            }
            
            Properties[key] = value;
        }
        
        /// <summary>
        /// Removes a property
        /// </summary>
        /// <param name="key">The property key</param>
        public void RemoveProperty(string key)
        {
            if (Properties != null && Properties.ContainsKey(key))
            {
                Properties.Remove(key);
            }
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: WorkflowEngineConsts.MaxNameLength
            );
        }
    }
}
