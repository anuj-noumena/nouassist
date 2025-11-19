using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement
{
    public class TaskInstanceRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly ITaskInstanceRepository _taskInstanceRepository;

        public TaskInstanceRepositoryTests()
        {
            _taskInstanceRepository = GetRequiredService<ITaskInstanceRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertTaskInstance()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var taskInstance = new TaskInstance(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    null, null, null);

                var inserted = await _taskInstanceRepository.InsertAsync(taskInstance);

                inserted.ShouldNotBeNull();
                inserted.Id.ShouldBe(taskInstance.Id);
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnTaskInstance()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var taskInstance = new TaskInstance(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    null, null, null);

                await _taskInstanceRepository.InsertAsync(taskInstance);
                var retrieved = await _taskInstanceRepository.GetAsync(taskInstance.Id);

                retrieved.ShouldNotBeNull();
                retrieved.Id.ShouldBe(taskInstance.Id);
            });
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateTaskInstance()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var taskInstance = new TaskInstance(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    null, null, null);

                await _taskInstanceRepository.InsertAsync(taskInstance);
                taskInstance.Notes = "Updated notes";
                await _taskInstanceRepository.UpdateAsync(taskInstance);
                
                var updated = await _taskInstanceRepository.GetAsync(taskInstance.Id);
                updated.Notes.ShouldBe("Updated notes");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteTaskInstance()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var taskInstance = new TaskInstance(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    null, null, null);

                await _taskInstanceRepository.InsertAsync(taskInstance);
                await _taskInstanceRepository.DeleteAsync(taskInstance);
                
                var deleted = await _taskInstanceRepository.FindAsync(taskInstance.Id);
                deleted.ShouldBeNull();
            });
        }
    }
}
