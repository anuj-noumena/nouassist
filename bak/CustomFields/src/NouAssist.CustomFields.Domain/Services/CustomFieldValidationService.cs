using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.CustomFields.Aggregates;
using NouAssist.CustomFields.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.CustomFields.Services
{
    public class CustomFieldValidationService : DomainService
    {
        public CustomFieldValidationResult ValidateCustomFields(
            Dictionary<Guid, object> fieldValues,
            List<CustomFieldDefinition> definitions
        )
        {
            var result = new CustomFieldValidationResult();

            // Check all required fields are present
            var requiredFields = definitions.Where(d => d.IsRequired && d.IsActive).ToList();
            foreach (var field in requiredFields)
            {
                if (!fieldValues.ContainsKey(field.Id) || fieldValues[field.Id] == null)
                {
                    result.AddError(field.Id, field.Name, "This field is required");
                }
            }

            // Validate each provided field
            foreach (var kvp in fieldValues)
            {
                var definition = definitions.FirstOrDefault(d => d.Id == kvp.Key);
                if (definition == null)
                {
                    result.AddError(kvp.Key, "Unknown", "Field definition not found");
                    continue;
                }

                if (!definition.IsActive)
                {
                    result.AddWarning(kvp.Key, definition.Name, "Field is inactive");
                    continue;
                }

                try
                {
                    definition.ValidateValue(kvp.Value);
                }
                catch (BusinessException ex)
                {
                    result.AddError(kvp.Key, definition.Name, ex.Message);
                }
            }

            result.IsValid = !result.HasErrors;
            return result;
        }

        public bool ShouldDisplayField(
            CustomFieldDefinition field,
            Dictionary<Guid, object> currentValues
        )
        {
            if (!field.ShowIfFieldId.HasValue)
            {
                return true;
            }

            if (!currentValues.TryGetValue(field.ShowIfFieldId.Value, out var conditionalValue))
            {
                return false;
            }

            return conditionalValue?.ToString() == field.ShowIfValue;
        }

        public List<CustomFieldDefinition> GetVisibleFields(
            List<CustomFieldDefinition> allFields,
            Dictionary<Guid, object> currentValues
        )
        {
            return allFields
                .Where(f => f.IsActive && ShouldDisplayField(f, currentValues))
                .OrderBy(f => f.DisplayOrder)
                .ToList();
        }

        public object CoerceValueToType(object value, CustomFieldType fieldType)
        {
            if (value == null) return null;

            try
            {
                return fieldType switch
                {
                    CustomFieldType.Number => Convert.ToInt32(value),
                    CustomFieldType.Decimal => Convert.ToDecimal(value),
                    CustomFieldType.Currency => Convert.ToDecimal(value),
                    CustomFieldType.Percentage => Convert.ToDecimal(value),
                    CustomFieldType.Boolean => Convert.ToBoolean(value),
                    CustomFieldType.Date => DateTime.Parse(value.ToString()).Date,
                    CustomFieldType.DateTime => DateTime.Parse(value.ToString()),
                    _ => value
                };
            }
            catch
            {
                throw new BusinessException("CustomFields:TypeConversionFailed")
                    .WithData("FieldType", fieldType)
                    .WithData("Value", value);
            }
        }
    }

    public class CustomFieldValidationResult
    {
        public bool IsValid { get; set; }
        public List<FieldValidationError> Errors { get; private set; } = new();
        public List<FieldValidationError> Warnings { get; private set; } = new();

        public void AddError(Guid fieldId, string fieldName, string message)
        {
            Errors.Add(new FieldValidationError
            {
                FieldId = fieldId,
                FieldName = fieldName,
                Message = message
            });
        }

        public void AddWarning(Guid fieldId, string fieldName, string message)
        {
            Warnings.Add(new FieldValidationError
            {
                FieldId = fieldId,
                FieldName = fieldName,
                Message = message
            });
        }

        public bool HasErrors => Errors.Any();
        public bool HasWarnings => Warnings.Any();
    }

    public class FieldValidationError
    {
        public Guid FieldId { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}
