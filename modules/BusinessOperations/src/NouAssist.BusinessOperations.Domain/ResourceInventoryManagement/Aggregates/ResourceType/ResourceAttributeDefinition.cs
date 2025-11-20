using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType
{
    /// <summary>
    /// Defines an attribute for a resource type
    /// </summary>
    public class ResourceAttributeDefinition : Entity
    {
        /// <summary>
        /// ID of the resource type this attribute belongs to
        /// </summary>
        public Guid ResourceTypeId { get; private set; }

        /// <summary>
        /// Name of the attribute
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the attribute
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Data type of the attribute
        /// </summary>
        public AttributeDataType DataType { get; private set; }

        /// <summary>
        /// Whether the attribute is required
        /// </summary>
        public bool IsRequired { get; private set; }

        /// <summary>
        /// Default value for the attribute (if any)
        /// </summary>
        public string DefaultValue { get; private set; }

        /// <summary>
        /// Possible values for enum type attributes (comma-separated)
        /// </summary>
        public string PossibleValues { get; private set; }

        /// <summary>
        /// Display order for UI presentation
        /// </summary>
        public int DisplayOrder { get; private set; }

        protected ResourceAttributeDefinition()
        {
        }

        /// <summary>
        /// Creates a new resource attribute definition
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID</param>
        /// <param name="name">Attribute name</param>
        /// <param name="dataType">Attribute data type</param>
        /// <param name="description">Attribute description (optional)</param>
        /// <param name="isRequired">Whether the attribute is required (optional)</param>
        /// <param name="defaultValue">Default value (optional)</param>
        /// <param name="possibleValues">Possible values for enum type (optional)</param>
        public ResourceAttributeDefinition(
            Guid resourceTypeId,
            string name,
            AttributeDataType dataType,
            string description = null,
            bool isRequired = false,
            string defaultValue = null,
            List<string> possibleValues = null)
        {
            ResourceTypeId = resourceTypeId;
            SetName(name);
            SetDescription(description);
            DataType = dataType;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
            
            // Only set possible values for enum type
            if (dataType == AttributeDataType.Enum && possibleValues != null && possibleValues.Count > 0)
            {
                PossibleValues = string.Join(",", possibleValues);
            }
            
            // Validation for default value based on data type
            ValidateDefaultValue();
        }

        /// <summary>
        /// Updates the attribute definition
        /// </summary>
        /// <param name="description">Updated description</param>
        /// <param name="isRequired">Updated required flag</param>
        /// <param name="defaultValue">Updated default value (optional)</param>
        /// <param name="possibleValues">Updated possible values for enum type (optional)</param>
        public void Update(
            string description,
            bool isRequired,
            string defaultValue = null,
            List<string> possibleValues = null)
        {
            SetDescription(description);
            IsRequired = isRequired;
            DefaultValue = defaultValue;
            
            // Only update possible values for enum type
            if (DataType == AttributeDataType.Enum && possibleValues != null && possibleValues.Count > 0)
            {
                PossibleValues = string.Join(",", possibleValues);
            }
            
            // Validation for default value based on data type
            ValidateDefaultValue();
        }

        /// <summary>
        /// Updates the display order
        /// </summary>
        /// <param name="displayOrder">New display order</param>
        public void UpdateDisplayOrder(int displayOrder)
        {
            DisplayOrder = displayOrder;
        }

        /// <summary>
        /// Gets the possible values as a list (for enum type)
        /// </summary>
        /// <returns>List of possible values</returns>
        public List<string> GetPossibleValuesList()
        {
            if (string.IsNullOrEmpty(PossibleValues))
            {
                return new List<string>();
            }

            return new List<string>(PossibleValues.Split(','));
        }

        /// <summary>
        /// Validates a value against this attribute definition
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>True if the value is valid</returns>
        /// <exception cref="RIMDomainException">Thrown if the value is invalid</exception>
        public bool ValidateValue(string value)
        {
            // Check required constraint
            if (IsRequired && string.IsNullOrEmpty(value))
            {
                throw new RIMDomainException("RIM:ErrorMessage:RequiredAttributeEmpty")
                    .WithData("attributeName", Name);
            }

            // If null/empty and not required, it's valid
            if (string.IsNullOrEmpty(value) && !IsRequired)
            {
                return true;
            }

            // Validate against data type
            switch (DataType)
            {
                case AttributeDataType.Text:
                    // All strings are valid
                    return true;

                case AttributeDataType.Number:
                    if (!decimal.TryParse(value, out _))
                    {
                        throw new RIMDomainException("RIM:ErrorMessage:InvalidAttributeValue")
                            .WithData("attributeName", Name)
                            .WithData("value", value)
                            .WithData("dataType", DataType.ToString());
                    }
                    return true;

                case AttributeDataType.Boolean:
                    if (!bool.TryParse(value, out _))
                    {
                        throw new RIMDomainException("RIM:ErrorMessage:InvalidAttributeValue")
                            .WithData("attributeName", Name)
                            .WithData("value", value)
                            .WithData("dataType", DataType.ToString());
                    }
                    return true;

                case AttributeDataType.Date:
                case AttributeDataType.DateTime:
                    if (!DateTime.TryParse(value, out _))
                    {
                        throw new RIMDomainException("RIM:ErrorMessage:InvalidAttributeValue")
                            .WithData("attributeName", Name)
                            .WithData("value", value)
                            .WithData("dataType", DataType.ToString());
                    }
                    return true;

                case AttributeDataType.Enum:
                    var possibleValues = GetPossibleValuesList();
                    if (possibleValues.Count > 0 && !possibleValues.Contains(value))
                    {
                        throw new RIMDomainException("RIM:ErrorMessage:InvalidEnumValue")
                            .WithData("attributeName", Name)
                            .WithData("value", value)
                            .WithData("allowedValues", PossibleValues);
                    }
                    return true;

                case AttributeDataType.Json:
                    try
                    {
                        System.Text.Json.JsonDocument.Parse(value);
                        return true;
                    }
                    catch (System.Text.Json.JsonException)
                    {
                        throw new RIMDomainException("RIM:ErrorMessage:InvalidJsonValue")
                            .WithData("attributeName", Name)
                            .WithData("value", value);
                    }

                default:
                    throw new RIMDomainException("RIM:ErrorMessage:UnsupportedDataType")
                        .WithData("dataType", DataType.ToString());
            }
        }

        private void ValidateDefaultValue()
        {
            if (!string.IsNullOrEmpty(DefaultValue))
            {
                ValidateValue(DefaultValue);
            }
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: Constants.RIMConstants.Attributes.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: Constants.RIMConstants.Attributes.MaxDescriptionLength
            );
        }

        /// <summary>
        /// Gets the entity's keys for comparison
        /// </summary>
        /// <returns>Array of key values</returns>
        public override object[] GetKeys()
        {
            return new object[] { ResourceTypeId, Name };
        }
    }
}
