using System.Collections.Generic;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects
{
    /// <summary>
    /// Represents a role name in the access control system.
    /// </summary>
    public class RoleName : ValueObject
    {
        public string Value { get; private set; }

        private RoleName() { }

        public RoleName(string value)
        {
            SetValue(value);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: CollaborationConsts.MaxRoleNameLength
            );
        }

        public static implicit operator string(RoleName roleName) => roleName.Value;
        public static explicit operator RoleName(string roleName) => new RoleName(roleName);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
