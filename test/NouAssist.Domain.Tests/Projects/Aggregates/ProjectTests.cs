using System;
using NouAssist.Projects.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Projects.Aggregates
{
    public class ProjectTests
    {
        [Fact]
        public void Should_Create_Project_With_Valid_Data()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Test Project";

            // Act
            var project = new Project(
                Guid.NewGuid(),
                ownerId,
                title,
                null,
                "Test Description");

            // Assert
            project.OwnerId.ShouldBe(ownerId);
            project.Title.ShouldBe(title);
            project.Status.ShouldBe(ProjectStatus.Planned);
            project.Progress.ShouldBe(0);
        }

        [Fact]
        public void Should_Throw_Exception_When_Title_Is_Empty()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                new Project(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "",
                    null);
            });
        }

        [Fact]
        public void Should_Start_Planned_Project()
        {
            // Arrange
            var project = CreateTestProject();

            // Act
            project.Start();

            // Assert
            project.Status.ShouldBe(ProjectStatus.Active);
            project.StartDate.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Not_Start_Active_Project()
        {
            // Arrange
            var project = CreateTestProject();
            project.Start();

            // Act & Assert
            Should.Throw<BusinessException>(() => project.Start())
                .Code.ShouldBe("Projects:CannotStart");
        }

        [Fact]
        public void Should_Complete_Project()
        {
            // Arrange
            var project = CreateTestProject();
            project.Start();
            var completedBy = "test-user";

            // Act
            project.Complete(completedBy);

            // Assert
            project.Status.ShouldBe(ProjectStatus.Completed);
            project.CompletedAt.ShouldNotBeNull();
            project.Progress.ShouldBe(100);
        }

        [Fact]
        public void Should_Update_Priority_And_Quadrant()
        {
            // Arrange
            var project = CreateTestProject();

            // Act
            project.SetPriority(0.8, 0.8);

            // Assert
            project.Importance.ShouldBe(0.8);
            project.Urgency.ShouldBe(0.8);
            project.EisenhowerQuadrant.ShouldBe(EisenhowerQuadrant.Q1DoFirst);
        }

        [Fact]
        public void Should_Calculate_Health_As_Healthy()
        {
            // Arrange
            var project = CreateTestProject();
            project.Start();
            project.UpdateProgress(60);

            // Act
            var health = project.Health;

            // Assert
            health.ShouldBe(ProjectHealth.Healthy);
        }

        [Fact]
        public void Should_Detect_Overdue_Project()
        {
            // Arrange
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                null,
                dueDate: DateTime.UtcNow.AddDays(-1));

            project.Start();

            // Act & Assert
            project.IsOverdue.ShouldBeTrue();
        }

        [Fact]
        public void Should_Add_Success_Criterion()
        {
            // Arrange
            var project = CreateTestProject();
            var criterionId = Guid.NewGuid();

            // Act
            project.AddSuccessCriterion(criterionId);

            // Assert
            project.HasSuccessCriterion(criterionId).ShouldBeTrue();
        }

        private static Project CreateTestProject()
        {
            return new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                null,
                "Test Description",
                startDate: DateTime.UtcNow,
                dueDate: DateTime.UtcNow.AddDays(30));
        }
    }
}
