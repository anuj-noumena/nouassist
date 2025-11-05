using System;
using NouAssist.Approval.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Approval.Aggregates
{
    public class ApprovalWorkflowTests
    {
        [Fact]
        public void AddStep_Should_CreateStep_WithValidData()
        {
            // Arrange
            var workflow = CreateWorkflow();

            // Act
            var step = workflow.AddStep("Test Step", 1, ApprovalRequirement.Any);

            // Assert
            step.ShouldNotBeNull();
            workflow.Steps.ShouldContain(step);
        }

        [Fact]
        public void AddStep_Should_ThrowException_WhenDuplicateOrderIndex()
        {
            // Arrange
            var workflow = CreateWorkflow();
            workflow.AddStep("Step 1", 1, ApprovalRequirement.Any);

            // Act & Assert
            Should.Throw<BusinessException>(() =>
                workflow.AddStep("Step 2", 1, ApprovalRequirement.Any)
            );
        }

        [Fact]
        public void Activate_Should_ThrowException_WhenNoSteps()
        {
            // Arrange
            var workflow = CreateWorkflow();

            // Act & Assert
            Should.Throw<BusinessException>(() => workflow.Activate());
        }

        [Fact]
        public void GetNextStep_Should_ReturnCorrectStep()
        {
            // Arrange
            var workflow = CreateWorkflow();
            var step1 = workflow.AddStep("Step 1", 1, ApprovalRequirement.Any);
            var step2 = workflow.AddStep("Step 2", 2, ApprovalRequirement.Any);

            // Act
            var nextStep = workflow.GetNextStep(step1.Id);

            // Assert
            nextStep.ShouldBe(step2);
        }

        [Fact]
        public void IsLastStep_Should_ReturnTrue_ForLastStep()
        {
            // Arrange
            var workflow = CreateWorkflow();
            workflow.AddStep("Step 1", 1, ApprovalRequirement.Any);
            var lastStep = workflow.AddStep("Step 2", 2, ApprovalRequirement.Any);

            // Act
            var isLast = workflow.IsLastStep(lastStep.Id);

            // Assert
            isLast.ShouldBeTrue();
        }

        private ApprovalWorkflow CreateWorkflow()
        {
            return new ApprovalWorkflow(
                Guid.NewGuid(),
                "Test Workflow",
                ApprovalWorkflowType.TaskApproval,
                ApprovalStrategy.Sequential,
                null
            );
        }
    }
}
