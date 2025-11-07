using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkflowOrchestration.GTDSystem.Entities
{
    public class SomedayMaybeReview : FullAuditedEntity<Guid>
    {
        public Guid SomedayMaybeId { get; private set; }
        public Guid OwnerId { get; private set; }
        public DateTime ReviewedAt { get; private set; }
        public string? Decision { get; set; }
        public string? Notes { get; set; }
        public int? InterestLevel { get; private set; }
        public bool? ShouldPromote { get; set; }
        public string? NextSteps { get; set; }

        protected SomedayMaybeReview() { }

        public SomedayMaybeReview(
            Guid id,
            Guid somedayMaybeId,
            Guid ownerId) : base(id)
        {
            SomedayMaybeId = somedayMaybeId;
            OwnerId = ownerId;
            ReviewedAt = DateTime.UtcNow;
        }

        public void SetInterestLevel(int level)
        {
            if (level < 1 || level > 5)
                throw new BusinessException("GTD:InvalidInterestLevel", 
                    "Interest level must be between 1 and 5.");
            InterestLevel = level;
        }

        public void RecommendPromotion()
        {
            ShouldPromote = true;
        }

        public bool HasDecision => !string.IsNullOrWhiteSpace(Decision);
        public bool IsHighInterest => InterestLevel.HasValue && InterestLevel.Value >= 4;
        public bool IsLowInterest => InterestLevel.HasValue && InterestLevel.Value <= 2;
    }
}
