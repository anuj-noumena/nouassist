using System;
using NouAssist.Core.Enums;
using NouAssist.GTD.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.GTD.Aggregates
{
    public class GtdHorizonAlignment : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public Guid HigherHorizonId { get; private set; }
        public GtdHorizonLevel HigherLevel { get; private set; }
        public string LowerEntityType { get; private set; }
        public Guid LowerEntityId { get; private set; }
        public int? AlignmentStrength { get; private set; }
        public string? AlignmentNotes { get; set; }
        public DateTime? LastValidatedAt { get; private set; }
        public string? ValidationNotes { get; set; }
        public bool IsValidated { get; private set; }

        protected GtdHorizonAlignment() { }

        public GtdHorizonAlignment(
            Guid id,
            Guid ownerId,
            Guid higherHorizonId,
            GtdHorizonLevel higherLevel,
            string lowerEntityType,
            Guid lowerEntityId,
            Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            HigherHorizonId = higherHorizonId;
            HigherLevel = higherLevel;
            LowerEntityType = Check.NotNullOrWhiteSpace(lowerEntityType, nameof(lowerEntityType));
            LowerEntityId = lowerEntityId;
            TenantId = tenantId;
            IsValidated = false;
        }

        public void UpdateStrength(int strength)
        {
            if (strength < 1 || strength > 5)
                throw new BusinessException("GTD:InvalidAlignmentStrength", 
                    "Alignment strength must be between 1 and 5.");
            AlignmentStrength = strength;
        }

        public void Validate(bool isAligned, string? notes = null)
        {
            IsValidated = isAligned;
            LastValidatedAt = DateTime.UtcNow;
            ValidationNotes = notes;
        }

        public bool HasStrongAlignment => AlignmentStrength.HasValue && AlignmentStrength.Value >= 4;
        public bool HasWeakAlignment => AlignmentStrength.HasValue && AlignmentStrength.Value <= 2;
        public bool HasBeenValidated => LastValidatedAt.HasValue;
    }
}
