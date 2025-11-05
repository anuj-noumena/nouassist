using System;
using System.Collections.Generic;
using NouAssist.CustomFields.Aggregates;
using NouAssist.CustomFields.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.CustomFields.Services
{
    public class CustomFieldValidationServiceTests
    {
        private readonly CustomFieldValidationService _service;

        public CustomFieldValidationServiceTests()
        {
            _service = new CustomFieldValidationService();
        }

        [Fact]
        public void ValidateCustomFields_Should_Fail_WhenRequiredFieldMissing()
        {
            // Arrange
            var definition = CreateRequiredTextField();
            var definitions = new List<CustomFieldDefinition> { definition };
            var values = new Dictionary<Guid, object>(); // Empty values

            // Act
            var result = _service.ValidateCustomFields(values, definitions);

            // Assert
            result.IsValid.ShouldBeFalse();
            result.HasErrors.ShouldBeTrue();
        }

        [Fact]
        public void ValidateCustomFields_Should_Pass_WhenAllRequiredFieldsPresent()
        {
            // Arrange
            var definition = CreateRequiredTextField();
            var definitions = new List<CustomFieldDefinition> { definition };
            var values = new Dictionary<Guid, object>
            {
                { definition.Id, "Valid Value" }
            };

            // Act
            var result = _service.ValidateCustomFields(values, definitions);

            // Assert
            result.IsValid.ShouldBeTrue();
            result.HasErrors.ShouldBeFalse();
        }

        [Fact]
        public void ValidateCustomFields_Should_Fail_WhenNumericValueOutOfRange()
        {
            // Arrange
            var definition = CreateNumericFieldWithRange(1, 10);
            var definitions = new List<CustomFieldDefinition> { definition };
            var values = new Dictionary<Guid, object>
            {
                { definition.Id, 15 } // Out of range
            };

            // Act
            var result = _service.ValidateCustomFields(values, definitions);

            // Assert
            result.IsValid.ShouldBeFalse();
            result.HasErrors.ShouldBeTrue();
        }

        [Fact]
        public void ShouldDisplayField_Should_ReturnTrue_WhenNoConditions()
        {
            // Arrange
            var field = CreateTextField();
            var values = new Dictionary<Guid, object>();

            // Act
            var result = _service.ShouldDisplayField(field, values);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void ShouldDisplayField_Should_ReturnFalse_WhenConditionNotMet()
        {
            // Arrange
            var dependentField = CreateTextField();
            var field = CreateConditionalField(dependentField.Id, "expected");
            var values = new Dictionary<Guid, object>
            {
                { dependentField.Id, "other_value" }
            };

            // Act
            var result = _service.ShouldDisplayField(field, values);

            // Assert
            result.ShouldBeFalse();
        }

        #region Test Helpers

        private CustomFieldDefinition CreateRequiredTextField()
        {
            var field = new CustomFieldDefinition(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Required Field",
                CustomFieldType.Text,
                CustomFieldEntityType.Task,
                null
            );
            field.MakeRequired();
            return field;
        }

        private CustomFieldDefinition CreateTextField()
        {
            return new CustomFieldDefinition(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Text Field",
                CustomFieldType.Text,
                CustomFieldEntityType.Task,
                null
            );
        }

        private CustomFieldDefinition CreateNumericFieldWithRange(decimal min, decimal max)
        {
            var field = new CustomFieldDefinition(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Numeric Field",
                CustomFieldType.Number,
                CustomFieldEntityType.Task,
                null
            );
            field.SetNumericConstraints(min, max);
            return field;
        }

        private CustomFieldDefinition CreateConditionalField(Guid dependsOnFieldId, string expectedValue)
        {
            var field = CreateTextField();
            field.SetConditionalDisplay(dependsOnFieldId, expectedValue);
            return field;
        }

        #endregion
    }
}
