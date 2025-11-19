using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Approval.Workflows;
using NouAssist.WorkManagement.Approval.Workflows.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Approval
{
    public class ApprovalWorkflowAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IApprovalWorkflowAppService _approvalWorkflowAppService;

        public ApprovalWorkflowAppServiceTests()
        {
            _approvalWorkflowAppService = GetRequiredService<IApprovalWorkflowAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateApprovalWorkflow()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateApprovalWorkflowDto
                {
                    Name = "Standard Approval Workflow"
                };

                var result = await _approvalWorkflowAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Name.ShouldBe("Standard Approval Workflow");
            });
        }
    }
}
