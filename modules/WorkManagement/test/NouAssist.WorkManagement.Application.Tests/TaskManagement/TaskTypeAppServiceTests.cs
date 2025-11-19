using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.TaskTypes;
using NouAssist.WorkManagement.TaskManagement.TaskTypes.Dtos;
using Shouldly;
using Volo.Abp.Validation;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class TaskTypeAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly ITaskTypeAppService _taskTypeAppService;

        public TaskTypeAppServiceTests()
        {
            _taskTypeAppService = GetRequiredService<ITaskTypeAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateTaskType()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateTaskTypeDto
                {
                    Name = "Bug",
                    Description = "Bug task type"
                };

                var result = await _taskTypeAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Name.ShouldBe("Bug");
            });
        }

        [Fact]
        public async Task CreateAsync_WithEmptyName_Should_ThrowValidationException()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateTaskTypeDto
                {
                    Name = ""
                };

                await Should.ThrowAsync<AbpValidationException>(async () =>
                    await _taskTypeAppService.CreateAsync(input));
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnTaskTypes()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                await _taskTypeAppService.CreateAsync(new CreateTaskTypeDto { Name = "Bug" });
                await _taskTypeAppService.CreateAsync(new CreateTaskTypeDto { Name = "Feature" });

                var input = new GetTaskTypeListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0
                };

                var result = await _taskTypeAppService.GetListAsync(input);

                result.ShouldNotBeNull();
                result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}
