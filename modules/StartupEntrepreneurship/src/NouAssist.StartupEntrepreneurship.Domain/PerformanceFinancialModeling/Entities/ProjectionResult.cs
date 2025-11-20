namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities
{
    /// <summary>
    /// Represents the results of a financial projection calculation
    /// </summary>
    public class ProjectionResult : Entity<Guid>
    {
        public Guid ModelId { get; private set; }
        public Guid ScenarioId { get; private set; }
        public ProjectionType Type { get; private set; }
        public DateTime CalculatedAt { get; private set; }
        public string Notes { get; private set; }

        private readonly List<ProjectedAmount> _projectedAmounts;
        public IReadOnlyCollection<ProjectedAmount> ProjectedAmounts => _projectedAmounts.AsReadOnly();

        protected ProjectionResult() 
        {
            _projectedAmounts = new List<ProjectedAmount>();
        }

        internal ProjectionResult(Guid modelId, Guid scenarioId, ProjectionType type, 
                                List<ProjectedAmount> projectedAmounts, string notes = null) : base(Guid.NewGuid())
        {
            ModelId = modelId;
            ScenarioId = scenarioId;
            Type = type;
            CalculatedAt = DateTime.UtcNow;
            SetNotes(notes);
            
            _projectedAmounts = projectedAmounts ?? throw new ArgumentNullException(nameof(projectedAmounts));
            
            if (!_projectedAmounts.Any())
            {
                throw new ArgumentException("Projected amounts cannot be empty", nameof(projectedAmounts));
            }
        }

        public void UpdateNotes(string notes)
        {
            SetNotes(notes);
        }

        public ProjectedAmount GetProjectionForPeriod(int periodIndex)
        {
            return _projectedAmounts.FirstOrDefault(p => p.PeriodIndex == periodIndex);
        }

        public ProjectedAmount GetProjectionForDate(DateTime date)
        {
            return _projectedAmounts
                .OrderBy(p => Math.Abs((p.ProjectionDate - date).TotalDays))
                .FirstOrDefault();
        }

        public decimal GetTotalProjected()
        {
            return _projectedAmounts.Sum(p => p.Amount);
        }

        public decimal GetAverageProjected()
        {
            return _projectedAmounts.Any() ? _projectedAmounts.Average(p => p.Amount) : 0;
        }

        public ProjectedAmount GetMaxProjection()
        {
            return _projectedAmounts.OrderByDescending(p => p.Amount).FirstOrDefault();
        }

        public ProjectedAmount GetMinProjection()
        {
            return _projectedAmounts.OrderBy(p => p.Amount).FirstOrDefault();
        }

        public decimal GetGrowthRate()
        {
            if (_projectedAmounts.Count < 2) return 0;

            var first = _projectedAmounts.OrderBy(p => p.PeriodIndex).First();
            var last = _projectedAmounts.OrderBy(p => p.PeriodIndex).Last();

            if (first.Amount == 0) return 0;

            var periods = last.PeriodIndex - first.PeriodIndex;
            if (periods == 0) return 0;

            return (decimal)(Math.Pow((double)(last.Amount / first.Amount), 1.0 / periods) - 1) * 100;
        }

        public int? GetRunwayMonths()
        {
            if (Type != ProjectionType.CashFlow) return null;

            var negativeMonth = _projectedAmounts
                .OrderBy(p => p.PeriodIndex)
                .FirstOrDefault(p => p.Amount < 0);

            return negativeMonth?.PeriodIndex;
        }

        public DateTime? GetBreakEvenDate()
        {
            if (Type != ProjectionType.CashFlow && Type != ProjectionType.ProfitAndLoss) return null;

            var breakEvenProjection = _projectedAmounts
                .OrderBy(p => p.PeriodIndex)
                .FirstOrDefault(p => p.Amount >= 0);

            return breakEvenProjection?.ProjectionDate;
        }

        private void SetNotes(string notes)
        {
            if (!string.IsNullOrEmpty(notes))
            {
                Notes = Check.Length(
                    notes.Trim(),
                    nameof(notes),
                    maxLength: PerformanceFinancialModelingConsts.MaxNotesLength
                );
            }
        }
    }
}
