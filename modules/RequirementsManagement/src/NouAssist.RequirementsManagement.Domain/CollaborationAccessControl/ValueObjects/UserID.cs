using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects
{
    /// <summary>
    /// Represents a user identifier in the system.
    /// </summary>
    public class UserID : ValueObject
    {
        public Guid Value { get; private set; }

        private UserID() { }

        public UserID(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty", nameof(value));
            }

            Value = value;
        }

        public static implicit operator Guid(UserID userId) => userId.Value;
        public static explicit operator UserID(Guid userId) => new UserID(userId);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}
