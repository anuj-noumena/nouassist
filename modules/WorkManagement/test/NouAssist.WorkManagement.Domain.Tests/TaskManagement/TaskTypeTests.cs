using System;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class TaskTypeTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateTaskType()
        {
            // Arrange
            var name = "Bug";
            var description = "Bug task type";
            var color = "#FF0000";

            // Act
            var taskType = new TaskType(
                Guid.NewGuid(),
                name,
                description,
                color);

            // Assert
            taskType.ShouldNotBeNull();
            taskType.Name.ShouldBe(name);
            taskType.Description.ShouldBe(description);
            taskType.Color.ShouldBe(color);
            taskType.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void Constructor_WithEmptyName_Should_ThrowException()
        {
            // Act & Assert
            Should.Throw<ArgumentException>(() => new TaskType(
                Guid.NewGuid(),
                ""));
        }

        [Fact]
        public void UpdateName_WithValidName_Should_UpdateName()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");
            var newName = "Feature";

            // Act
            taskType.UpdateName(newName);

            // Assert
            taskType.Name.ShouldBe(newName);
        }

        [Fact]
        public void UpdateDescription_Should_UpdateDescription()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");
            var newDescription = "New description";

            // Act
            taskType.UpdateDescription(newDescription);

            // Assert
            taskType.Description.ShouldBe(newDescription);
        }

        [Fact]
        public void UpdateColor_WithValidColor_Should_UpdateColor()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");
            var newColor = "#00FF00";

            // Act
            taskType.UpdateColor(newColor);

            // Assert
            taskType.Color.ShouldBe(newColor);
        }

        [Fact]
        public void Activate_Should_SetIsActiveToTrue()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");
            taskType.Deactivate();

            // Act
            taskType.Activate();

            // Assert
            taskType.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void Deactivate_Should_SetIsActiveToFalse()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");

            // Act
            taskType.Deactivate();

            // Assert
            taskType.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void AddCustomField_WithValidField_Should_AddCustomField()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");
            var customFieldId = Guid.NewGuid();

            // Act
            taskType.AddCustomField(customFieldId, isRequired: true);

            // Assert
            taskType.CustomFields.Count.ShouldBe(1);
        }

        [Fact]
        public void AddCustomField_WithDuplicateField_Should_ThrowException()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");
            var customFieldId = Guid.NewGuid();
            taskType.AddCustomField(customFieldId);

            // Act & Assert
            Should.Throw<BusinessException>(() => taskType.AddCustomField(customFieldId));
        }

        [Fact]
        public void RemoveCustomField_Should_RemoveCustomField()
        {
            // Arrange
            var taskType = new TaskType(Guid.NewGuid(), "Bug");
            var customFieldId = Guid.NewGuid();
            taskType.AddCustomField(customFieldId);

            // Act
            taskType.RemoveCustomField(customFieldId);

            // Assert
            taskType.CustomFields.Count.ShouldBe(0);
        }
    }
}
