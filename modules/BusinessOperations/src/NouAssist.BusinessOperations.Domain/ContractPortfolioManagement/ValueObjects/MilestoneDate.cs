namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a milestone date in a roadmap
    /// </summary>
    public class MilestoneDate : ValueObject
    {
        public DateTime PlannedDate { get; private set; }
        public DateTime? ActualDate { get; private set; }
        public bool IsCompleted { get; private set; }

        protected MilestoneDate() { }

        public MilestoneDate(DateTime plannedDate, DateTime? actualDate = null)
        {
            PlannedDate = plannedDate;
            ActualDate = actualDate;
            IsCompleted = actualDate.HasValue;
        }

        public void MarkAsCompleted(DateTime completionDate)
        {
            ActualDate = completionDate;
            IsCompleted = true;
        }

        public void ResetCompletion()
        {
            ActualDate = null;
            IsCompleted = false;
        }

        public int CalculateDaysVariance()
        {
            if (!ActualDate.HasValue)
            {
                return 0;
            }

            return (int)(ActualDate.Value - PlannedDate).TotalDays;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PlannedDate;
            yield return ActualDate ?? DateTime.MinValue;
            yield return IsCompleted;
        }
    }
}
