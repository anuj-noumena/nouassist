using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Tasks;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class TaskAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly ITaskAppService _taskAppService;

        public TaskAppServiceTests()
        {
            _taskAppService = GetRequiredService<ITaskAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateTask()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateTaskDto
                {
                    Title = "Test Task",
                    TaskTypeId = Guid.NewGuid(),
                    Description = "Test Description"
                };

                // Act
                var result = await _taskAppService.CreateAsync(input);

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
                var input = new CreateTaskDto
                {
                    Title = "", // Empty title
                    TaskTypeId = Guid.NewGuid()
                };

                // Act & Assert
                await Should.ThrowAsync<AbpValidationException>(async () =>
                    await _taskAppService.CreateAsync(input));
            });
        }

        [Fact]
        public async Task GetAsync_WithValidId_Should_ReturnTask()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateTaskDto
                {
                    Title = "Test Task",
                    TaskTypeId = Guid.NewGuid()
                };

                var createdTask = await _taskAppService.CreateAsync(createInput);
                var retrievedTask = await _taskAppService.GetAsync(createdTask.Id);

                // Assert
                retrievedTask.ShouldNotBeNull();
                retrievedTask.Id.ShouldBe(createdTask.Id);
                retrievedTask.Title.ShouldBe(createdTask.Title);
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnPagedResults()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                // Create test tasks
                var taskTypeId = Guid.NewGuid();
                await _taskAppService.CreateAsync(new CreateTaskDto
                {
                    Title = "Task 1",
                    TaskTypeId = taskTypeId
                });
                await _taskAppService.CreateAsync(new CreateTaskDto
                {
                    Title = "Task 2",
                    TaskTypeId = taskTypeId
                });

                var input = new GetTaskListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0
                };

                var result = await _taskAppService.GetListAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Items.ShouldNotBeEmpty();
                result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            });
        }

        [Fact]
        public async Task UpdateAsync_WithValidInput_Should_UpdateTask()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateTaskDto
                {
                    Title = "Original Title",
                    TaskTypeId = Guid.NewGuid()
                };

                var createdTask = await _taskAppService.CreateAsync(createInput);

                var updateInput = new UpdateTaskDto
                {
                    Title = "Updated Title",
                    Description = "Updated Description"
                };

                var updatedTask = await _taskAppService.UpdateAsync(createdTask.Id, updateInput);

                // Assert
                updatedTask.ShouldNotBeNull();
                updatedTask.Title.ShouldBe("Updated Title");
                updatedTask.Description.ShouldBe("Updated Description");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteTask()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateTaskDto
                {
                    Title = "Task to Delete",
                    TaskTypeId = Guid.NewGuid()
                };

                var createdTask = await _taskAppService.CreateAsync(createInput);
                await _taskAppService.DeleteAsync(createdTask.Id);

                // Assert
                await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(async () =>
                    await _taskAppService.GetAsync(createdTask.Id));
            });
        }

        [Fact]
        public async Task GetListAsync_WithProjectIdFilter_Should_ReturnFilteredTasks()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var projectId = Guid.NewGuid();
                var taskTypeId = Guid.NewGuid();

                await _taskAppService.CreateAsync(new CreateTaskDto
                {
                    Title = "Project Task 1",
                    TaskTypeId = taskTypeId,
                    ProjectId = projectId
                });

                await _taskAppService.CreateAsync(new CreateTaskDto
                {
                    Title = "Other Task",
                    TaskTypeId = taskTypeId,
                    ProjectId = Guid.NewGuid() // Different project
                });

                var input = new GetTaskListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0,
                    ProjectId = projectId
                };

                var result = await _taskAppService.GetListAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Items.ShouldAllBe(t => t.ProjectId == projectId);
            });
        }
    }
}
