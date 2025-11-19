using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IdentityAccess.Users.Entities
{
    public class UserPreference : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public string Category { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected UserPreference() { }

        public UserPreference(
            Guid id,
            Guid userId,
            string key,
            string value,
            string category = "general"
        ) : base(id)
        {
            UserId = userId;
            Key = key;
            Value = value;
            Category = category;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateValue(string newValue)
        {
            Value = newValue;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
