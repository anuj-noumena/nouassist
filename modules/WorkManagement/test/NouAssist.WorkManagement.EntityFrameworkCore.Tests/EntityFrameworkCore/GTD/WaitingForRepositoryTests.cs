using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD
{
    public class WaitingForRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly IWaitingForRepository _waitingForRepository;

        public WaitingForRepositoryTests()
        {
            _waitingForRepository = GetRequiredService<IWaitingForRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertWaitingFor()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var waitingFor = new WaitingFor(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    WaitingForType.User);

                // Act
                var insertedWaitingFor = await _waitingForRepository.InsertAsync(waitingFor);

                // Assert
                insertedWaitingFor.ShouldNotBeNull();
                insertedWaitingFor.Id.ShouldBe(waitingFor.Id);
                insertedWaitingFor.Status.ShouldBe(WaitingForStatus.Active);
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnWaitingFor()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var waitingFor = new WaitingFor(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    WaitingForType.User);

                await _waitingForRepository.InsertAsync(waitingFor);

                var retrievedWaitingFor = await _waitingForRepository.GetAsync(waitingFor.Id);

                // Assert
                retrievedWaitingFor.ShouldNotBeNull();
                retrievedWaitingFor.Id.ShouldBe(waitingFor.Id);
            });
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateWaitingFor()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var waitingFor = new WaitingFor(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    WaitingForType.User);

                await _waitingForRepository.InsertAsync(waitingFor);

                waitingFor.Reason = "Updated Reason";
                await _waitingForRepository.UpdateAsync(waitingFor);

                var updatedWaitingFor = await _waitingForRepository.GetAsync(waitingFor.Id);

                // Assert
                updatedWaitingFor.Reason.ShouldBe("Updated Reason");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteWaitingFor()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var waitingFor = new WaitingFor(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    WaitingForType.User);

                await _waitingForRepository.InsertAsync(waitingFor);
                await _waitingForRepository.DeleteAsync(waitingFor);

                // Assert
                var deletedWaitingFor = await _waitingForRepository.FindAsync(waitingFor.Id);
                deletedWaitingFor.ShouldBeNull();
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnWaitingForItems()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var waitingFor1 = new WaitingFor(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    WaitingForType.User);

                var waitingFor2 = new WaitingFor(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    WaitingForType.External);

                await _waitingForRepository.InsertAsync(waitingFor1);
                await _waitingForRepository.InsertAsync(waitingFor2);

                var waitingForItems = await _waitingForRepository.GetListAsync();

                // Assert
                waitingForItems.Count.ShouldBeGreaterThanOrEqualTo(2);
            });
        }

        [Fact]
        public async Task GetByOwnerAsync_Should_ReturnOwnerWaitingForItems()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var ownerId = Guid.NewGuid();

                var waitingFor1 = new WaitingFor(
                    Guid.NewGuid(),
                    ownerId,
                    WaitingForType.User);

                var waitingFor2 = new WaitingFor(
                    Guid.NewGuid(),
                    ownerId,
                    WaitingForType.External);

                var otherWaitingFor = new WaitingFor(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    WaitingForType.User);

                await _waitingForRepository.InsertAsync(waitingFor1);
                await _waitingForRepository.InsertAsync(waitingFor2);
                await _waitingForRepository.InsertAsync(otherWaitingFor);

                var ownerWaitingForItems = await _waitingForRepository.GetByOwnerAsync(ownerId);

                // Assert
                ownerWaitingForItems.Count.ShouldBe(2);
                ownerWaitingForItems.ShouldAllBe(w => w.OwnerId == ownerId);
            });
        }
    }
}
