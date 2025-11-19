using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Exceptions;
using NouAssist.WorkManagement.TaskManagement.Managers;
using NSubstitute;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class TaskManagerTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly TaskManager _taskManager;
        private readonly IGuidGenerator _guidGenerator;

        public TaskManagerTests()
        {
            _taskRepository = Substitute.For<ITaskRepository>();
            var logger = Substitute.For<ILogger<TaskManager>>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();

            _taskManager = new TaskManager(_taskRepository, logger);

            // Set up required services for domain service
            var currentTenant = Substitute.For<ICurrentTenant>();
            currentTenant.Id.Returns((Guid?)null);
            _taskManager.CurrentTenant = currentTenant;
            _taskManager.GuidGenerator = _guidGenerator;
        }

        [Fact]
        public async Task CreateAsync_WithValidParameters_Should_CreateTask()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var taskTypeId = Guid.NewGuid();
            var title = "Test Task";

            // Act
            var task = await _taskManager.CreateAsync(ownerId, title, taskTypeId);

            // Assert
            task.ShouldNotBeNull();
            task.OwnerId.ShouldBe(ownerId);
            task.TaskTypeId.ShouldBe(taskTypeId);
            task.Title.ShouldBe(title);
            task.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task CreateAsync_WithDescription_Should_SetDescription()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var taskTypeId = Guid.NewGuid();
            var title = "Test Task";
            var description = "Test Description";

            // Act
            var task = await _taskManager.CreateAsync(
                ownerId,
                title,
                taskTypeId,
                description: description);

            // Assert
            task.Description.ShouldBe(description);
        }

        [Fact]
        public async Task CreateAsync_WithProjectId_Should_SetProjectId()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var taskTypeId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var title = "Test Task";

            _taskRepository.GetListAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Aggregates.Task, bool>>>())
                .Returns(new System.Collections.Generic.List<Aggregates.Task>());

            // Act
            var task = await _taskManager.CreateAsync(
                ownerId,
                title,
                taskTypeId,
                projectId: projectId);

            // Assert
            task.ProjectId.ShouldBe(projectId);
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateTitleInProject_Should_ThrowException()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var taskTypeId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var title = "Duplicate Task";

            var existingTask = new Aggregates.Task(
                Guid.NewGuid(),
                ownerId,
                title,
                taskTypeId,
                projectId: projectId);

            _taskRepository.GetListAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Aggregates.Task, bool>>>())
                .Returns(new System.Collections.Generic.List<Aggregates.Task> { existingTask });

            // Act & Assert
            await Should.ThrowAsync<TaskDomainException>(async () =>
                await _taskManager.CreateAsync(ownerId, title, taskTypeId, projectId: projectId));
        }

        [Fact]
        public async Task CreateAsync_WithGTDArea_Should_SetAreaId()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var taskTypeId = Guid.NewGuid();
            var areaId = Guid.NewGuid();
            var title = "Test Task";

            // Act
            var task = await _taskManager.CreateAsync(
                ownerId,
                title,
                taskTypeId,
                areaId: areaId);

            // Assert
            task.AreaId.ShouldBe(areaId);
        }

        [Fact]
        public async Task CreateAsync_WithGTDContext_Should_SetContextId()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var taskTypeId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var title = "Test Task";

            // Act
            var task = await _taskManager.CreateAsync(
                ownerId,
                title,
                taskTypeId,
                contextId: contextId);

            // Assert
            task.ContextId.ShouldBe(contextId);
        }
    }
}
