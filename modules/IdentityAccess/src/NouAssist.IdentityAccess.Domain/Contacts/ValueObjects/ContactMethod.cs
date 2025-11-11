using System.Collections.Generic;
using NouAssist.Contacts.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Contacts.ValueObjects
{
    public class ContactMethod : ValueObject
    {
        public string Type { get; private set; } // Email, Phone, Mobile, etc.
        public string Value { get; private set; }
        public string Label { get; private set; }
        public bool IsPrimary { get; private set; }

        protected ContactMethod() { }

        public ContactMethod(string type, string value, string label = null, bool isPrimary = false)
        {
            Type = Check.NotNullOrWhiteSpace(type, nameof(type));
            Value = Check.NotNullOrWhiteSpace(value, nameof(value));
            Label = label;
            IsPrimary = isPrimary;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Value;
            yield return Label;
            yield return IsPrimary;
        }
    }
}
