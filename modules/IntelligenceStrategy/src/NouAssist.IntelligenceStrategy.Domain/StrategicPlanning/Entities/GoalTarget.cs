namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Entities
{
    /// <summary>
    /// Represents a measurable target for a strategic goal
    /// </summary>
    public class GoalTarget : Entity<Guid>
    {
        public Guid StrategicGoalId { get; private set; }
        public TargetMetric Metric { get; private set; }
        public decimal? CurrentValue { get; private set; }
        public bool IsAchieved { get; private set; }
        public DateTime? AchievedDate { get; private set; }
        public string Notes { get; private set; }

        protected GoalTarget() { }

        internal GoalTarget(
            Guid id,
            Guid strategicGoalId,
            TargetMetric metric,
            string notes = null) : base(id)
        {
            StrategicGoalId = strategicGoalId;
            Metric = metric;
            Notes = notes;
            IsAchieved = false;
        }

        internal void UpdateProgress(decimal currentValue, DateTime measurementDate)
        {
            CurrentValue = currentValue;
            
            if (!IsAchieved && Metric.IsAchieved(currentValue))
            {
                IsAchieved = true;
                AchievedDate = measurementDate;
            }
            else if (IsAchieved && !Metric.IsAchieved(currentValue))
            {
                IsAchieved = false;
                AchievedDate = null;
            }
        }

        internal void UpdateNotes(string notes)
        {
            Notes = Check.Length(notes, nameof(notes), StrategicPlanningConsts.MaxNotesLength);
        }
    }
}
