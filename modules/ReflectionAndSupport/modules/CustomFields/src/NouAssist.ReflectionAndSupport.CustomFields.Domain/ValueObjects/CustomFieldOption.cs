using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.ReflectionAndSupport.CustomFields.ValueObjects
{
    public class CustomFieldOption : ValueObject
    {
        public Guid Id { get; private set; }
        public string Label { get; private set; }
        public string Value { get; private set; }
        public string Color { get; private set; }
        public string Icon { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; }

        private CustomFieldOption() { /* ORM */ }

        public CustomFieldOption(string label, string value = null, string color = null)
        {
            Id = Guid.NewGuid();
            Label = Check.NotNullOrWhiteSpace(label, nameof(label));
            Value = value ?? label.ToLowerInvariant().Replace(" ", "_");
            Color = color;
            IsActive = true;
            DisplayOrder = 0;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Label;
            yield return Value;
            yield return Color;
            yield return Icon;
            yield return DisplayOrder;
        }
    }
}
