using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.ValueObjects
{
    public class WorkflowCondition : ValueObject  // Inherit from your base if exists, or simple class
    {
        public string Key { get; private set; }
        public object Value { get; private set; }
        public string Operator { get; private set; } = "equals";  // e.g., "equals", "greaterThan"

        protected WorkflowCondition() { }

        public WorkflowCondition(string key, object value, string op = "equals")
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value;
            Operator = op;
        }

        // Equality override
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Value;
            yield return Operator;
        }
    }
}