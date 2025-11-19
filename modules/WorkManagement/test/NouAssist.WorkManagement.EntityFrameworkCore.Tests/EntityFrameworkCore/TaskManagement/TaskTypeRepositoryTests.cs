using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement
{
    public class TaskTypeRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly ITaskTypeRepository _taskTypeRepository;

        public TaskTypeRepositoryTests()
        {
            _taskTypeRepository = GetRequiredService<ITaskTypeRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertTaskType()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var taskType = new TaskType(Guid.NewGuid(), "Bug");
                var inserted = await _taskTypeRepository.InsertAsync(taskType);

                inserted.ShouldNotBeNull();
                inserted.Name.ShouldBe("Bug");
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnTaskType()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var taskType = new TaskType(Guid.NewGuid(), "Feature");
                await _taskTypeRepository.InsertAsync(taskType);
                
                var retrieved = await _taskTypeRepository.GetAsync(taskType.Id);
                retrieved.ShouldNotBeNull();
                retrieved.Name.ShouldBe("Feature");
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnTaskTypes()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                await _taskTypeRepository.InsertAsync(new TaskType(Guid.NewGuid(), "Bug"));
                await _taskTypeRepository.InsertAsync(new TaskType(Guid.NewGuid(), "Feature"));
                
                var list = await _taskTypeRepository.GetListAsync();
                list.Count.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}
