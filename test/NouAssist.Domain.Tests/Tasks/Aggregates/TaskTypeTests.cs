using System;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Tasks.Aggregates
{
    public class TaskTypeTests
    {
        [Fact]
        public void Constructor_Should_CreateValidTaskType()
        {
            // Arrange & Act
            var taskType = new TaskType(
                Guid.NewGuid(),
                "Bug",
                "Defect or issue",
                "#FF0000");

            // Assert
            taskType.Name.ShouldBe("Bug");
            taskType.Description.ShouldBe("Defect or issue");
            taskType.Color.ShouldBe("#FF0000");
            taskType.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void AddCustomField_Should_AddFieldToTaskType()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Feature");
            var fieldId = Guid.NewGuid();

            // Act
            taskType.AddCustomField(fieldId, isRequired: true, displayOrder: 1);

            // Assert
            taskType.CustomFields.Count.ShouldBe(1);
            taskType.CustomFields.ShouldContain(cf => cf.CustomFieldDefinitionId == fieldId);
        }

        [Fact]
        public void AddCustomField_Should_ThrowException_WhenFieldAlreadyExists()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Feature");
            var fieldId = Guid.NewGuid();
            taskType.AddCustomField(fieldId);

            // Act & Assert
            Should.Throw<BusinessException>(() => taskType.AddCustomField(fieldId));
        }

        [Fact]
        public void UpdateColor_Should_ValidateHexFormat()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");

            // Act & Assert
            Should.Throw<BusinessException>(() => taskType.UpdateColor("invalid"));
            Should.NotThrow(() => taskType.UpdateColor("#FF0000"));
            Should.NotThrow(() => taskType.UpdateColor("#F00"));
        }

        [Fact]
        public void Deactivate_Should_SetIsActiveFalse()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");

            // Act
            taskType.Deactivate();

            // Assert
            taskType.IsActive.ShouldBeFalse();
        }
    }
}
