using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Approval.ApprovalRequests;
using NouAssist.WorkManagement.Approval.ApprovalRequests.Dtos;
using Shouldly;
using Volo.Abp.Validation;
using Xunit;

namespace NouAssist.WorkManagement.Approval
{
    public class ApprovalRequestAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IApprovalRequestAppService _approvalRequestAppService;

        public ApprovalRequestAppServiceTests()
        {
            _approvalRequestAppService = GetRequiredService<IApprovalRequestAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateApprovalRequest()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateApprovalRequestDto
                {
                    WorkflowId = Guid.NewGuid(),
                    EntityType = "Task",
                    EntityId = Guid.NewGuid(),
                    Title = "Test Approval Request",
                    Description = "Test Description"
                };

                // Act
                var result = await _approvalRequestAppService.CreateAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Id.ShouldNotBe(Guid.Empty);
                result.Title.ShouldBe(input.Title);
                result.Description.ShouldBe(input.Description);
            });
        }

        [Fact]
        public async Task CreateAsync_WithEmptyTitle_Should_ThrowValidationException()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateApprovalRequestDto
                {
                    WorkflowId = Guid.NewGuid(),
                    EntityType = "Task",
                    EntityId = Guid.NewGuid(),
                    Title = "" // Empty title
                };

                // Act & Assert
                await Should.ThrowAsync<AbpValidationException>(async () =>
                    await _approvalRequestAppService.CreateAsync(input));
            });
        }

        [Fact]
        public async Task GetAsync_WithValidId_Should_ReturnApprovalRequest()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateApprovalRequestDto
                {
                    WorkflowId = Guid.NewGuid(),
                    EntityType = "Task",
                    EntityId = Guid.NewGuid(),
                    Title = "Test Approval Request"
                };

                var createdRequest = await _approvalRequestAppService.CreateAsync(createInput);
                var retrievedRequest = await _approvalRequestAppService.GetAsync(createdRequest.Id);

                // Assert
                retrievedRequest.ShouldNotBeNull();
                retrievedRequest.Id.ShouldBe(createdRequest.Id);
                retrievedRequest.Title.ShouldBe(createdRequest.Title);
            });
        }

        [Fact]
        public async Task ApproveAsync_Should_ApproveRequest()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateApprovalRequestDto
                {
                    WorkflowId = Guid.NewGuid(),
                    EntityType = "Task",
                    EntityId = Guid.NewGuid(),
                    Title = "Test Approval Request"
                };

                var createdRequest = await _approvalRequestAppService.CreateAsync(createInput);
                await _approvalRequestAppService.SubmitAsync(createdRequest.Id);

                var approvedRequest = await _approvalRequestAppService.ApproveAsync(createdRequest.Id, new ApproveApprovalRequestDto());

                // Assert
                approvedRequest.ShouldNotBeNull();
                approvedRequest.Status.ShouldBe(Approval.Enums.ApprovalStatus.Approved);
            });
        }

        [Fact]
        public async Task RejectAsync_Should_RejectRequest()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateApprovalRequestDto
                {
                    WorkflowId = Guid.NewGuid(),
                    EntityType = "Task",
                    EntityId = Guid.NewGuid(),
                    Title = "Test Approval Request"
                };

                var createdRequest = await _approvalRequestAppService.CreateAsync(createInput);
                await _approvalRequestAppService.SubmitAsync(createdRequest.Id);

                var rejectInput = new RejectApprovalRequestDto
                {
                    Reason = "Does not meet requirements"
                };
                var rejectedRequest = await _approvalRequestAppService.RejectAsync(createdRequest.Id, rejectInput);

                // Assert
                rejectedRequest.ShouldNotBeNull();
                rejectedRequest.Status.ShouldBe(Approval.Enums.ApprovalStatus.Rejected);
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnPagedResults()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                // Create test approval requests
                var workflowId = Guid.NewGuid();
                await _approvalRequestAppService.CreateAsync(new CreateApprovalRequestDto
                {
                    WorkflowId = workflowId,
                    EntityType = "Task",
                    EntityId = Guid.NewGuid(),
                    Title = "Request 1"
                });
                await _approvalRequestAppService.CreateAsync(new CreateApprovalRequestDto
                {
                    WorkflowId = workflowId,
                    EntityType = "Task",
                    EntityId = Guid.NewGuid(),
                    Title = "Request 2"
                });

                var input = new GetApprovalRequestListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0
                };

                var result = await _approvalRequestAppService.GetListAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Items.ShouldNotBeEmpty();
                result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}
