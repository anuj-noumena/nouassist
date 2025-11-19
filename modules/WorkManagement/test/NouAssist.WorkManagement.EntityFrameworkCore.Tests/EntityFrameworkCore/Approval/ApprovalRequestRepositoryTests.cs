using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Data;
using NouAssist.WorkManagement.Approval.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Approval
{
    public class ApprovalRequestRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly IApprovalRequestRepository _approvalRequestRepository;

        public ApprovalRequestRepositoryTests()
        {
            _approvalRequestRepository = GetRequiredService<IApprovalRequestRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertApprovalRequest()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var approvalRequest = new ApprovalRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Test Approval Request");

                // Act
                var insertedRequest = await _approvalRequestRepository.InsertAsync(approvalRequest);

                // Assert
                insertedRequest.ShouldNotBeNull();
                insertedRequest.Id.ShouldBe(approvalRequest.Id);
                insertedRequest.Title.ShouldBe("Test Approval Request");
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnApprovalRequest()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var approvalRequest = new ApprovalRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Test Approval Request");

                await _approvalRequestRepository.InsertAsync(approvalRequest);

                var retrievedRequest = await _approvalRequestRepository.GetAsync(approvalRequest.Id);

                // Assert
                retrievedRequest.ShouldNotBeNull();
                retrievedRequest.Id.ShouldBe(approvalRequest.Id);
                retrievedRequest.Title.ShouldBe("Test Approval Request");
            });
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateApprovalRequest()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var approvalRequest = new ApprovalRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Original Title");

                await _approvalRequestRepository.InsertAsync(approvalRequest);

                approvalRequest.UpdateTitle("Updated Title");
                await _approvalRequestRepository.UpdateAsync(approvalRequest);

                var updatedRequest = await _approvalRequestRepository.GetAsync(approvalRequest.Id);

                // Assert
                updatedRequest.Title.ShouldBe("Updated Title");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteApprovalRequest()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var approvalRequest = new ApprovalRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Request to Delete");

                await _approvalRequestRepository.InsertAsync(approvalRequest);
                await _approvalRequestRepository.DeleteAsync(approvalRequest);

                // Assert
                var deletedRequest = await _approvalRequestRepository.FindAsync(approvalRequest.Id);
                deletedRequest.ShouldBeNull();
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnApprovalRequests()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var request1 = new ApprovalRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Request 1");

                var request2 = new ApprovalRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Request 2");

                await _approvalRequestRepository.InsertAsync(request1);
                await _approvalRequestRepository.InsertAsync(request2);

                var requests = await _approvalRequestRepository.GetListAsync();

                // Assert
                requests.Count.ShouldBeGreaterThanOrEqualTo(2);
            });
        }

        [Fact]
        public async Task GetByStatusAsync_Should_ReturnFilteredRequests()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var request1 = new ApprovalRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Pending Request");

                var request2 = new ApprovalRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Approved Request");
                request2.Submit();
                request2.Approve(Guid.NewGuid());

                await _approvalRequestRepository.InsertAsync(request1);
                await _approvalRequestRepository.InsertAsync(request2);

                var pendingRequests = await _approvalRequestRepository.GetByStatusAsync(ApprovalStatus.Pending);

                // Assert
                pendingRequests.ShouldContain(r => r.Id == request1.Id);
                pendingRequests.ShouldNotContain(r => r.Id == request2.Id);
            });
        }
    }
}
