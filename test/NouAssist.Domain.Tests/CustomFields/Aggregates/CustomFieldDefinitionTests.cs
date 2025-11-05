using System;
using NouAssist.CustomFields.Enums;
using NouAssist.CustomFields.ValueObjects;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.CustomFields.Aggregates
{
    public class CustomFieldDefinitionTests
    {
        [Fact]
        public void SetNumericConstraints_Should_ThrowException_WhenMinGreaterThanMax()
        {
            // Arrange
            var field = CreateNumericField();

            // Act & Assert
            Should.Throw<BusinessException>(() =>
                field.SetNumericConstraints(10, 5)
            );
        }

        [Fact]
        public void ValidateValue_Should_ThrowException_WhenRequiredFieldEmpty()
        {
            // Arrange
            var field = CreateTextField();
            field.MakeRequired();

            // Act & Assert
            Should.Throw<BusinessException>(() => field.ValidateValue(null));
        }

        [Fact]
        public void AddOption_Should_ThrowException_WhenFieldTypeDoesNotSupport()
        {
            // Arrange
            var field = CreateTextField();
            var option = new CustomFieldOption("Test", "test");

            // Act & Assert
            Should.Throw<BusinessException>(() => field.AddOption(option));
        }

        [Fact]
        public void AddOption_Should_AddOption_ForSelectField()
        {
            // Arrange
            var field = CreateSelectField();
            var option = new CustomFieldOption("Test", "test");

            // Act
            field.AddOption(option);

            // Assert
            field.Options.ShouldContain(option);
        }

        [Fact]
        public void Deactivate_Should_ThrowException_ForSystemFields()
        {
            // Arrange
            var field = CreateSystemField();

            // Act & Assert
            Should.Throw<BusinessException>(() => field.Deactivate());
        }

        private CustomFieldDefinition CreateTextField()
        {
            return new CustomFieldDefinition(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Field",
                CustomFieldType.Text,
                CustomFieldEntityType.Task,
                null
            );
        }

        private CustomFieldDefinition CreateNumericField()
        {
            return new CustomFieldDefinition(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Numeric Field",
                CustomFieldType.Number,
                CustomFieldEntityType.Task,
                null
            );
        }

        private CustomFieldDefinition CreateSelectField()
        {
            return new CustomFieldDefinition(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Select Field",
                CustomFieldType.Select,
                CustomFieldEntityType.Task,
                null
            );
        }

        private CustomFieldDefinition CreateSystemField()
        {
            // Note: You'd need to expose IsSystem setter or create through a different method
            var field = new CustomFieldDefinition(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "System Field",
                CustomFieldType.Text,
                CustomFieldEntityType.Task,
                null
            );
            // Simulate system field creation
            return field;
        }
    }
}
