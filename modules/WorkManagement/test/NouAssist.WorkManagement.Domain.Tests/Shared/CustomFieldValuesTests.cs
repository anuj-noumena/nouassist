using System.Collections.Generic;
using NouAssist.WorkManagement.Shared.ValueObjects;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Shared
{
    public class CustomFieldValuesTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Create_WithNoValues_Should_ReturnEmptyCustomFieldValues()
        {
            // Act
            var customFields = CustomFieldValues.Create();

            // Assert
            customFields.ShouldNotBeNull();
            customFields.GetValues().ShouldNotBeNull();
            customFields.GetValues().Count.ShouldBe(0);
        }

        [Fact]
        public void Create_WithValues_Should_ReturnCustomFieldValuesWithValues()
        {
            // Arrange
            var values = new Dictionary<string, object>
            {
                { "field1", "value1" },
                { "field2", 100 }
            };

            // Act
            var customFields = CustomFieldValues.Create(values);

            // Assert
            customFields.ShouldNotBeNull();
            customFields.GetValues().Count.ShouldBe(2);
            customFields.GetValue("field1").ShouldBe("value1");
            customFields.GetValue("field2").ShouldBe(100);
        }

        [Fact]
        public void SetValue_NewField_Should_AddField()
        {
            // Arrange
            var customFields = CustomFieldValues.Create();

            // Act
            var updated = customFields.SetValue("newField", "newValue");

            // Assert
            updated.GetValues().Count.ShouldBe(1);
            updated.GetValue("newField").ShouldBe("newValue");
            // Original should be unchanged (immutability)
            customFields.GetValues().Count.ShouldBe(0);
        }

        [Fact]
        public void SetValue_ExistingField_Should_UpdateField()
        {
            // Arrange
            var customFields = CustomFieldValues.Create(new Dictionary<string, object>
            {
                { "field1", "oldValue" }
            });

            // Act
            var updated = customFields.SetValue("field1", "newValue");

            // Assert
            updated.GetValue("field1").ShouldBe("newValue");
            // Original should be unchanged (immutability)
            customFields.GetValue("field1").ShouldBe("oldValue");
        }

        [Fact]
        public void GetValue_ExistingField_Should_ReturnValue()
        {
            // Arrange
            var customFields = CustomFieldValues.Create(new Dictionary<string, object>
            {
                { "field1", "value1" }
            });

            // Act
            var value = customFields.GetValue("field1");

            // Assert
            value.ShouldBe("value1");
        }

        [Fact]
        public void GetValue_NonExistingField_Should_ReturnNull()
        {
            // Arrange
            var customFields = CustomFieldValues.Create();

            // Act
            var value = customFields.GetValue("nonExistingField");

            // Assert
            value.ShouldBeNull();
        }

        [Fact]
        public void HasValue_ExistingField_Should_ReturnTrue()
        {
            // Arrange
            var customFields = CustomFieldValues.Create(new Dictionary<string, object>
            {
                { "field1", "value1" }
            });

            // Act & Assert
            customFields.HasValue("field1").ShouldBeTrue();
        }

        [Fact]
        public void HasValue_NonExistingField_Should_ReturnFalse()
        {
            // Arrange
            var customFields = CustomFieldValues.Create();

            // Act & Assert
            customFields.HasValue("nonExistingField").ShouldBeFalse();
        }

        [Fact]
        public void GetValues_Should_ReturnCopyOfDictionary()
        {
            // Arrange
            var customFields = CustomFieldValues.Create(new Dictionary<string, object>
            {
                { "field1", "value1" }
            });

            // Act
            var values = customFields.GetValues();
            values["field1"] = "modified";

            // Assert
            customFields.GetValue("field1").ShouldBe("value1"); // Original unchanged
        }

        [Fact]
        public void SetValue_WithDifferentTypes_Should_HandleAllTypes()
        {
            // Arrange
            var customFields = CustomFieldValues.Create();

            // Act
            var updated = customFields
                .SetValue("stringField", "text")
                .SetValue("intField", 42)
                .SetValue("boolField", true)
                .SetValue("doubleField", 3.14);

            // Assert
            updated.GetValue("stringField").ShouldBe("text");
            updated.GetValue("intField").ShouldBe(42);
            updated.GetValue("boolField").ShouldBe(true);
            updated.GetValue("doubleField").ShouldBe(3.14);
        }

        [Fact]
        public void Equality_WithSameValues_Should_BeEqual()
        {
            // Arrange
            var customFields1 = CustomFieldValues.Create(new Dictionary<string, object>
            {
                { "field1", "value1" }
            });
            var customFields2 = CustomFieldValues.Create(new Dictionary<string, object>
            {
                { "field1", "value1" }
            });

            // Act & Assert
            customFields1.Equals(customFields2).ShouldBeTrue();
        }

        [Fact]
        public void Equality_WithDifferentValues_Should_NotBeEqual()
        {
            // Arrange
            var customFields1 = CustomFieldValues.Create(new Dictionary<string, object>
            {
                { "field1", "value1" }
            });
            var customFields2 = CustomFieldValues.Create(new Dictionary<string, object>
            {
                { "field1", "value2" }
            });

            // Act & Assert
            customFields1.Equals(customFields2).ShouldBeFalse();
        }
    }
}
