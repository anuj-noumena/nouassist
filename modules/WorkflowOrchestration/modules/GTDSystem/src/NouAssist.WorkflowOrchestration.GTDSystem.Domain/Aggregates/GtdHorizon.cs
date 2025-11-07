using System;
using System.Collections.Generic;
using NouAssist.Core.Enums;
using NouAssist.WorkflowOrchestration.GTDSystem.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkflowOrchestration.GTDSystem.Aggregates
{
    public class GtdHorizon : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public GtdHorizonLevel HorizonLevel { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? LifePurpose { get; set; }
        public List<string> CoreValues { get; private set; }
        public string? VisionStatement { get; set; }
        public List<string> KeyThemes { get; private set; }
        public List<Guid> RelatedGoalIds { get; private set; }
        public List<Guid> RelatedAreaIds { get; private set; }
        public List<Guid> RelatedProjectIds { get; private set; }
        public HorizonReviewFrequency ReviewFrequency { get; private set; }
        public DateTime? LastReviewedAt { get; private set; }
        public DateTime? NextReviewAt { get; private set; }
        public int ReviewCount { get; private set; }
        public bool IsActive { get; set; }
        public bool IsDraft { get; private set; }
        public int Version { get; private set; }

        protected GtdHorizon() 
        {
            CoreValues = new List<string>();
            KeyThemes = new List<string>();
            RelatedGoalIds = new List<Guid>();
            RelatedAreaIds = new List<Guid>();
            RelatedProjectIds = new List<Guid>();
        }

        public GtdHorizon(
            Guid id, 
            Guid ownerId, 
            GtdHorizonLevel horizonLevel, 
            string title,
            Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            TenantId = tenantId;
            HorizonLevel = horizonLevel;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
            
            CoreValues = new List<string>();
            KeyThemes = new List<string>();
            RelatedGoalIds = new List<Guid>();
            RelatedAreaIds = new List<Guid>();
            RelatedProjectIds = new List<Guid>();
            
            ReviewFrequency = GetDefaultReviewFrequency(horizonLevel);
            NextReviewAt = CalculateNextReview(DateTime.UtcNow);
            IsActive = true;
            IsDraft = true;
            Version = 1;
            ReviewCount = 0;
        }

        public void MarkAsReviewed()
        {
            LastReviewedAt = DateTime.UtcNow;
            NextReviewAt = CalculateNextReview(DateTime.UtcNow);
            ReviewCount++;
            Version++;
        }

        public void Publish()
        {
            if (!IsDraft)
            {
                throw new BusinessException("GTD:HorizonAlreadyPublished", 
                    "Horizon is already published.");
            }
            IsDraft = false;
            Version++;
        }

        public void LinkGoal(Guid goalId)
        {
            if (!RelatedGoalIds.Contains(goalId))
            {
                RelatedGoalIds.Add(goalId);
                Version++;
            }
        }

        public void UnlinkGoal(Guid goalId)
        {
            if (RelatedGoalIds.Remove(goalId))
            {
                Version++;
            }
        }

        public void LinkArea(Guid areaId)
        {
            if (!RelatedAreaIds.Contains(areaId))
            {
                RelatedAreaIds.Add(areaId);
                Version++;
            }
        }

        public void LinkProject(Guid projectId)
        {
            if (!RelatedProjectIds.Contains(projectId))
            {
                RelatedProjectIds.Add(projectId);
                Version++;
            }
        }

        public void AddCoreValue(string value)
        {
            if (!CoreValues.Contains(value))
            {
                CoreValues.Add(value);
            }
        }

        public void AddKeyTheme(string theme)
        {
            if (!KeyThemes.Contains(theme))
            {
                KeyThemes.Add(theme);
            }
        }

        public void UpdateReviewFrequency(HorizonReviewFrequency frequency)
        {
            ReviewFrequency = frequency;
            NextReviewAt = CalculateNextReview(DateTime.UtcNow);
        }

        public bool NeedsReview => 
            NextReviewAt.HasValue && DateTime.UtcNow > NextReviewAt.Value;

        public int TotalRelatedEntities => 
            RelatedGoalIds.Count + RelatedAreaIds.Count + RelatedProjectIds.Count;

        private DateTime CalculateNextReview(DateTime from)
        {
            return ReviewFrequency switch
            {
                HorizonReviewFrequency.Daily => from.AddDays(1),
                HorizonReviewFrequency.Weekly => from.AddDays(7),
                HorizonReviewFrequency.Biweekly => from.AddDays(14),
                HorizonReviewFrequency.Monthly => from.AddMonths(1),
                HorizonReviewFrequency.Quarterly => from.AddMonths(3),
                HorizonReviewFrequency.Semiannually => from.AddMonths(6),
                HorizonReviewFrequency.Annually => from.AddYears(1),
                _ => from.AddDays(7)
            };
        }

        private static HorizonReviewFrequency GetDefaultReviewFrequency(GtdHorizonLevel level)
        {
            return level switch
            {
                GtdHorizonLevel.Horizon0Ground => HorizonReviewFrequency.Daily,
                GtdHorizonLevel.Horizon1Projects => HorizonReviewFrequency.Weekly,
                GtdHorizonLevel.Horizon2Areas => HorizonReviewFrequency.Monthly,
                GtdHorizonLevel.Horizon3Goals => HorizonReviewFrequency.Quarterly,
                GtdHorizonLevel.Horizon4Vision => HorizonReviewFrequency.Semiannually,
                GtdHorizonLevel.Horizon5Purpose => HorizonReviewFrequency.Annually,
                _ => HorizonReviewFrequency.Weekly
            };
        }
    }
}
