using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.Workflow.Entities
{
    /// <summary>
    /// Simplified custom field configuration
    /// </summary>
    public class WorkflowCustomField : FullAuditedEntity<Guid>, IHasConcurrencyStamp
    {
        public Guid WorkflowSchemeId { get; private set; }
        public Guid CustomFieldDefinitionId { get; private set; }

        // === DISPLAY ===
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        // === RULES (Simplified as JSON) ===
        public string VisibilityRules { get; private set; } // JSON
        public string ValidationRules { get; private set; } // JSON

        protected WorkflowCustomField()
        {
        }

        public WorkflowCustomField(
            Guid id,
            Guid workflowSchemeId,
            Guid customFieldDefinitionId,
            bool isRequired = false,
            string defaultValue = null,
            int displayOrder = 0) : base(id)
        {
            WorkflowSchemeId = workflowSchemeId;
            CustomFieldDefinitionId = customFieldDefinitionId;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
            DisplayOrder = displayOrder;
            IsActive = true;
        }

        public void SetVisibilityRules(FieldVisibilityRules rules)
        {
            VisibilityRules = JsonSerializer.Serialize(rules);
        }

        public void SetValidationRules(FieldValidationRules rules)
        {
            ValidationRules = JsonSerializer.Serialize(rules);
        }

        public bool IsVisibleInStatus(Guid statusId)
        {
            if (string.IsNullOrEmpty(VisibilityRules)) return true;

            var rules = JsonSerializer.Deserialize<FieldVisibilityRules>(VisibilityRules);
            return rules.IsVisibleInStatus(statusId);
        }

        public string ConcurrencyStamp { get; set; }
    }

    // Supporting classes
    public class FieldVisibilityRules
    {
        public List<Guid> VisibleInStatusIds { get; set; } = new();
        public List<Guid> HiddenInStatusIds { get; set; } = new();
        public List<Guid> ReadOnlyInStatusIds { get; set; } = new();

        public bool IsVisibleInStatus(Guid statusId)
        {
            if (HiddenInStatusIds.Contains(statusId)) return false;
            if (VisibleInStatusIds.Any() && !VisibleInStatusIds.Contains(statusId)) return false;
            return true;
        }
    }

    public class FieldValidationRules
    {
        // === STRING VALIDATIONS ===
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public string RegexPattern { get; set; }
        public string RegexErrorMessage { get; set; }

        // === NUMERIC VALIDATIONS ===
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }

        // === DATE VALIDATIONS ===
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool MustBeFutureDate { get; set; }
        public bool MustBePastDate { get; set; }

        // === COLLECTION VALIDATIONS ===
        public int? MinSelections { get; set; }
        public int? MaxSelections { get; set; }
        public List<string> AllowedValues { get; set; } = new();
        public List<string> DisallowedValues { get; set; } = new();

        // === STATUS-BASED VALIDATIONS ===
        public List<Guid> RequiredInStatusIds { get; set; } = new();
        public List<Guid> OptionalInStatusIds { get; set; } = new();

        // === CUSTOM VALIDATIONS ===
        public string CustomValidationExpression { get; set; }
        public string CustomErrorMessage { get; set; }

        /// <summary>
        /// Validates a string value against the rules
        /// </summary>
        public ValidationResult ValidateString(string value, Guid? currentStatusId = null)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(value))
            {
                if (IsRequiredInStatus(currentStatusId))
                {
                    result.IsValid = false;
                    result.Errors.Add("Field is required in the current status");
                }

                return result;
            }

            // Length validations
            if (MinLength.HasValue && value.Length < MinLength.Value)
            {
                result.IsValid = false;
                result.Errors.Add($"Minimum length is {MinLength.Value} characters");
            }

            if (MaxLength.HasValue && value.Length > MaxLength.Value)
            {
                result.IsValid = false;
                result.Errors.Add($"Maximum length is {MaxLength.Value} characters");
            }

            // Regex validation
            if (!string.IsNullOrEmpty(RegexPattern))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, RegexPattern))
                {
                    result.IsValid = false;
                    result.Errors.Add(RegexErrorMessage ?? "Value does not match required pattern");
                }
            }

            // Allowed/Disallowed values
            if (AllowedValues.Any() && !AllowedValues.Contains(value))
            {
                result.IsValid = false;
                result.Errors.Add($"Value must be one of: {string.Join(", ", AllowedValues)}");
            }

            if (DisallowedValues.Contains(value))
            {
                result.IsValid = false;
                result.Errors.Add("This value is not allowed");
            }

            return result;
        }

        /// <summary>
        /// Validates a numeric value against the rules
        /// </summary>
        public ValidationResult ValidateNumeric(decimal? value, Guid? currentStatusId = null)
        {
            var result = new ValidationResult { IsValid = true };

            if (!value.HasValue)
            {
                if (IsRequiredInStatus(currentStatusId))
                {
                    result.IsValid = false;
                    result.Errors.Add("Field is required in the current status");
                }

                return result;
            }

            if (MinValue.HasValue && value.Value < MinValue.Value)
            {
                result.IsValid = false;
                result.Errors.Add($"Minimum value is {MinValue.Value}");
            }

            if (MaxValue.HasValue && value.Value > MaxValue.Value)
            {
                result.IsValid = false;
                result.Errors.Add($"Maximum value is {MaxValue.Value}");
            }

            return result;
        }

        /// <summary>
        /// Validates a date value against the rules
        /// </summary>
        public ValidationResult ValidateDate(DateTime? value, Guid? currentStatusId = null)
        {
            var result = new ValidationResult { IsValid = true };

            if (!value.HasValue)
            {
                if (IsRequiredInStatus(currentStatusId))
                {
                    result.IsValid = false;
                    result.Errors.Add("Field is required in the current status");
                }

                return result;
            }

            if (MinDate.HasValue && value.Value < MinDate.Value)
            {
                result.IsValid = false;
                result.Errors.Add($"Date must be after {MinDate.Value:yyyy-MM-dd}");
            }

            if (MaxDate.HasValue && value.Value > MaxDate.Value)
            {
                result.IsValid = false;
                result.Errors.Add($"Date must be before {MaxDate.Value:yyyy-MM-dd}");
            }

            if (MustBeFutureDate && value.Value <= DateTime.UtcNow)
            {
                result.IsValid = false;
                result.Errors.Add("Date must be in the future");
            }

            if (MustBePastDate && value.Value >= DateTime.UtcNow)
            {
                result.IsValid = false;
                result.Errors.Add("Date must be in the past");
            }

            return result;
        }

        /// <summary>
        /// Validates a collection of values against the rules
        /// </summary>
        public ValidationResult ValidateCollection(List<string> values, Guid? currentStatusId = null)
        {
            var result = new ValidationResult { IsValid = true };

            if (values == null || !values.Any())
            {
                if (IsRequiredInStatus(currentStatusId))
                {
                    result.IsValid = false;
                    result.Errors.Add("At least one selection is required in the current status");
                }

                return result;
            }

            if (MinSelections.HasValue && values.Count < MinSelections.Value)
            {
                result.IsValid = false;
                result.Errors.Add($"Minimum {MinSelections.Value} selection(s) required");
            }

            if (MaxSelections.HasValue && values.Count > MaxSelections.Value)
            {
                result.IsValid = false;
                result.Errors.Add($"Maximum {MaxSelections.Value} selection(s) allowed");
            }

            // Validate each value
            foreach (var value in values)
            {
                if (AllowedValues.Any() && !AllowedValues.Contains(value))
                {
                    result.IsValid = false;
                    result.Errors.Add($"'{value}' is not an allowed value");
                }

                if (DisallowedValues.Contains(value))
                {
                    result.IsValid = false;
                    result.Errors.Add($"'{value}' is not allowed");
                }
            }

            return result;
        }

        /// <summary>
        /// Checks if field is required in the given status
        /// </summary>
        public bool IsRequiredInStatus(Guid? statusId)
        {
            if (!statusId.HasValue) return false;

            if (OptionalInStatusIds.Contains(statusId.Value)) return false;
            if (RequiredInStatusIds.Contains(statusId.Value)) return true;

            return false;
        }
    }

    /// <summary>
    /// Result of a validation operation
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();

        public string GetErrorMessage() => string.Join("; ", Errors);
    }
}