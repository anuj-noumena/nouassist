using System;
using NouAssist.WorkManagement.Workflow.Aggregates;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Workflow
{
    public class WorkflowTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateWorkflow()
        {
            // Arrange
            var name = "Test Workflow";
            var initialStatusId = Guid.NewGuid();

            // Act
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                name,
                initialStatusId);

            // Assert
            workflow.ShouldNotBeNull();
            workflow.Name.ShouldBe(name);
            workflow.InitialStatusId.ShouldBe(initialStatusId);
            workflow.IsActive.ShouldBeTrue();
            workflow.IsDefault.ShouldBeFalse();
        }

        [Fact]
        public void AddStatus_Should_AddStatusToWorkflow()
        {
            // Arrange
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                "Test Workflow",
                Guid.NewGuid());

            var statusId = Guid.NewGuid();
            var statusName = "In Progress";

            // Act
            workflow.AddStatus(statusId, statusName, 1);

            // Assert
            workflow.Statuses.Count.ShouldBe(1);
            workflow.Statuses.ShouldContain(s => s.StatusId == statusId);
        }

        [Fact]
        public void AddTransition_Should_AddTransitionToWorkflow()
        {
            // Arrange
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                "Test Workflow",
                Guid.NewGuid());

            var fromStatusId = Guid.NewGuid();
            var toStatusId = Guid.NewGuid();

            workflow.AddStatus(fromStatusId, "Open", 1);
            workflow.AddStatus(toStatusId, "In Progress", 2);

            // Act
            workflow.AddTransition(fromStatusId, toStatusId, "Start Work");

            // Assert
            workflow.Transitions.Count.ShouldBe(1);
            workflow.Transitions.ShouldContain(t =>
                t.FromStatusId == fromStatusId && t.ToStatusId == toStatusId);
        }

        [Fact]
        public void Activate_Should_SetIsActiveToTrue()
        {
            // Arrange
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                "Test Workflow",
                Guid.NewGuid());
            workflow.Deactivate();

            // Act
            workflow.Activate();

            // Assert
            workflow.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void Deactivate_Should_SetIsActiveToFalse()
        {
            // Arrange
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                "Test Workflow",
                Guid.NewGuid());

            // Act
            workflow.Deactivate();

            // Assert
            workflow.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void SetAsDefault_Should_SetIsDefaultToTrue()
        {
            // Arrange
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                "Test Workflow",
                Guid.NewGuid());

            // Act
            workflow.SetAsDefault();

            // Assert
            workflow.IsDefault.ShouldBeTrue();
        }

        [Fact]
        public void RemoveStatus_Should_RemoveStatusFromWorkflow()
        {
            // Arrange
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                "Test Workflow",
                Guid.NewGuid());

            var statusId = Guid.NewGuid();
            workflow.AddStatus(statusId, "In Progress", 1);

            // Act
            workflow.RemoveStatus(statusId);

            // Assert
            workflow.Statuses.Count.ShouldBe(0);
        }
    }
}
