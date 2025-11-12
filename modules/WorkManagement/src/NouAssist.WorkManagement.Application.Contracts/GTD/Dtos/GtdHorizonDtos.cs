using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.GTD.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.GTD.Dtos
{
    public class GtdHorizonDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public GtdHorizonLevel HorizonLevel { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? LifePurpose { get; set; }
        public List<string> CoreValues { get; set; }
        public string? VisionStatement { get; set; }
        public List<string> KeyThemes { get; set; }
        public List<Guid> RelatedGoalIds { get; set; }
        public List<Guid> RelatedAreaIds { get; set; }
        public List<Guid> RelatedProjectIds { get; set; }
        public HorizonReviewFrequency ReviewFrequency { get; set; }
        public DateTime? LastReviewedAt { get; set; }
        public DateTime? NextReviewAt { get; set; }
        public int ReviewCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDraft { get; set; }
        public int Version { get; set; }
        public bool NeedsReview { get; set; }
        public int TotalRelatedEntities { get; set; }
    }

    public class CreateGtdHorizonDto
    {
        public GtdHorizonLevel HorizonLevel { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateGtdHorizonDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? LifePurpose { get; set; }
        public string? VisionStatement { get; set; }
    }

    public class LinkEntityToHorizonDto
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; } // "Goal", "Area", "Project"
    }

    public class GtdHorizonAlignmentDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public Guid HigherHorizonId { get; set; }
        public GtdHorizonLevel HigherLevel { get; set; }
        public string LowerEntityType { get; set; }
        public Guid LowerEntityId { get; set; }
        public int? AlignmentStrength { get; set; }
        public string? AlignmentNotes { get; set; }
        public DateTime? LastValidatedAt { get; set; }
        public bool IsValidated { get; set; }
        public bool HasStrongAlignment { get; set; }
        public bool HasWeakAlignment { get; set; }
    }

    public class CreateAlignmentDto
    {
        public Guid HigherHorizonId { get; set; }
        public string LowerEntityType { get; set; }
        public Guid LowerEntityId { get; set; }
        public int? AlignmentStrength { get; set; }
    }

    public class ValidateAlignmentDto
    {
        public bool IsAligned { get; set; }
        public string? ValidationNotes { get; set; }
    }
}
