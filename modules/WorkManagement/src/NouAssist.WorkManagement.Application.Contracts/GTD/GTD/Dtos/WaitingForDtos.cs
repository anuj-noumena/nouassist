using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.GTD.GTD.Dtos
{
    public class WaitingForDto : FullAuditedEntityDto<Guid>
    {
        public Guid? TaskInstanceId { get; set; }
        public Guid OwnerId { get; set; }
        public WaitingForType WfType { get; set; }
        public Guid? ForUserId { get; set; }
        public string? Reason { get; set; }
        public WaitingForStatus Status { get; set; }
        public string? Note { get; set; }
        public DateTime? ClearedAt { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int WaitingDays { get; set; }
        public bool IsFollowUpDue { get; set; }
    }

    public class CreateWaitingForDto
    {
        public Guid? TaskInstanceId { get; set; }
        public WaitingForType WfType { get; set; } = WaitingForType.User;
        public Guid? ForUserId { get; set; }
        public string? Reason { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }

    public class UpdateWaitingForDto
    {
        public string? Reason { get; set; }
        public string? Note { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }

    public class FollowupDto : FullAuditedEntityDto<Guid>
    {
        public Guid? TaskInstanceId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime FollowupAt { get; set; }
        public string? Note { get; set; }
        public FollowupStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDue { get; set; }
    }

    public class CreateFollowupDto
    {
        public Guid? TaskInstanceId { get; set; }
        [Required]
        public DateTime FollowupAt { get; set; }
        public string? Note { get; set; }
    }
}
