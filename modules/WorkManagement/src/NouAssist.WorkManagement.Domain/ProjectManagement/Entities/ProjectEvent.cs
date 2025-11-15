// src/NouAssist.CoreProductivity.ProjectManagement.Domain/Entities/ProjectEvent.cs

using System;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.ProjectManagement.Entities
{
    public class ProjectEvent : FullAuditedEntity<Guid>
    {
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public ProjectEventType EventType { get; private set; }
        public DateTime EventDate { get; private set; }
        public DateTime? EndDate { get; set; }
        public bool IsAllDay { get; set; }
        public string Location { get; set; }
        public Guid? RelatedSprintId { get; set; }
        public Guid? RelatedMilestoneId { get; set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        protected ProjectEvent() { }

        public ProjectEvent(
            Guid id,
            Guid projectId,
            string title,
            ProjectEventType eventType,
            DateTime eventDate,
            string description = null) : base(id)
        {
            ProjectId = projectId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            EventType = eventType;
            EventDate = eventDate;
            Description = description;
            IsCompleted = false;
        }
        
        public void Update(string title, string description, DateTime eventDate, DateTime? endDate, bool isAllDay, string location)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            EventDate = eventDate;
            EndDate = endDate;
            IsAllDay = isAllDay;
            Location = location;
        }

        public void MarkAsCompleted()
        {
            if (IsCompleted)
            {
                return;
            }

            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }

        public void Reschedule(DateTime newDate)
        {
            if (IsCompleted)
            {
                throw new BusinessException("Projects:CannotRescheduleCompleted");
            }

            EventDate = newDate;
        }

        public bool IsPast => DateTime.UtcNow > EventDate;
        public bool IsToday => EventDate.Date == DateTime.UtcNow.Date;
        public bool IsUpcoming => EventDate > DateTime.UtcNow;
    }
}