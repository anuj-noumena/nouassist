using System;
using System.Collections.Generic;
using NouAssist.DecisionSupport.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.DecisionSupport.Entities
{
    /// <summary>
    /// Represents the usage of a prioritization framework in a decision model
    /// </summary>
    public class PrioritizationFrameworkUsage : Entity<Guid>
    {
        /// <summary>
        /// The ID of the decision model this framework usage belongs to
        /// </summary>
        public Guid DecisionModelId { get; private set; }
        
        /// <summary>
        /// The type of prioritization framework
        /// </summary>
        public PrioritizationFramework FrameworkType { get; private set; }
        
        /// <summary>
        /// The name of this framework usage
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The description of this framework usage
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Priority settings for alternatives
        /// </summary>
        private readonly List<PriorityItem> _priorityItems;
        public IReadOnlyCollection<PriorityItem> PriorityItems => _priorityItems.AsReadOnly();

        protected PrioritizationFrameworkUsage()
        {
            _priorityItems = new List<PriorityItem>();
        }

        /// <summary>
        /// Creates a new prioritization framework usage
        /// </summary>
        public PrioritizationFrameworkUsage(
            Guid id,
            Guid decisionModelId,
            PrioritizationFramework frameworkType,
            string name,
            string description)
            : base(id)
        {
            DecisionModelId = decisionModelId;
            FrameworkType = frameworkType;
            Name = name;
            Description = description;
            _priorityItems = new List<PriorityItem>();
        }
        
        /// <summary>
        /// Adds a priority item for an alternative
        /// </summary>
        public void AddPriorityItem(Guid alternativeId, string priorityValue, string notes = null)
        {
            _priorityItems.Add(new PriorityItem(alternativeId, priorityValue, notes));
        }
        
        /// <summary>
        /// Updates the framework usage properties
        /// </summary>
        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
    
    /// <summary>
    /// Represents a priority assignment for an alternative
    /// </summary>
    public class PriorityItem : Entity
    {
        /// <summary>
        /// The ID of the alternative this priority is for
        /// </summary>
        public Guid AlternativeId { get; private set; }
        
        /// <summary>
        /// The priority value (e.g., "Must Have", "Important/Urgent")
        /// </summary>
        public string PriorityValue { get; private set; }
        
        /// <summary>
        /// Additional notes about the priority assignment
        /// </summary>
        public string Notes { get; private set; }

        protected PriorityItem()
        {
        }

        /// <summary>
        /// Creates a new priority item
        /// </summary>
        public PriorityItem(
            Guid alternativeId,
            string priorityValue,
            string notes = null)
        {
            AlternativeId = alternativeId;
            PriorityValue = priorityValue;
            Notes = notes;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { AlternativeId };
        }
    }
}
