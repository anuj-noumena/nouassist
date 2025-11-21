using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Specification.ValueObjects
{
    /// <summary>
    /// Value object representing a business objective identifier
    /// </summary>
    public class ObjectiveId : ValueObject
    {
        public Guid Value { get; private set; }

        protected ObjectiveId() { }

        public ObjectiveId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:InvalidObjectiveId");
            }

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator Guid(ObjectiveId objectiveId)
        {
            return objectiveId?.Value ?? Guid.Empty;
        }

        public static implicit operator ObjectiveId(Guid value)
        {
            return value == Guid.Empty ? null : new ObjectiveId(value);
        }
    }
}
