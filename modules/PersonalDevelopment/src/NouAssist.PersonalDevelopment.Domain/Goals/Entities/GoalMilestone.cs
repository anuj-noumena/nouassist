using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.PersonalDevelopment.Goals.Entities
{
    public class GoalMilestone : FullAuditedEntity<Guid>
    {
        public Guid GoalId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; set; }
        public DateTime? TargetDate { get; set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public int OrderIndex { get; set; }

        protected GoalMilestone() { }

        public GoalMilestone(Guid id, Guid goalId, string title) : base(id)
        {
            GoalId = goalId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
            IsCompleted = false;
            OrderIndex = 0;
        }

        public void Complete()
        {
            if (IsCompleted) return;
            
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }

        public void Uncomplete()
        {
            IsCompleted = false;
            CompletedAt = null;
        }

        public void UpdateTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
        }

        public bool IsOverdue => 
            !IsCompleted && 
            TargetDate.HasValue && 
            DateTime.UtcNow > TargetDate.Value;

        public TimeSpan? GetTimeRemaining()
        {
            if (IsCompleted || !TargetDate.HasValue)
                return null;
            return TargetDate.Value - DateTime.UtcNow;
        }
    }
}
