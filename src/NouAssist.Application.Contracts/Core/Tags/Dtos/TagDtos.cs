using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Core.Dtos
{
    public class TagDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Color { get; set; }
        public Guid? CategoryId { get; set; }
        public int UsageCount { get; set; }
        public bool IsPopular { get; set; }
    }

    public class CreateTagDto
    {
        public string Title { get; set; }
        public string? Color { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class ContextDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Notes { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Location { get; set; }
        public int? EnergyLevel { get; set; }
        public bool IsActive { get; set; }
        public int UsageCount { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public bool IsArchived { get; set; }
    }

    public class CreateContextDto
    {
        public string Title { get; set; }
        public string? Notes { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Location { get; set; }
        public int? EnergyLevel { get; set; }
    }

    public class AreaDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public string? Purpose { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastReviewedAt { get; set; }
        public bool IsArchived { get; set; }
        public bool HasParent { get; set; }
        public bool IsRootArea { get; set; }
    }

    public class CreateAreaDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public string? Purpose { get; set; }
    }

    public class InboxItemDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string InboxContent { get; set; }
        public string ItemType { get; set; }
        public string Source { get; set; }
        public bool Processed { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ProcessedToEntityType { get; set; }
        public Guid? ProcessedToEntityId { get; set; }
        public string? Notes { get; set; }
        public int AgeInDays { get; set; }
        public bool IsStale { get; set; }
    }

    public class CreateInboxItemDto
    {
        public string InboxContent { get; set; }
        public string? ItemType { get; set; }
        public string? Source { get; set; }
    }

    public class ProcessInboxItemDto
    {
        public string? ProcessedToEntityType { get; set; }
        public Guid? ProcessedToEntityId { get; set; }
        public string? Notes { get; set; }
    }
}
