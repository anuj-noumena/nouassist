using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SharedKernel.Inbox.Dtos
{
    
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
    
    public class UpdateInboxItemDto
    {
        public string? InboxContent { get; set; }
        public string? Notes { get; set; }
    }
}
