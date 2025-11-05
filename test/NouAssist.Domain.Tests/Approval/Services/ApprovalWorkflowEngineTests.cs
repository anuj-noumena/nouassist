using System;
using System.Linq;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Entities;
using NouAssist.Approval.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.Approval.Services
{
    public class ApprovalWorkflowEngineTests
    {
        private readonly ApprovalWorkflowEngine _engine;

        public ApprovalWorkflowEngineTests()
        {
            _engine = new ApprovalWorkflowEngine();
        }

        [Fact]
        public void ProcessDecision_Should_MoveToNextStep_WhenStepApproved()
        {
            // Arrange
            var workflow = CreateSimpleWorkflow();
            var request = CreateRequest(workflow.Id);
            var firstStep = workflow.Steps.First();
            request.MoveToStep(firstStep.Id);

            var decision = new ApprovalDecision(
                Guid.NewGuid(),
                request.Id,
                firstStep.Id,
                Guid.NewGuid(),
                ApprovalAction.Approved
            );

            // Act
            var result = _engine.ProcessDecision(request, workflow, decision, Guid.NewGuid());

            // Assert
            result.IsStepComplete.ShouldBeTrue();
            result.IsFinalDecision.ShouldBeFalse();
            result.NextStepId.ShouldNotBeNull();
        }

        [Fact]
        public void ProcessDecision_Should_RejectRequest_WhenStepRejected()
        {
            // Arrange
            var workflow = CreateSimpleWorkflow();
            var request = CreateRequest(workflow.Id);
            var firstStep = workflow.Steps.First();
            request.MoveToStep(firstStep.Id);

            var decision = new ApprovalDecision(
                Guid.NewGuid(),
                request.Id,
                firstStep.Id,
                Guid.NewGuid(),
                ApprovalAction.Rejected
            );

            // Act
            var result = _engine.ProcessDecision(request, workflow, decision, Guid.NewGuid());

            // Assert
            result.IsFinalDecision.ShouldBeTrue();
            result.IsApproved.ShouldBeFalse();
            request.IsRejected.ShouldBeTrue();
        }

        [Fact]
        public void ProcessDecision_Should_CompleteWorkflow_WhenLastStepApproved()
        {
            // Arrange
            var workflow = CreateSingleStepWorkflow();
            var request = CreateRequest(workflow.Id);
            var step = workflow.Steps.First();
            request.MoveToStep(step.Id);

            var decision = new ApprovalDecision(
                Guid.NewGuid(),
                request.Id,
                step.Id,
                Guid.NewGuid(),
                ApprovalAction.Approved
            );

            // Act
            var result = _engine.ProcessDecision(request, workflow, decision, Guid.NewGuid());

            // Assert
            result.IsFinalDecision.ShouldBeTrue();
            result.IsApproved.ShouldBeTrue();
            request.IsApproved.ShouldBeTrue();
        }

        [Fact]
        public void InitiateApprovalRequest_Should_CreateRequest_WithFirstStep()
        {
            // Arrange
            var workflow = CreateSimpleWorkflow();

            // Act
            var request = _engine.InitiateApprovalRequest(
                workflow,
                "Task",
                Guid.NewGuid(),
                Guid.NewGuid(),
                null
            );

            // Assert
            request.ShouldNotBeNull();
            request.CurrentStepId.ShouldNotBeNull();
            request.Status.ShouldBe(ApprovalStatus.InProgress);
        }

        #region Test Helpers

        private ApprovalWorkflow CreateSimpleWorkflow()
        {
            var workflow = new ApprovalWorkflow(
                Guid.NewGuid(),
                "Test Workflow",
                ApprovalWorkflowType.TaskApproval,
                ApprovalStrategy.Sequential,
                null
            );

            var step1 = workflow.AddStep("Step 1", 1, ApprovalRequirement.Any);
            step1.AddApproverUser(Guid.NewGuid());

            var step2 = workflow.AddStep("Step 2", 2, ApprovalRequirement.Any);
            step2.AddApproverUser(Guid.NewGuid());

            workflow.Activate();
            return workflow;
        }

        private ApprovalWorkflow CreateSingleStepWorkflow()
        {
            var workflow = new ApprovalWorkflow(
                Guid.NewGuid(),
                "Single Step Workflow",
                ApprovalWorkflowType.TaskApproval,
                ApprovalStrategy.Sequential,
                null
            );

            var step = workflow.AddStep("Only Step", 1, ApprovalRequirement.Any);
            step.AddApproverUser(Guid.NewGuid());

            workflow.Activate();
            return workflow;
        }

        private ApprovalRequest CreateRequest(Guid workflowId)
        {
            return new ApprovalRequest(
                Guid.NewGuid(),
                workflowId,
                "Task",
                Guid.NewGuid(),
                Guid.NewGuid(),
                null
            );
        }

        #endregion
    }
}
