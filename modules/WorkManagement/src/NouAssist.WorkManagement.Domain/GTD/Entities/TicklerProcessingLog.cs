using System;
using NouAssist.WorkManagement.GTD.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.WorkManagement.GTD.Entities
{
    public class TicklerProcessingLog : Entity<Guid>
    {
        public Guid TicklerId { get; private set; }
        public Guid OwnerId { get; private set; }
        public DateTime ProcessedAt { get; private set; }
        public TicklerActionType ActionTaken { get; private set; }
        public string? ResultingEntityType { get; private set; }
        public Guid? ResultingEntityId { get; private set; }
        public string? Notes { get; set; }
        public int? ProcessingTimeMinutes { get; set; }

        protected TicklerProcessingLog() { }

        public TicklerProcessingLog(
            Guid id,
            Guid ticklerId,
            Guid ownerId,
            TicklerActionType actionTaken) : base(id)
        {
            TicklerId = ticklerId;
            OwnerId = ownerId;
            ActionTaken = actionTaken;
            ProcessedAt = DateTime.UtcNow;
        }

        public void SetResultingEntity(string entityType, Guid entityId)
        {
            ResultingEntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            ResultingEntityId = entityId;
        }

        public void AddProcessingTime(int minutes)
        {
            if (minutes < 0)
                throw new ArgumentException("Processing time cannot be negative.");
            ProcessingTimeMinutes = minutes;
        }

        public bool CreatedEntity => !string.IsNullOrWhiteSpace(ResultingEntityType) && ResultingEntityId.HasValue;
    }
}
