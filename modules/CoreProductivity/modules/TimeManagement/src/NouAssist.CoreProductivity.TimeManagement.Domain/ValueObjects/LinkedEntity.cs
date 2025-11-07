using System;
using System.Collections.Generic;
using NouAssist.CoreProductivity.TimeManagement.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.CoreProductivity.TimeManagement.ValueObjects
{
    public class LinkedEntity : ValueObject
    {
        public Guid EntityId { get; private set; }
        public LinkedEntityType EntityType { get; private set; }

        protected LinkedEntity() { }

        public LinkedEntity(Guid entityId, LinkedEntityType entityType)
        {
            EntityId = entityId;
            EntityType = entityType;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EntityId;
            yield return EntityType;
        }
    }
}
