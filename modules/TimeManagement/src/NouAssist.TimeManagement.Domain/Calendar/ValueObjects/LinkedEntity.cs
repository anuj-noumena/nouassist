using System;
using System.Collections.Generic;
using NouAssist.TimeManagement.Calendar.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.TimeManagement.Calendar.ValueObjects
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
