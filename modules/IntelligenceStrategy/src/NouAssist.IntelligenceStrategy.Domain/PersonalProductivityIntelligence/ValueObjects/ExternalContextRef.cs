using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing a reference to an external context
    /// </summary>
    public class ExternalContextRef : ValueObject
    {
        /// <summary>
        /// Type of the external entity (e.g., "Project", "Team", "Task")
        /// </summary>
        public string EntityType { get; private set; }
        
        /// <summary>
        /// ID of the external entity
        /// </summary>
        public Guid EntityId { get; private set; }
        
        /// <summary>
        /// Additional properties for the reference
        /// </summary>
        public Dictionary<string, string> Properties { get; private set; }

        protected ExternalContextRef()
        {
            Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Creates a new external context reference
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="entityId">ID of the entity</param>
        public ExternalContextRef(string entityType, Guid entityId)
            : this(entityType, entityId, null)
        {
        }

        /// <summary>
        /// Creates a new external context reference with properties
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="entityId">ID of the entity</param>
        /// <param name="properties">Additional properties</param>
        public ExternalContextRef(string entityType, Guid entityId, Dictionary<string, string> properties)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            EntityId = entityId;
            Properties = properties ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds a property to the context reference
        /// </summary>
        /// <param name="key">Property key</param>
        /// <param name="value">Property value</param>
        public void AddProperty(string key, string value)
        {
            Properties[key] = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EntityType;
            yield return EntityId;
            
            foreach (var property in Properties.OrderBy(p => p.Key))
            {
                yield return $"{property.Key}={property.Value}";
            }
        }
    }
}
