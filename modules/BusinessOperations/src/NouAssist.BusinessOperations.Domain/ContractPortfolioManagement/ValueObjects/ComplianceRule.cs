using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a compliance rule for contracts or portfolios
    /// </summary>
    public class ComplianceRule : ValueObject
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string RequirementText { get; private set; }
        public bool IsMandatory { get; private set; }

        protected ComplianceRule() { }

        public ComplianceRule(string code, string name, string description, string requirementText, bool isMandatory)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Description = description;
            RequirementText = Check.NotNullOrWhiteSpace(requirementText, nameof(requirementText));
            IsMandatory = isMandatory;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Name;
            yield return Description ?? string.Empty;
            yield return RequirementText;
            yield return IsMandatory;
        }
    }
}
