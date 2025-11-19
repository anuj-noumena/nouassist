using System;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectSprintTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateProjectSprint()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var name = "Sprint 1";
            var goal = "Complete user authentication";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(14);

            // Act
            var sprint = new ProjectSprint(
                Guid.NewGuid(),
                projectId,
                name,
                goal,
                startDate,
                endDate,
                null);

            // Assert
            sprint.ShouldNotBeNull();
            sprint.ProjectId.ShouldBe(projectId);
            sprint.Name.ShouldBe(name);
            sprint.Goal.ShouldBe(goal);
            sprint.Status.ShouldBe(SprintStatus.Planned);
        }

        [Fact]
        public void Constructor_WithEmptyName_Should_ThrowException()
        {
            // Act & Assert
            Should.Throw<ArgumentException>(() => new ProjectSprint(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                "Goal",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                null));
        }

        [Fact]
        public void UpdateDetails_Should_UpdateNameAndGoal()
        {
            // Arrange
            var sprint = new ProjectSprint(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Sprint 1",
                "Initial goal",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                null);

            // Act
            sprint.UpdateDetails("Sprint 2", "Updated goal");

            // Assert
            sprint.Name.ShouldBe("Sprint 2");
            sprint.Goal.ShouldBe("Updated goal");
        }

        [Fact]
        public void UpdateDates_Should_UpdateTimeline()
        {
            // Arrange
            var sprint = new ProjectSprint(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Sprint 1",
                "Goal",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                null);
            var newStartDate = DateTime.Now.AddDays(1);
            var newEndDate = DateTime.Now.AddDays(15);

            // Act
            sprint.UpdateDates(newStartDate, newEndDate);

            // Assert
            sprint.Timeline.ShouldNotBeNull();
        }

        [Fact]
        public void Start_WhenPlanned_Should_ChangeStatusToInProgress()
        {
            // Arrange
            var sprint = new ProjectSprint(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Sprint 1",
                "Goal",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                null);

            // Act
            sprint.Start();

            // Assert
            sprint.Status.ShouldBe(SprintStatus.InProgress);
        }

        [Fact]
        public void Start_WhenAlreadyStarted_Should_ThrowException()
        {
            // Arrange
            var sprint = new ProjectSprint(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Sprint 1",
                "Goal",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                null);
            sprint.Start();

            // Act & Assert
            Should.Throw<BusinessException>(() => sprint.Start());
        }

        [Fact]
        public void Complete_Should_ChangeStatusToCompleted()
        {
            // Arrange
            var sprint = new ProjectSprint(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Sprint 1",
                "Goal",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                null);
            sprint.Start();

            // Act
            sprint.Complete("John Doe", "Retrospective notes");

            // Assert
            sprint.Status.ShouldBe(SprintStatus.Completed);
            sprint.RetrospectiveNotes.ShouldBe("Retrospective notes");
        }

        [Fact]
        public void Cancel_Should_ChangeStatusToCancelled()
        {
            // Arrange
            var sprint = new ProjectSprint(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Sprint 1",
                "Goal",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                null);

            // Act
            sprint.Cancel("Project cancelled");

            // Assert
            sprint.Status.ShouldBe(SprintStatus.Cancelled);
        }

        [Fact]
        public void UpdateMetrics_Should_UpdateSprintMetrics()
        {
            // Arrange
            var sprint = new ProjectSprint(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Sprint 1",
                "Goal",
                DateTime.Now,
                DateTime.Now.AddDays(14),
                null,
                velocityEstimate: 50);

            // Act
            sprint.UpdateMetrics(10, 8, 5);

            // Assert
            sprint.Metrics.ShouldNotBeNull();
        }
    }
}
