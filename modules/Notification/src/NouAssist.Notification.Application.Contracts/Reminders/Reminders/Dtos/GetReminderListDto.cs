using System;
using NouAssist.Notification.Reminders.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Notification.Reminders.Reminders.Dtos
{
    public class GetReminderListDto : PagedAndSortedResultRequestDto
    {
        public ReminderStatus? Status { get; set; }
        public ReminderEntityType? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsSent { get; set; }
        public ReminderPriority? Priority { get; set; }
    }
}
