using System;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class TaskTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateTask()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Test Task Blueprint";
            var taskTypeId = Guid.NewGuid();

            // Act
            var task = new Task(
                Guid.NewGuid(),
                ownerId,
                title,
                taskTypeId);

            // Assert
            task.ShouldNotBeNull();
            task.OwnerId.ShouldBe(ownerId);
            task.Title.ShouldBe(title);
            task.TaskTypeId.ShouldBe(taskTypeId);
            task.IsActive.ShouldBeTrue();
            task.Version.ShouldBe(1);
        }

        [Fact]
        public void AssignToArea_Should_UpdateAreaId()
        {
            // Arrange
            var task = new Task(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Task",
                Guid.NewGuid());
            var areaId = Guid.NewGuid();

            // Act
            task.AssignToArea(areaId);

            // Assert
            task.AreaId.ShouldBe(areaId);
            task.Version.ShouldBe(2);
        }

        [Fact]
        public void RemoveFromArea_Should_ClearAreaId()
        {
            // Arrange
            var task = new Task(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Task",
                Guid.NewGuid(),
                areaId: Guid.NewGuid());

            // Act
            task.RemoveFromArea();

            // Assert
            task.AreaId.ShouldBeNull();
        }

        [Fact]
        public void SetDefaults_Should_UpdateDefaultPriority()
        {
            // Arrange
            var task = new Task(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Task",
                Guid.NewGuid());

            // Act
            task.SetDefaults(0.8, 0.6);

            // Assert
            task.DefaultImportance.ShouldBe(0.8, tolerance: 0.01);
            task.DefaultUrgency.ShouldBe(0.6, tolerance: 0.01);
        }

        [Fact]
        public void Activate_Should_SetIsActiveToTrue()
        {
            // Arrange
            var task = new Task(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Task",
                Guid.NewGuid());
            task.Deactivate();

            // Act
            task.Activate();

            // Assert
            task.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void Deactivate_Should_SetIsActiveToFalse()
        {
            // Arrange
            var task = new Task(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Task",
                Guid.NewGuid());

            // Act
            task.Deactivate();

            // Assert
            task.IsActive.ShouldBeFalse();
        }
    }
}
