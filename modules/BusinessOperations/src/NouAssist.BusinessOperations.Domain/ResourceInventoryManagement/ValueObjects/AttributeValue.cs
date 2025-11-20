using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects
{
    /// <summary>
    /// Represents a value for a resource attribute with type-specific validation
    /// </summary>
    public class AttributeValue : ValueObject
    {
        /// <summary>
        /// The name of the attribute
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The data type of the attribute
        /// </summary>
        public AttributeDataType DataType { get; private set; }

        /// <summary>
        /// The string representation of the value
        /// </summary>
        public string StringValue { get; private set; }

        private AttributeValue()
        {
        }

        /// <summary>
        /// Creates a new attribute value
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="dataType">Attribute data type</param>
        /// <param name="value">Attribute value</param>
        public AttributeValue(string name, AttributeDataType dataType, object value)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            DataType = dataType;
            StringValue = ConvertAndValidateValue(value);
        }

        /// <summary>
        /// Gets the typed value of the attribute
        /// </summary>
        /// <returns>The typed value</returns>
        public object GetTypedValue()
        {
            return DataType switch
            {
                AttributeDataType.Text => StringValue,
                AttributeDataType.Number => decimal.Parse(StringValue, CultureInfo.InvariantCulture),
                AttributeDataType.Boolean => bool.Parse(StringValue),
                AttributeDataType.Date => DateTime.Parse(StringValue, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal).Date,
                AttributeDataType.DateTime => DateTime.Parse(StringValue, CultureInfo.InvariantCulture),
                AttributeDataType.Enum => StringValue,
                AttributeDataType.Json => JsonSerializer.Deserialize<object>(StringValue),
                _ => throw new ArgumentException($"Unsupported data type: {DataType}")
            };
        }

        private string ConvertAndValidateValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                return DataType switch
                {
                    AttributeDataType.Text => value.ToString(),
                    AttributeDataType.Number => Convert.ToDecimal(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture),
                    AttributeDataType.Boolean => Convert.ToBoolean(value).ToString(),
                    AttributeDataType.Date => Convert.ToDateTime(value, CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    AttributeDataType.DateTime => Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                    AttributeDataType.Enum => value.ToString(),
                    AttributeDataType.Json => value is string stringValue ? stringValue : JsonSerializer.Serialize(value),
                    _ => throw new ArgumentException($"Unsupported data type: {DataType}")
                };
            }
            catch (Exception ex)
            {
                throw new RIMDomainException("RIM:ErrorMessage:InvalidAttributeValue")
                    .WithData("attributeName", Name)
                    .WithData("value", value?.ToString() ?? "null")
                    .WithData("dataType", DataType.ToString())
                    .WithData("details", ex.Message);
            }
        }

        /// <summary>
        /// Gets the atomic values for value comparison
        /// </summary>
        /// <returns>Collection of atomic values</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return DataType;
            yield return StringValue;
        }
    }
}
