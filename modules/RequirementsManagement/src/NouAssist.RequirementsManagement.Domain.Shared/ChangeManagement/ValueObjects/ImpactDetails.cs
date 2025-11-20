using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ChangeManagement.ValueObjects
{
    /// <summary>
    /// Represents detailed impact information including cost, time, and dependencies
    /// </summary>
    public class ImpactDetails : ValueObject
    {
        public decimal? CostImpact { get; private set; }
        public int? TimeImpactDays { get; private set; }
        public string Dependencies { get; private set; }
        public ImpactType Type { get; private set; }

        private ImpactDetails() { }

        public ImpactDetails(
            ImpactType type,
            decimal? costImpact = null,
            int? timeImpactDays = null,
            string dependencies = null)
        {
            Type = type;
            CostImpact = costImpact;
            TimeImpactDays = timeImpactDays;
            Dependencies = dependencies;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return CostImpact ?? 0;
            yield return TimeImpactDays ?? 0;
            yield return Dependencies ?? string.Empty;
        }

        public bool HasCostImpact => CostImpact.HasValue && CostImpact > 0;
        public bool HasTimeImpact => TimeImpactDays.HasValue && TimeImpactDays > 0;
        public bool HasDependencies => !string.IsNullOrWhiteSpace(Dependencies);
    }
}
