using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkflowOrchestration.GTDSystem.ValueObjects
{
    public class CoreValue : ValueObject
    {
        public string Value { get; private set; }
        public string? Description { get; private set; }
        public string? Example { get; private set; }
        public int Priority { get; private set; }

        private CoreValue() { }

        public CoreValue(string value, string? description = null, string? example = null, int priority = 0)
        {
            Value = Check.NotNullOrWhiteSpace(value, nameof(value), maxLength: 100);
            Description = Check.Length(description, nameof(description), 500);
            Example = example;
            Priority = priority;
        }

        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
        public bool HasExample => !string.IsNullOrWhiteSpace(Example);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Description ?? string.Empty;
            yield return Priority;
        }
    }
}
