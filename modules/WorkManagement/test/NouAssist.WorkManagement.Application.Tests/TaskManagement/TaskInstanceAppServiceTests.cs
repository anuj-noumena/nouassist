using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.TaskInstances;
using NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class TaskInstanceAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly ITaskInstanceAppService _taskInstanceAppService;

        public TaskInstanceAppServiceTests()
        {
            _taskInstanceAppService = GetRequiredService<ITaskInstanceAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateTaskInstance()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateTaskInstanceDto
                {
                    TaskId = Guid.NewGuid(),
                    WorkflowId = Guid.NewGuid(),
                    WorkflowStatusId = Guid.NewGuid()
                };

                var result = await _taskInstanceAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Id.ShouldNotBe(Guid.Empty);
            });
        }

        [Fact]
        public async Task GetAsync_WithValidId_Should_ReturnTaskInstance()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateTaskInstanceDto
                {
                    TaskId = Guid.NewGuid(),
                    WorkflowId = Guid.NewGuid(),
                    WorkflowStatusId = Guid.NewGuid()
                };

                var created = await _taskInstanceAppService.CreateAsync(createInput);
                var retrieved = await _taskInstanceAppService.GetAsync(created.Id);

                retrieved.ShouldNotBeNull();
                retrieved.Id.ShouldBe(created.Id);
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnPagedResults()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input1 = new CreateTaskInstanceDto
                {
                    TaskId = Guid.NewGuid(),
                    WorkflowId = Guid.NewGuid(),
                    WorkflowStatusId = Guid.NewGuid()
                };
                var input2 = new CreateTaskInstanceDto
                {
                    TaskId = Guid.NewGuid(),
                    WorkflowId = Guid.NewGuid(),
                    WorkflowStatusId = Guid.NewGuid()
                };

                await _taskInstanceAppService.CreateAsync(input1);
                await _taskInstanceAppService.CreateAsync(input2);

                var listInput = new GetTaskInstanceListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0
                };

                var result = await _taskInstanceAppService.GetListAsync(listInput);

                result.ShouldNotBeNull();
                result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}
