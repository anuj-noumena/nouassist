namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities
{
    /// <summary>
    /// Represents a user's progress on a learning resource
    /// </summary>
    public class UserProgress : Entity, IMultiTenant
    {
        public Guid LearningResourceId { get; private set; }
        public Guid UserId { get; private set; }
        public CompletionStatus Status { get; private set; }
        public int ProgressPercentage { get; private set; }
        public DateTime LastAccessedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public Guid? TenantId { get; private set; }

        protected UserProgress() { }

        internal UserProgress(
            Guid learningResourceId, 
            Guid userId, 
            CompletionStatus status = CompletionStatus.NotStarted, 
            int progressPercentage = 0, 
            DateTime? lastAccessedAt = null, 
            Guid? tenantId = null)
        {
            LearningResourceId = learningResourceId;
            UserId = userId;
            Status = status;
            ProgressPercentage = Check.Range(progressPercentage, nameof(progressPercentage), 0, 100);
            LastAccessedAt = lastAccessedAt ?? DateTime.UtcNow;
            TenantId = tenantId;
        }

        internal void UpdateProgress(int progressPercentage, DateTime accessTime)
        {
            ProgressPercentage = Check.Range(progressPercentage, nameof(progressPercentage), 0, 100);
            LastAccessedAt = accessTime;

            if (progressPercentage == 100 && Status != CompletionStatus.Completed)
            {
                Status = CompletionStatus.Completed;
                CompletedAt = accessTime;
            }
            else if (progressPercentage > 0 && progressPercentage < 100)
            {
                Status = CompletionStatus.InProgress;
            }
        }

        internal void MarkAsCompleted(DateTime completionTime)
        {
            Status = CompletionStatus.Completed;
            ProgressPercentage = 100;
            CompletedAt = completionTime;
            LastAccessedAt = completionTime;
        }

        internal void MarkAsAbandoned(DateTime abandonTime)
        {
            Status = CompletionStatus.Abandoned;
            LastAccessedAt = abandonTime;
        }

        public override object[] GetKeys()
        {
            return new object[] { LearningResourceId, UserId };
        }
    }
}
