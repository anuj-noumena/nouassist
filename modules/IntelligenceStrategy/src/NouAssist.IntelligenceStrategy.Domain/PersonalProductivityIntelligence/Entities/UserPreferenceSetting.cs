using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents a user preference setting
    /// </summary>
    public class UserPreferenceSetting : Entity
    {
        /// <summary>
        /// Setting key
        /// </summary>
        public string Key { get; protected set; }
        
        /// <summary>
        /// Setting value
        /// </summary>
        public string Value { get; protected set; }
        
        /// <summary>
        /// When the setting was last updated
        /// </summary>
        public DateTime LastUpdated { get; protected set; }

        protected UserPreferenceSetting()
        {
        }

        /// <summary>
        /// Creates a new user preference setting
        /// </summary>
        public UserPreferenceSetting(string key, string value)
        {
            Key = key;
            Value = value;
            LastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the setting value
        /// </summary>
        public void UpdateValue(string value)
        {
            Value = value;
            LastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the entity keys
        /// </summary>
        public override object[] GetKeys()
        {
            return new object[] { Key };
        }
    }
}
