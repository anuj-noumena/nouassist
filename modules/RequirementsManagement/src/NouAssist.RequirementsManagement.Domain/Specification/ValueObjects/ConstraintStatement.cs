using System.Collections.Generic;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Specification.ValueObjects
{
    /// <summary>
    /// Value object representing a constraint statement
    /// </summary>
    public class ConstraintStatement : ValueObject
    {
        public string Name { get; private set; }
        public string Statement { get; private set; }
        public ConstraintType Type { get; private set; }

        protected ConstraintStatement() { }

        public ConstraintStatement(string name, string statement, ConstraintType type)
        {
            SetName(name);
            SetStatement(statement);
            Type = type;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: RequirementSpecificationManagementConsts.Constraint.MaxNameLength
            );
        }

        private void SetStatement(string statement)
        {
            Statement = Check.NotNullOrWhiteSpace(
                statement,
                nameof(statement),
                maxLength: RequirementSpecificationManagementConsts.Constraint.MaxStatementLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Statement;
            yield return Type;
        }
    }
}
