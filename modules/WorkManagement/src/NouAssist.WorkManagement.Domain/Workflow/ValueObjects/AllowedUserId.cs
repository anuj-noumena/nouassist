using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.Workflow.ValueObjects // Adjust namespace as needed
{
    public class AllowedUserId: ValueObject
    {
        public Guid Value { get; private set; }

        private AllowedUserId() { } // For EF

        public AllowedUserId(Guid value)
        {
            Value = value != default ? value : throw new ArgumentException("Invalid GUID", nameof(value));
        }

        public static implicit operator Guid(AllowedUserId id) => id.Value;
        public static implicit operator AllowedUserId(Guid value) => new(value);
        public override string ToString() => Value.ToString();
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}