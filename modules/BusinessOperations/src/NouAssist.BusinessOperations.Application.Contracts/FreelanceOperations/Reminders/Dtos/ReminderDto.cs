using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Reminders.Dtos
{
    public class ReminderDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DueDate { get; set; }
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string EntityTypeName { get; set; }
        public string EntityLink { get; set; }
        public bool IsOverdue { get; set; }
    }
}
