namespace NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects
{
    /// <summary>
    /// Value object representing a timeline date with planning context
    /// </summary>
    public class TimelineDate : ValueObject
    {
        public DateTime Date { get; private set; }
        public PlanningHorizon Horizon { get; private set; }
        public string Description { get; private set; }
        public bool IsEstimated { get; private set; }

        private TimelineDate() { }

        public TimelineDate(DateTime date, PlanningHorizon horizon, string description, bool isEstimated = false)
        {
            Date = date;
            Horizon = horizon;
            SetDescription(description);
            IsEstimated = isEstimated;
        }

        public static TimelineDate CreateMilestone(DateTime date, string description)
        {
            var horizon = DetermineHorizon(date);
            return new TimelineDate(date, horizon, description, false);
        }

        public static TimelineDate CreateEstimate(DateTime date, string description)
        {
            var horizon = DetermineHorizon(date);
            return new TimelineDate(date, horizon, description, true);
        }

        private static PlanningHorizon DetermineHorizon(DateTime date)
        {
            var monthsFromNow = (date.Year - DateTime.UtcNow.Year) * 12 + date.Month - DateTime.UtcNow.Month;
            
            return monthsFromNow switch
            {
                <= 24 => PlanningHorizon.H1,
                <= 60 => PlanningHorizon.H2,
                _ => PlanningHorizon.H3
            };
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 200);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Date;
            yield return Horizon;
            yield return Description;
            yield return IsEstimated;
        }
    }
}
