using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NouAssist.Customization.CustomFields.Enums;
using NouAssist.Customization.CustomFields.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Customization.CustomFields.Aggregates
{
    public class CustomFieldDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public string Name { get; private set; }
        public string FieldKey { get; private set; }
        public string Description { get; private set; }
        public CustomFieldType FieldType { get; private set; }
        public CustomFieldEntityType EntityType { get; private set; }
        
        // Validation
        public bool IsRequired { get; private set; }
        public string ValidationRegex { get; private set; }
        public string ValidationMessage { get; private set; }
        
        // Default value
        public string DefaultValue { get; private set; }
        
        // Numeric constraints
        public decimal? MinValue { get; private set; }
        public decimal? MaxValue { get; private set; }
        
        // Text constraints
        public int? MinLength { get; private set; }
        public int? MaxLength { get; private set; }
        
        // Date constraints
        public DateTime? MinDate { get; private set; }
        public DateTime? MaxDate { get; private set; }
        
        // Display
        public int DisplayOrder { get; private set; }
        public string Placeholder { get; private set; }
        public string HelpText { get; private set; }
        public string Icon { get; private set; }
        
        // Conditional display
        public Guid? ShowIfFieldId { get; private set; }
        public string ShowIfValue { get; private set; }
        
        public bool IsSystem { get; private set; }
        public bool IsActive { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<CustomFieldOption> _options = new();
        public IReadOnlyCollection<CustomFieldOption> Options => _options.AsReadOnly();

        protected CustomFieldDefinition() { /* ORM */ }

        public CustomFieldDefinition(
            Guid id,
            Guid ownerId,
            string name,
            CustomFieldType fieldType,
            CustomFieldEntityType entityType,
            Guid? tenantId
        ) : base(id)
        {
            OwnerId = ownerId;
            SetName(name);
            FieldType = fieldType;
            EntityType = entityType;
            TenantId = tenantId;
            FieldKey = GenerateFieldKey(name);
            IsActive = true;
            IsRequired = false;
            DisplayOrder = 0;
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 100);
            FieldKey = GenerateFieldKey(name);
        }

        public void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 500)
            {
                throw new BusinessException("CustomFields:DescriptionTooLong");
            }
            Description = description;
        }

        public void MakeRequired()
        {
            IsRequired = true;
        }

        public void MakeOptional()
        {
            IsRequired = false;
        }

        public void SetValidationRegex(string regex, string message = null)
        {
            if (!string.IsNullOrEmpty(regex))
            {
                try
                {
                    _ = new Regex(regex);
                }
                catch
                {
                    throw new BusinessException("CustomFields:InvalidRegex");
                }
            }
            ValidationRegex = regex;
            ValidationMessage = message;
        }

        public void SetDefaultValue(string value)
        {
            DefaultValue = value;
        }

        public void SetNumericConstraints(decimal? min, decimal? max)
        {
            if (!FieldType.IsNumeric())
            {
                throw new BusinessException("CustomFields:NotNumericField");
            }

            if (min.HasValue && max.HasValue && min.Value > max.Value)
            {
                throw new BusinessException("CustomFields:MinGreaterThanMax");
            }

            MinValue = min;
            MaxValue = max;
        }

        public void SetTextConstraints(int? minLength, int? maxLength)
        {
            if (!FieldType.IsText())
            {
                throw new BusinessException("CustomFields:NotTextField");
            }

            if (minLength.HasValue && maxLength.HasValue && minLength.Value > maxLength.Value)
            {
                throw new BusinessException("CustomFields:MinLengthGreaterThanMaxLength");
            }

            MinLength = minLength;
            MaxLength = maxLength;
        }

        public void SetDateConstraints(DateTime? minDate, DateTime? maxDate)
        {
            if (!FieldType.IsDate())
            {
                throw new BusinessException("CustomFields:NotDateField");
            }

            if (minDate.HasValue && maxDate.HasValue && minDate.Value > maxDate.Value)
            {
                throw new BusinessException("CustomFields:MinDateAfterMaxDate");
            }

            MinDate = minDate;
            MaxDate = maxDate;
        }

        public void SetDisplayOrder(int order)
        {
            DisplayOrder = order;
        }

        public void SetDisplayProperties(string placeholder, string helpText, string icon)
        {
            Placeholder = placeholder;
            HelpText = helpText;
            Icon = icon;
        }

        public void SetConditionalDisplay(Guid fieldId, string value)
        {
            ShowIfFieldId = fieldId;
            ShowIfValue = value;
        }

        public void ClearConditionalDisplay()
        {
            ShowIfFieldId = null;
            ShowIfValue = null;
        }

        public void AddOption(CustomFieldOption option)
        {
            if (!FieldType.SupportsOptions())
            {
                throw new BusinessException("CustomFields:FieldTypeDoesNotSupportOptions");
            }

            if (_options.Any(o => o.Value == option.Value))
            {
                throw new BusinessException("CustomFields:DuplicateOptionValue")
                    .WithData("Value", option.Value);
            }

            _options.Add(option);
        }

        public void RemoveOption(Guid optionId)
        {
            var option = _options.FirstOrDefault(o => o.Id == optionId);
            if (option != null)
            {
                _options.Remove(option);
            }
        }

        public void ReorderOptions(List<Guid> orderedIds)
        {
            var reordered = new List<CustomFieldOption>();
            foreach (var id in orderedIds)
            {
                var option = _options.FirstOrDefault(o => o.Id == id);
                if (option != null)
                {
                    reordered.Add(option);
                }
            }
            
            _options.Clear();
            _options.AddRange(reordered);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            if (IsSystem)
            {
                throw new BusinessException("CustomFields:CannotDeactivateSystemField");
            }
            IsActive = false;
        }

        public void ValidateValue(object value)
        {
            // Required check
            if (IsRequired && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                throw new BusinessException("CustomFields:ValueRequired")
                    .WithData("FieldName", Name);
            }

            if (value == null)
            {
                return;
            }

            // Type-specific validation
            switch (FieldType)
            {
                case CustomFieldType.Number:
                case CustomFieldType.Decimal:
                case CustomFieldType.Currency:
                case CustomFieldType.Percentage:
                    ValidateNumericValue(value);
                    break;

                case CustomFieldType.Text:
                case CustomFieldType.TextArea:
                case CustomFieldType.Email:
                case CustomFieldType.Url:
                    ValidateTextValue(value.ToString());
                    break;

                case CustomFieldType.Select:
                    ValidateSelectValue(value.ToString());
                    break;

                case CustomFieldType.MultiSelect:
                    ValidateMultiSelectValue(value);
                    break;

                case CustomFieldType.Date:
                case CustomFieldType.DateTime:
                    ValidateDateValue(value);
                    break;
            }
        }

        private void ValidateNumericValue(object value)
        {
            if (!decimal.TryParse(value.ToString(), out var numValue))
            {
                throw new BusinessException("CustomFields:InvalidNumericValue");
            }

            if (MinValue.HasValue && numValue < MinValue.Value)
            {
                throw new BusinessException("CustomFields:ValueBelowMinimum")
                    .WithData("MinValue", MinValue.Value);
            }

            if (MaxValue.HasValue && numValue > MaxValue.Value)
            {
                throw new BusinessException("CustomFields:ValueAboveMaximum")
                    .WithData("MaxValue", MaxValue.Value);
            }
        }

        private void ValidateTextValue(string value)
        {
            if (MinLength.HasValue && value.Length < MinLength.Value)
            {
                throw new BusinessException("CustomFields:ValueTooShort")
                    .WithData("MinLength", MinLength.Value);
            }

            if (MaxLength.HasValue && value.Length > MaxLength.Value)
            {
                throw new BusinessException("CustomFields:ValueTooLong")
                    .WithData("MaxLength", MaxLength.Value);
            }

            if (!string.IsNullOrEmpty(ValidationRegex))
            {
                var regex = new Regex(ValidationRegex);
                if (!regex.IsMatch(value))
                {
                    throw new BusinessException("CustomFields:ValidationFailed")
                        .WithData("Message", ValidationMessage ?? "Invalid format");
                }
            }
        }

        private void ValidateSelectValue(string value)
        {
            if (!_options.Any(o => o.Value == value))
            {
                throw new BusinessException("CustomFields:InvalidSelectValue");
            }
        }

        private void ValidateMultiSelectValue(object value)
        {
            if (value is not IEnumerable<string> values)
            {
                throw new BusinessException("CustomFields:InvalidMultiSelectValue");
            }

            var validValues = _options.Select(o => o.Value).ToHashSet();
            var invalidValues = values.Where(v => !validValues.Contains(v)).ToList();

            if (invalidValues.Any())
            {
                throw new BusinessException("CustomFields:InvalidMultiSelectValues")
                    .WithData("InvalidValues", string.Join(", ", invalidValues));
            }
        }

        private void ValidateDateValue(object value)
        {
            if (!DateTime.TryParse(value.ToString(), out var dateValue))
            {
                throw new BusinessException("CustomFields:InvalidDateValue");
            }

            if (MinDate.HasValue && dateValue < MinDate.Value)
            {
                throw new BusinessException("CustomFields:DateBeforeMinimum");
            }

            if (MaxDate.HasValue && dateValue > MaxDate.Value)
            {
                throw new BusinessException("CustomFields:DateAfterMaximum");
            }
        }

        private static string GenerateFieldKey(string name)
        {
            return Regex.Replace(name.ToLowerInvariant(), @"[^a-z0-9]+", "_")
                       .Trim('_');
        }
    }
}
