using System.Collections.Generic;
using NouAssist.RequirementsManagement.Specification.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Specification.ValueObjects
{
    /// <summary>
    /// Value object representing a business rule description
    /// </summary>
    public class RuleDescription : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        protected RuleDescription() { }

        public RuleDescription(string name, string description)
        {
            SetName(name);
            SetDescription(description);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: RequirementSpecificationManagementConsts.BusinessRule.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: RequirementSpecificationManagementConsts.BusinessRule.MaxDescriptionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
        }
    }
}
