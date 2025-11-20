namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities
{
    /// <summary>
    /// Represents a goal set for a metric
    /// </summary>
    public class MetricGoal : Entity<Guid>
    {
        public Guid MetricId { get; private set; }
        public GoalTarget Target { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsAchieved { get; private set; }
        public DateTime? AchievedAt { get; private set; }

        protected MetricGoal() { }

        internal MetricGoal(Guid metricId, GoalTarget target, string description = null) : base(Guid.NewGuid())
        {
            MetricId = metricId;
            Target = target ?? throw new ArgumentNullException(nameof(target));
            SetDescription(description);
            IsActive = true;
            IsAchieved = false;
        }

        public void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        public void Activate()
        {
            if (Target.IsOverdue())
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:OverdueGoal");
            }
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void MarkAsAchieved()
        {
            IsAchieved = true;
            AchievedAt = DateTime.UtcNow;
        }

        public decimal GetProgress(MetricValue currentValue, MetricValue initialValue = null)
        {
            return Target.GetProgress(currentValue, initialValue);
        }

        public bool IsOnTrack(MetricValue currentValue, decimal acceptableVariance = 0.1m)
        {
            if (IsAchieved) return true;
            if (Target.IsOverdue()) return false;

            var progress = Target.GetProgress(currentValue);
            var timeProgress = GetTimeProgress();
            
            // Check if we're within acceptable variance of expected progress
            return Math.Abs(progress - timeProgress) <= acceptableVariance * 100;
        }

        public decimal GetTimeProgress()
        {
            var totalDays = (Target.TargetDate - DateTime.UtcNow.AddDays(-Target.GetDaysRemaining())).Days;
            var elapsedDays = totalDays - Target.GetDaysRemaining();
            
            if (totalDays <= 0) return 100;
            return Math.Max(0, Math.Min(100, (elapsedDays / (decimal)totalDays) * 100));
        }

        private void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                Description = Check.Length(
                    description.Trim(),
                    nameof(description),
                    maxLength: PerformanceFinancialModelingConsts.MaxNotesLength
                );
            }
        }
    }
}
