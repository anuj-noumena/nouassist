using System.Collections.Generic;
using NouAssist.RequirementsManagement.Specification.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Specification.ValueObjects
{
    /// <summary>
    /// Value object representing a unique requirement identifier
    /// </summary>
    public class RequirementIdentifier : ValueObject
    {
        public string Value { get; private set; }

        protected RequirementIdentifier() { }

        public RequirementIdentifier(string value)
        {
            SetValue(value);
        }

        private void SetValue(string value)
        {
            Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: RequirementSpecificationManagementConsts.Requirement.MaxIdentifierLength
            );

            // Validate format (e.g., REQ-001, FR-001, etc.)
            if (!IsValidFormat(value))
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:InvalidIdentifierFormat")
                    .WithData("identifier", value);
            }

            Value = value.ToUpperInvariant();
        }

        private static bool IsValidFormat(string value)
        {
            // Allow alphanumeric characters, hyphens, and underscores
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[A-Za-z0-9_-]+$");
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(RequirementIdentifier identifier)
        {
            return identifier?.Value;
        }

        public static implicit operator RequirementIdentifier(string value)
        {
            return value == null ? null : new RequirementIdentifier(value);
        }
    }
}
