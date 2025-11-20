using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ValidationVerification.ValueObjects
{
    /// <summary>
    /// Reference to a workflow definition
    /// </summary>
    public class WorkflowDefinitionRef : ValueObject
    {
        public string Name { get; private set; }
        public string Version { get; private set; }
        public Guid DefinitionId { get; private set; }

        private WorkflowDefinitionRef() { }

        public WorkflowDefinitionRef(Guid definitionId, string name, string version)
        {
            DefinitionId = definitionId;
            SetName(name);
            SetVersion(version);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ValidationVerificationConsts.WorkflowDefinition.MaxNameLength
            );
        }

        private void SetVersion(string version)
        {
            Version = Check.NotNullOrWhiteSpace(
                version,
                nameof(version),
                maxLength: ValidationVerificationConsts.WorkflowDefinition.MaxVersionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DefinitionId;
            yield return Name;
            yield return Version;
        }
    }
}
