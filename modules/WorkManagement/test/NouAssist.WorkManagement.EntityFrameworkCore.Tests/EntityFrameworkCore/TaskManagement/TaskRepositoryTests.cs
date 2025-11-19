using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Specifications;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement
{
    public class TaskRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskRepositoryTests()
        {
            _taskRepository = GetRequiredService<ITaskRepository>();
        }

        [Fact]
        public async System.Threading.Tasks.Task InsertAsync_Should_InsertTask()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var task = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Test Task",
                    Guid.NewGuid());

                // Act
                var insertedTask = await _taskRepository.InsertAsync(task);

                // Assert
                insertedTask.ShouldNotBeNull();
                insertedTask.Id.ShouldBe(task.Id);
                insertedTask.Title.ShouldBe("Test Task");
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAsync_Should_ReturnTask()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var task = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Test Task",
                    Guid.NewGuid());

                await _taskRepository.InsertAsync(task);

                var retrievedTask = await _taskRepository.GetAsync(task.Id);

                // Assert
                retrievedTask.ShouldNotBeNull();
                retrievedTask.Id.ShouldBe(task.Id);
                retrievedTask.Title.ShouldBe("Test Task");
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task GetWithDetailsAsync_Should_IncludeRelatedEntities()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var task = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Test Task with Details",
                    Guid.NewGuid());

                await _taskRepository.InsertAsync(task);

                var retrievedTask = await _taskRepository.GetWithDetailsAsync(task.Id);

                // Assert
                retrievedTask.ShouldNotBeNull();
                retrievedTask.Id.ShouldBe(task.Id);
                retrievedTask.ChecklistItems.ShouldNotBeNull();
                retrievedTask.AcceptanceCriteria.ShouldNotBeNull();
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateAsync_Should_UpdateTask()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var task = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Original Title",
                    Guid.NewGuid());

                await _taskRepository.InsertAsync(task);

                task.Title = "Updated Title";
                await _taskRepository.UpdateAsync(task);

                var updatedTask = await _taskRepository.GetAsync(task.Id);

                // Assert
                updatedTask.Title.ShouldBe("Updated Title");
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteAsync_Should_DeleteTask()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var task = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task to Delete",
                    Guid.NewGuid());

                await _taskRepository.InsertAsync(task);
                await _taskRepository.DeleteAsync(task);

                // Assert
                var deletedTask = await _taskRepository.FindAsync(task.Id);
                deletedTask.ShouldBeNull();
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task GetListAsync_WithSpecification_Should_ReturnFilteredTasks()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var projectId = Guid.NewGuid();

                var task1 = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task 1",
                    Guid.NewGuid(),
                    projectId: projectId);

                var task2 = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task 2",
                    Guid.NewGuid(),
                    projectId: projectId);

                var task3 = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task 3",
                    Guid.NewGuid(),
                    projectId: Guid.NewGuid()); // Different project

                await _taskRepository.InsertAsync(task1);
                await _taskRepository.InsertAsync(task2);
                await _taskRepository.InsertAsync(task3);

                var spec = new TasksByProjectSpecification(projectId);
                var tasks = await _taskRepository.GetListAsync(spec);

                // Assert
                tasks.Count.ShouldBe(2);
                tasks.ShouldAllBe(t => t.ProjectId == projectId);
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task GetCountAsync_Should_ReturnCorrectCount()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var task1 = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task 1",
                    Guid.NewGuid());

                var task2 = new Task(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Task 2",
                    Guid.NewGuid());

                await _taskRepository.InsertAsync(task1);
                await _taskRepository.InsertAsync(task2);

                var count = await _taskRepository.GetCountAsync();

                // Assert
                count.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}
