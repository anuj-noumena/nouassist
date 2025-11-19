using System;
using NouAssist.WorkManagement.Approval.Aggregates;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Approval
{
    public class ApprovalWorkflowTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateApprovalWorkflow()
        {
            // Arrange
            var name = "Standard Approval Workflow";

            // Act
            var workflow = new ApprovalWorkflow(Guid.NewGuid(), name);

            // Assert
            workflow.ShouldNotBeNull();
            workflow.Name.ShouldBe(name);
        }
    }
}
