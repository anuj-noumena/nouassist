using System;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
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
            var workflowId = Guid.NewGuid();
            var workflow = new Aggregates.Workflow(
                workflowId,
                "Test Workflow",
                Guid.NewGuid());

            var statusId = Guid.NewGuid();
            var statusName = "In Progress";
            var status = new WorkflowStatus(statusId, workflowId, statusName, WorkflowStatusCategory.InProgress);

            // Act
            workflow.AddStatus(status);

            // Assert
            workflow.Statuses.Count.ShouldBe(1);
            workflow.Statuses.ShouldContain(s => s.Id == statusId);
        }

        [Fact]
        public void AddTransition_Should_AddTransitionToWorkflow()
        {
            // Arrange
            var workflowId = Guid.NewGuid();
            var workflow = new Aggregates.Workflow(
                workflowId,
                "Test Workflow",
                Guid.NewGuid());

            var fromStatusId = Guid.NewGuid();
            var toStatusId = Guid.NewGuid();

            var openStatus = new WorkflowStatus(fromStatusId, workflowId, "Open", WorkflowStatusCategory.Todo);
            var inProgressStatus = new WorkflowStatus(toStatusId, workflowId, "In Progress", WorkflowStatusCategory.InProgress);

            workflow.AddStatus(openStatus);
            workflow.AddStatus(inProgressStatus);

            var transition = new WorkflowTransition(Guid.NewGuid(), workflowId, "Start Work", fromStatusId, toStatusId);

            // Act
            workflow.AddTransition(transition);

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
            var workflowId = Guid.NewGuid();
            var workflow = new Aggregates.Workflow(
                workflowId,
                "Test Workflow",
                Guid.NewGuid());

            var statusId = Guid.NewGuid();
            var status = new WorkflowStatus(statusId, workflowId, "In Progress", WorkflowStatusCategory.InProgress);
            workflow.AddStatus(status);

            // Act
            workflow.RemoveStatus(statusId);

            // Assert
            workflow.Statuses.Count.ShouldBe(0);
        }
    }
}
