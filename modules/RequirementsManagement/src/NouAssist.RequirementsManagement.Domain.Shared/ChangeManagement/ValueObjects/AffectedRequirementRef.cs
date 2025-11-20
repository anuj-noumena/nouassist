using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ChangeManagement.ValueObjects
{
    /// <summary>
    /// Represents a reference to an affected requirement by ID and version
    /// </summary>
    public class AffectedRequirementRef : ValueObject
    {
        public Guid RequirementId { get; private set; }
        public string Version { get; private set; }

        private AffectedRequirementRef() { }

        public AffectedRequirementRef(Guid requirementId, string version)
        {
            RequirementId = Check.NotEmpty(requirementId, nameof(requirementId));
            Version = Check.NotNullOrWhiteSpace(version, nameof(version));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return RequirementId;
            yield return Version;
        }

        public override string ToString()
        {
            return $"{RequirementId}@{Version}";
        }
    }
}
