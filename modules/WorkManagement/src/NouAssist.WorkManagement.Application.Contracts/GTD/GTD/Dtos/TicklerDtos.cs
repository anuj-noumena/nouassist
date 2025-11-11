using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.GTD.GTD.Dtos
{
    public class TicklerItemDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public DateTime TickleDate { get; set; }
        public TimeSpan? TickleTime { get; set; }
        public TicklerStatus Status { get; set; }
        public DateTime? SurfacedAt { get; set; }
        public DateTime? ActionedAt { get; set; }
        public DateTime? DismissedAt { get; set; }
        public DateTime? SnoozeUntil { get; set; }
        public TicklerActionType? ActionType { get; set; }
        public double ImportanceScore { get; set; }
        public double UrgencyScore { get; set; }
        public bool IsRecurring { get; set; }
        public RecurrenceRuleDto? RecurrencePattern { get; set; }
        public Guid? ContextId { get; set; }
        public bool IsDue { get; set; }
    }

    public class RecurrenceRuleDto
    {
        public string Frequency { get; set; }
        public int Interval { get; set; }
        public DateTime? Until { get; set; }
        public int? Count { get; set; }
        public string? Description { get; set; }
    }

    public class CreateTicklerItemDto
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime TickleDate { get; set; }
        public string? TickleTime { get; set; }
        public double? ImportanceScore { get; set; }
        public double? UrgencyScore { get; set; }
        public Guid? ContextId { get; set; }
    }

    public class UpdateTicklerItemDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? TickleDate { get; set; }
        public string? Notes { get; set; }
    }

    public class SnoozeTicklerDto
    {
        [Required]
        public DateTime SnoozeUntil { get; set; }
    }

    public class SetRecurrenceDto
    {
        [Required]
        public string Frequency { get; set; } // "Daily", "Weekly", "Monthly", "Yearly"
        public int Interval { get; set; } = 1;
        public DateTime? Until { get; set; }
        public int? Count { get; set; }
    }

    public class GetTicklerListDto : PagedAndSortedResultRequestDto
    {
        public TicklerStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? OnlyDue { get; set; }
    }
}
