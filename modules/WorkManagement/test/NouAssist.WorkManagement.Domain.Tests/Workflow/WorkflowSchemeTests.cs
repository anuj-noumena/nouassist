using System;
using NouAssist.WorkManagement.Workflow.Aggregates;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Workflow
{
    public class WorkflowSchemeTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateWorkflowScheme()
        {
            // Arrange
            var name = "Default Workflow Scheme";

            // Act
            var scheme = new WorkflowScheme(Guid.NewGuid(), name);

            // Assert
            scheme.ShouldNotBeNull();
            scheme.Name.ShouldBe(name);
        }
    }
}
