namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing metadata for a contract version
    /// </summary>
    public class VersionMetadata : ValueObject
    {
        public int VersionNumber { get; private set; }
        public string VersionLabel { get; private set; }
        public string ChangeReason { get; private set; }

        protected VersionMetadata() { }

        public VersionMetadata(int versionNumber, string versionLabel, string changeReason)
        {
            if (versionNumber < 1)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:InvalidValue")
                    .WithData("propertyName", "VersionNumber");
            }

            VersionNumber = versionNumber;
            VersionLabel = Check.NotNullOrWhiteSpace(versionLabel, nameof(versionLabel));
            ChangeReason = changeReason; // Can be null for initial version
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return VersionNumber;
            yield return VersionLabel;
            yield return ChangeReason ?? string.Empty;
        }
    }
}
