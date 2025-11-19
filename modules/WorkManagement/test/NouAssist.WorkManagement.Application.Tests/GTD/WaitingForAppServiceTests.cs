using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.GTD;
using NouAssist.WorkManagement.GTD.GTD.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class WaitingForAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IWaitingForAppService _waitingForAppService;

        public WaitingForAppServiceTests()
        {
            _waitingForAppService = GetRequiredService<IWaitingForAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateWaitingFor()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateWaitingForDto
                {
                    WfType = GTD.Enums.WaitingForType.User,
                    ForUserId = Guid.NewGuid(),
                    Reason = "Waiting for approval"
                };

                // Act
                var result = await _waitingForAppService.CreateAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Id.ShouldNotBe(Guid.Empty);
                result.Reason.ShouldBe(input.Reason);
            });
        }

        [Fact]
        public async Task GetAsync_WithValidId_Should_ReturnWaitingFor()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateWaitingForDto
                {
                    WfType = GTD.Enums.WaitingForType.User,
                    Reason = "Test Reason"
                };

                var createdWaitingFor = await _waitingForAppService.CreateAsync(createInput);
                var retrievedWaitingFor = await _waitingForAppService.GetAsync(createdWaitingFor.Id);

                // Assert
                retrievedWaitingFor.ShouldNotBeNull();
                retrievedWaitingFor.Id.ShouldBe(createdWaitingFor.Id);
                retrievedWaitingFor.Reason.ShouldBe(createdWaitingFor.Reason);
            });
        }

        [Fact]
        public async Task UpdateAsync_WithValidInput_Should_UpdateWaitingFor()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateWaitingForDto
                {
                    WfType = GTD.Enums.WaitingForType.User,
                    Reason = "Original Reason"
                };

                var createdWaitingFor = await _waitingForAppService.CreateAsync(createInput);

                var updateInput = new UpdateWaitingForDto
                {
                    Reason = "Updated Reason",
                    Note = "Updated Note"
                };

                var updatedWaitingFor = await _waitingForAppService.UpdateAsync(createdWaitingFor.Id, updateInput);

                // Assert
                updatedWaitingFor.ShouldNotBeNull();
                updatedWaitingFor.Reason.ShouldBe("Updated Reason");
                updatedWaitingFor.Note.ShouldBe("Updated Note");
            });
        }

        [Fact]
        public async Task ResolveAsync_Should_ResolveWaitingFor()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateWaitingForDto
                {
                    WfType = GTD.Enums.WaitingForType.User,
                    Reason = "Test Reason"
                };

                var createdWaitingFor = await _waitingForAppService.CreateAsync(createInput);
                var resolvedWaitingFor = await _waitingForAppService.ResolveAsync(createdWaitingFor.Id);

                // Assert
                resolvedWaitingFor.ShouldNotBeNull();
                resolvedWaitingFor.Status.ShouldBe(GTD.Enums.WaitingForStatus.Resolved);
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteWaitingFor()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateWaitingForDto
                {
                    WfType = GTD.Enums.WaitingForType.User,
                    Reason = "Test Reason"
                };

                var createdWaitingFor = await _waitingForAppService.CreateAsync(createInput);
                await _waitingForAppService.DeleteAsync(createdWaitingFor.Id);

                // Assert
                await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(async () =>
                    await _waitingForAppService.GetAsync(createdWaitingFor.Id));
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnPagedResults()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                // Create test waiting for items
                await _waitingForAppService.CreateAsync(new CreateWaitingForDto
                {
                    WfType = GTD.Enums.WaitingForType.User,
                    Reason = "Reason 1"
                });
                await _waitingForAppService.CreateAsync(new CreateWaitingForDto
                {
                    WfType = GTD.Enums.WaitingForType.External,
                    Reason = "Reason 2"
                });

                var input = new GetWaitingForListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0
                };

                var result = await _waitingForAppService.GetListAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Items.ShouldNotBeEmpty();
                result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}
