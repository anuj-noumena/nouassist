using System.Collections.Generic;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Specification.ValueObjects
{
    /// <summary>
    /// Value object representing an acceptance criterion statement
    /// </summary>
    public class CriterionStatement : ValueObject
    {
        public string Statement { get; private set; }
        public string Description { get; private set; }
        public CriterionType Type { get; private set; }

        protected CriterionStatement() { }

        public CriterionStatement(string statement, string description, CriterionType type)
        {
            SetStatement(statement);
            SetDescription(description);
            Type = type;
        }

        private void SetStatement(string statement)
        {
            Statement = Check.NotNullOrWhiteSpace(
                statement,
                nameof(statement),
                maxLength: RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxStatementLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxDescriptionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Statement;
            yield return Description;
            yield return Type;
        }
    }
}
