using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.ProjectManagement.Enums;

namespace NouAssist.WorkManagement.ProjectManagement.Events.Dtos
{
    public class CreateEventDto
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public ProjectEventType EventType { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsAllDay { get; set; }

        [StringLength(500)]
        public string Location { get; set; }

        public Guid? RelatedSprintId { get; set; }
        public Guid? RelatedMilestoneId { get; set; }
    }
}
