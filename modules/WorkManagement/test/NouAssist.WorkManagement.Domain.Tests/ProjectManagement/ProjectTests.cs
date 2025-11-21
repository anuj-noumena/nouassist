using System;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using NouAssist.WorkManagement.ProjectManagement.ValueObjects;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateProject()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Test Project";
            var tenantId = Guid.NewGuid();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(30);

            // Act
            var project = new Project(
                Guid.NewGuid(),
                ownerId,
                title,
                tenantId,
                areaId: null,
                description: null,
                expectedOutcome: null,
                startDate: startDate,
                dueDate: endDate);

            // Assert
            project.ShouldNotBeNull();
            project.OwnerId.ShouldBe(ownerId);
            project.Title.ShouldBe(title);
            project.Status.ShouldBe(ProjectStatus.Planned);
        }

        [Fact]
        public void Start_Should_ChangeStatusToActive()
        {
            // Arrange
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                tenantId: null,
                startDate: DateTime.Now,
                dueDate: DateTime.Now.AddDays(30));

            // Act
            project.Start();

            // Assert
            project.Status.ShouldBe(ProjectStatus.Active);
            project.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void Complete_Should_ChangeStatusToCompleted()
        {
            // Arrange
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                tenantId: null,
                startDate: DateTime.Now,
                dueDate: DateTime.Now.AddDays(30));
            project.Start();

            // Act
            project.Complete("Test User");

            // Assert
            project.Status.ShouldBe(ProjectStatus.Completed);
            project.IsCompleted.ShouldBeTrue();
        }

        [Fact]
        public void Pause_Should_ChangeStatusToOnHold()
        {
            // Arrange
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                tenantId: null,
                startDate: DateTime.Now,
                dueDate: DateTime.Now.AddDays(30));
            project.Start();

            // Act
            project.Pause();

            // Assert
            project.Status.ShouldBe(ProjectStatus.OnHold);
            project.IsOnHold.ShouldBeTrue();
        }

        [Fact]
        public void Archive_Should_SetIsArchivedToTrue()
        {
            // Arrange
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                tenantId: null,
                startDate: DateTime.Now,
                dueDate: DateTime.Now.AddDays(30));
            project.Complete("Test User");

            // Act
            project.Archive();

            // Assert
            project.IsArchived.ShouldBeTrue();
            project.ArchivedAt.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateProgress_Should_SetProgress()
        {
            // Arrange
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                tenantId: null,
                startDate: DateTime.Now,
                dueDate: DateTime.Now.AddDays(30));

            // Act
            project.UpdateProgress(50.0);

            // Assert
            project.Progress.ShouldBe(50.0);
        }

        [Fact]
        public void SetAreaId_Should_UpdateAreaId()
        {
            // Arrange
            var areaId = Guid.NewGuid();
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                tenantId: null,
                areaId: areaId,
                startDate: DateTime.Now,
                dueDate: DateTime.Now.AddDays(30));

            // Assert
            project.AreaId.ShouldBe(areaId);
        }
    }
}
