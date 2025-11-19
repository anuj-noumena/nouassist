using System;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Approval
{
    public class ApprovalRequestTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateApprovalRequest()
        {
            // Arrange
            var workflowId = Guid.NewGuid();
            var entityType = "Task";
            var entityId = Guid.NewGuid();
            var requestedBy = Guid.NewGuid();
            var title = "Test Approval Request";

            // Act
            var approvalRequest = new ApprovalRequest(
                Guid.NewGuid(),
                workflowId,
                entityType,
                entityId,
                requestedBy,
                title);

            // Assert
            approvalRequest.ShouldNotBeNull();
            approvalRequest.WorkflowId.ShouldBe(workflowId);
            approvalRequest.EntityType.ShouldBe(entityType);
            approvalRequest.EntityId.ShouldBe(entityId);
            approvalRequest.RequestedBy.ShouldBe(requestedBy);
            approvalRequest.Title.ShouldBe(title);
            approvalRequest.Status.ShouldBe(ApprovalStatus.Pending);
        }

        [Fact]
        public void Submit_Should_ChangeStatusToInProgress()
        {
            // Arrange
            var approvalRequest = new ApprovalRequest(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Task",
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Request");

            // Act
            approvalRequest.Submit();

            // Assert
            approvalRequest.Status.ShouldBe(ApprovalStatus.InProgress);
            approvalRequest.SubmittedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Approve_Should_ChangeStatusToApproved()
        {
            // Arrange
            var approvalRequest = new ApprovalRequest(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Task",
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Request");
            approvalRequest.Submit();

            // Act
            approvalRequest.Approve(Guid.NewGuid());

            // Assert
            approvalRequest.Status.ShouldBe(ApprovalStatus.Approved);
            approvalRequest.CompletedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Reject_Should_ChangeStatusToRejected()
        {
            // Arrange
            var approvalRequest = new ApprovalRequest(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Task",
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Request");
            approvalRequest.Submit();

            // Act
            approvalRequest.Reject(Guid.NewGuid(), "Does not meet requirements");

            // Assert
            approvalRequest.Status.ShouldBe(ApprovalStatus.Rejected);
            approvalRequest.CompletedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Cancel_Should_ChangeStatusToCancelled()
        {
            // Arrange
            var approvalRequest = new ApprovalRequest(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Task",
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Request");
            var cancelledBy = Guid.NewGuid();

            // Act
            approvalRequest.Cancel(cancelledBy, "No longer needed");

            // Assert
            approvalRequest.Status.ShouldBe(ApprovalStatus.Cancelled);
            approvalRequest.CancelledBy.ShouldBe(cancelledBy);
            approvalRequest.CancelledAt.ShouldNotBeNull();
        }

        [Fact]
        public void SetCurrentStep_Should_UpdateCurrentStepId()
        {
            // Arrange
            var approvalRequest = new ApprovalRequest(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Task",
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Request");
            var stepId = Guid.NewGuid();

            // Act
            approvalRequest.SetCurrentStep(stepId);

            // Assert
            approvalRequest.CurrentStepId.ShouldBe(stepId);
        }
    }
}
