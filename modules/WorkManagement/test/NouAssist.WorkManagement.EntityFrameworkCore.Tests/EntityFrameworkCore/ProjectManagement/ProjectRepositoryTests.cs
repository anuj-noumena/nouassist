using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement
{
    public class ProjectRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectRepositoryTests()
        {
            _projectRepository = GetRequiredService<IProjectRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertProject()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var project = new Project(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Test Project",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                // Act
                var insertedProject = await _projectRepository.InsertAsync(project);

                // Assert
                insertedProject.ShouldNotBeNull();
                insertedProject.Id.ShouldBe(project.Id);
                insertedProject.Title.ShouldBe("Test Project");
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnProject()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var project = new Project(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Test Project",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                await _projectRepository.InsertAsync(project);

                var retrievedProject = await _projectRepository.GetAsync(project.Id);

                // Assert
                retrievedProject.ShouldNotBeNull();
                retrievedProject.Id.ShouldBe(project.Id);
                retrievedProject.Title.ShouldBe("Test Project");
            });
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateProject()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var project = new Project(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Original Title",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                await _projectRepository.InsertAsync(project);

                project.UpdateTitle("Updated Title");
                await _projectRepository.UpdateAsync(project);

                var updatedProject = await _projectRepository.GetAsync(project.Id);

                // Assert
                updatedProject.Title.ShouldBe("Updated Title");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteProject()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var project = new Project(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Project to Delete",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                await _projectRepository.InsertAsync(project);
                await _projectRepository.DeleteAsync(project);

                // Assert
                var deletedProject = await _projectRepository.FindAsync(project.Id);
                deletedProject.ShouldBeNull();
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnProjects()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var project1 = new Project(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Project 1",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                var project2 = new Project(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Project 2",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                await _projectRepository.InsertAsync(project1);
                await _projectRepository.InsertAsync(project2);

                var projects = await _projectRepository.GetListAsync();

                // Assert
                projects.Count.ShouldBeGreaterThanOrEqualTo(2);
            });
        }

        [Fact]
        public async Task GetByOwnerAsync_Should_ReturnOwnerProjects()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var ownerId = Guid.NewGuid();

                var project1 = new Project(
                    Guid.NewGuid(),
                    ownerId,
                    "Owner Project 1",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                var project2 = new Project(
                    Guid.NewGuid(),
                    ownerId,
                    "Owner Project 2",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                var otherProject = new Project(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Other Project",
                    DateTime.Now,
                    DateTime.Now.AddDays(30));

                await _projectRepository.InsertAsync(project1);
                await _projectRepository.InsertAsync(project2);
                await _projectRepository.InsertAsync(otherProject);

                var ownerProjects = await _projectRepository.GetByOwnerAsync(ownerId);

                // Assert
                ownerProjects.Count.ShouldBe(2);
                ownerProjects.ShouldAllBe(p => p.OwnerId == ownerId);
            });
        }
    }
}
