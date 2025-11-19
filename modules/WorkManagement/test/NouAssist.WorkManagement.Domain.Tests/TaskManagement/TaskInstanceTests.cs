using System;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class TaskInstanceTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateTaskInstance()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var workflowId = Guid.NewGuid();
            var workflowStatusId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            // Act
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                taskId,
                workflowId,
                workflowStatusId,
                ownerId,
                null,
                null,
                null);

            // Assert
            taskInstance.ShouldNotBeNull();
            taskInstance.TaskId.ShouldBe(taskId);
            taskInstance.WorkflowId.ShouldBe(workflowId);
            taskInstance.WorkflowStatusId.ShouldBe(workflowStatusId);
            taskInstance.OwnerId.ShouldBe(ownerId);
        }

        [Fact]
        public void AssignTo_Should_UpdateAssigneeId()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);
            var assigneeId = Guid.NewGuid();

            // Act
            taskInstance.AssignTo(assigneeId);

            // Assert
            taskInstance.AssigneeId.ShouldBe(assigneeId);
        }

        [Fact]
        public void TransitionTo_Should_UpdateWorkflowStatus()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);
            var newStatusId = Guid.NewGuid();

            // Act
            taskInstance.TransitionTo(newStatusId);

            // Assert
            taskInstance.WorkflowStatusId.ShouldBe(newStatusId);
        }

        [Fact]
        public void Block_Should_SetBlockedStatus()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);

            // Act
            taskInstance.Block("Waiting for dependencies");

            // Assert
            taskInstance.IsBlocked.ShouldBeTrue();
        }

        [Fact]
        public void Unblock_Should_RemoveBlockedStatus()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);
            taskInstance.Block("Blocked");

            // Act
            taskInstance.Unblock();

            // Assert
            taskInstance.IsBlocked.ShouldBeFalse();
        }

        [Fact]
        public void Flag_Should_SetFlaggedStatus()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);

            // Act
            taskInstance.Flag("Needs attention");

            // Assert
            taskInstance.IsFlagged.ShouldBeTrue();
        }

        [Fact]
        public void Unflag_Should_RemoveFlaggedStatus()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);
            taskInstance.Flag("Flagged");

            // Act
            taskInstance.Unflag();

            // Assert
            taskInstance.IsFlagged.ShouldBeFalse();
        }

        [Fact]
        public void Resolve_Should_SetResolvedStatus()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);

            // Act
            taskInstance.Resolve("Done", "Completed successfully");

            // Assert
            taskInstance.IsResolved.ShouldBeTrue();
        }

        [Fact]
        public void SetAreaId_Should_UpdateAreaId()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);
            var areaId = Guid.NewGuid();

            // Act
            taskInstance.SetAreaId(areaId);

            // Assert
            taskInstance.AreaId.ShouldBe(areaId);
        }

        [Fact]
        public void SetContextId_Should_UpdateContextId()
        {
            // Arrange
            var taskInstance = new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                null,
                null);
            var contextId = Guid.NewGuid();

            // Act
            taskInstance.SetContextId(contextId);

            // Assert
            taskInstance.ContextId.ShouldBe(contextId);
        }
    }
}
