using System;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.ProjectManagement.Events.Dtos
{
    public class ProjectEventDto : FullAuditedEntityDto<Guid>
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectEventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAllDay { get; set; }
        public string Location { get; set; }
        public Guid? RelatedSprintId { get; set; }
        public Guid? RelatedMilestoneId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Computed
        public bool IsPast { get; set; }
        public bool IsToday { get; set; }
        public bool IsUpcoming { get; set; }
    }
}
