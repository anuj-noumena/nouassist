using System.Collections.Generic;
using NouAssist.RequirementsManagement.Specification.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Specification.ValueObjects
{
    /// <summary>
    /// Value object representing requirement text content
    /// </summary>
    public class RequirementText : ValueObject
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        protected RequirementText() { }

        public RequirementText(string title, string description)
        {
            SetTitle(title);
            SetDescription(description);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: RequirementSpecificationManagementConsts.Requirement.MaxTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: RequirementSpecificationManagementConsts.Requirement.MaxDescriptionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Description;
        }
    }
}
