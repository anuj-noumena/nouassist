using System;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using NouAssist.PersonalDevelopment.Goals.Entities;
using NouAssist.SharedKernel.Enums;
using NouAssist.SharedKernel.ValueObjects;
using Shouldly;
using Volo.Abp;
using Xunit;
using GoalStatus = NouAssist.PersonalDevelopment.Goals.Enums.GoalStatus;
using GoalProgressLevel = NouAssist.PersonalDevelopment.Goals.Enums.GoalProgressLevel;

namespace NouAssist.PersonalDevelopment.Goals
{
    public class GoalTests : PersonalDevelopmentDomainTestBase<PersonalDevelopmentDomainTestModule>
    {
        private readonly Guid _userId = Guid.NewGuid();

        [Fact]
        public void Should_Create_Goal_With_Valid_Data()
        {
            // Arrange & Act
            var goal = new Goal(
                Guid.NewGuid(),
                _userId,
                "Complete Project X",
                ImportanceScore.High(),
                UrgencyScore.Default()
            );

            // Assert
            goal.ShouldNotBeNull();
            goal.Title.ShouldBe("Complete Project X");
            goal.OwnerId.ShouldBe(_userId);
            goal.Status.ShouldBe(GoalStatus.Active);
            goal.Progress.ShouldBe(0);
            goal.Importance.ShouldNotBeNull();
            goal.Urgency.ShouldNotBeNull();
            goal.IsActive.ShouldBeTrue();
            goal.IsCompleted.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Create_Goal_With_Empty_Title()
        {
            // Assert
            Should.Throw<ArgumentException>(() =>
                new Goal(Guid.NewGuid(), _userId, ""));
        }

        [Fact]
        public void Should_Not_Create_Goal_With_Null_Title()
        {
            // Assert
            Should.Throw<ArgumentException>(() =>
                new Goal(Guid.NewGuid(), _userId, null));
        }

        [Fact]
        public void Should_Update_Progress_Successfully()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");

            // Act
            goal.UpdateProgress(50);

            // Assert
            goal.Progress.ShouldBe(50);
            goal.IsCompleted.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Allow_Progress_Below_Zero()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");

            // Assert
            Should.Throw<BusinessException>(() => goal.UpdateProgress(-1));
        }

        [Fact]
        public void Should_Not_Allow_Progress_Above_100()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");

            // Assert
            Should.Throw<BusinessException>(() => goal.UpdateProgress(101));
        }

        [Fact]
        public void Should_Auto_Achieve_When_Progress_Reaches_100()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");

            // Act
            goal.UpdateProgress(100);

            // Assert
            goal.Status.ShouldBe(GoalStatus.Achieved);
            goal.IsAchieved.ShouldBeTrue();
            goal.IsCompleted.ShouldBeTrue();
            goal.CompletedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Achieve_Goal_Successfully()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            var notes = "Completed ahead of schedule";

            // Act
            goal.Achieve(notes);

            // Assert
            goal.Status.ShouldBe(GoalStatus.Achieved);
            goal.Progress.ShouldBe(100);
            goal.CompletionNotes.ShouldBe(notes);
            goal.CompletedAt.ShouldNotBeNull();
            goal.IsCompleted.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Achieve_Already_Achieved_Goal()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            goal.Achieve();

            // Assert
            Should.Throw<BusinessException>(() => goal.Achieve());
        }

        [Fact]
        public void Should_Pause_Active_Goal()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");

            // Act
            goal.Pause();

            // Assert
            goal.Status.ShouldBe(GoalStatus.OnHold);
            goal.IsOnHold.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Pause_Non_Active_Goal()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            goal.Achieve();

            // Assert
            Should.Throw<BusinessException>(() => goal.Pause());
        }

        [Fact]
        public void Should_Resume_OnHold_Goal()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            goal.Pause();

            // Act
            goal.Resume();

            // Assert
            goal.Status.ShouldBe(GoalStatus.Active);
            goal.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Resume_Non_OnHold_Goal()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");

            // Assert
            Should.Throw<BusinessException>(() => goal.Resume());
        }

        [Fact]
        public void Should_Archive_Goal_With_Reason()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            var reason = "No longer relevant";

            // Act
            goal.Archive(reason);

            // Assert
            goal.Status.ShouldBe(GoalStatus.Archived);
            goal.ArchiveReason.ShouldBe(reason);
            goal.IsArchived.ShouldBeTrue();
        }

        [Fact]
        public void Should_Add_Milestone_Successfully()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            var milestone = new GoalMilestone(Guid.NewGuid(), goal.Id, "Milestone 1");

            // Act
            goal.AddMilestone(milestone);

            // Assert
            goal.Milestones.Count.ShouldBe(1);
            goal.Milestones.ShouldContain(milestone);
        }

        [Fact]
        public void Should_Not_Add_Duplicate_Milestone_Title()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            var milestone1 = new GoalMilestone(Guid.NewGuid(), goal.Id, "Milestone 1");
            var milestone2 = new GoalMilestone(Guid.NewGuid(), goal.Id, "Milestone 1");
            goal.AddMilestone(milestone1);

            // Assert
            Should.Throw<BusinessException>(() => goal.AddMilestone(milestone2));
        }

        [Fact]
        public void Should_Remove_Milestone_Successfully()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            var milestone = new GoalMilestone(Guid.NewGuid(), goal.Id, "Milestone 1");
            goal.AddMilestone(milestone);

            // Act
            goal.RemoveMilestone(milestone.Id);

            // Assert
            goal.Milestones.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Update_Importance_Successfully()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            var newImportance = new ImportanceScore(0.9);

            // Act
            goal.UpdateImportance(newImportance);

            // Assert
            goal.Importance.ShouldBe(newImportance);
        }

        [Fact]
        public void Should_Update_Urgency_Successfully()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            var newUrgency = new UrgencyScore(0.9);

            // Act
            goal.UpdateUrgency(newUrgency);

            // Assert
            goal.Urgency.ShouldBe(newUrgency);
        }

        [Fact]
        public void Should_Detect_Overdue_Goal()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal")
            {
                TargetDate = DateTime.UtcNow.AddDays(-1)
            };

            // Assert
            goal.IsOverdue.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Mark_Completed_Goal_As_Overdue()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal")
            {
                TargetDate = DateTime.UtcNow.AddDays(-1)
            };
            goal.Achieve();

            // Assert
            goal.IsOverdue.ShouldBeFalse();
        }

        [Fact]
        public void Should_Calculate_Days_Remaining_Correctly()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal")
            {
                TargetDate = DateTime.UtcNow.AddDays(5)
            };

            // Act
            var daysRemaining = goal.GetDaysRemaining();

            // Assert
            daysRemaining.ShouldNotBeNull();
            daysRemaining.Value.ShouldBeGreaterThanOrEqualTo(4);
            daysRemaining.Value.ShouldBeLessThanOrEqualTo(5);
        }

        [Fact]
        public void Should_Return_Correct_Progress_Level()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");

            // Act & Assert
            goal.UpdateProgress(0);
            goal.GetProgressLevel().ShouldBe(GoalProgressLevel.NotStarted);

            goal.UpdateProgress(10);
            goal.GetProgressLevel().ShouldBe(GoalProgressLevel.JustStarted);

            goal.UpdateProgress(35);
            goal.GetProgressLevel().ShouldBe(GoalProgressLevel.Progressing);

            goal.UpdateProgress(60);
            goal.GetProgressLevel().ShouldBe(GoalProgressLevel.WellUnderway);

            goal.UpdateProgress(85);
            goal.GetProgressLevel().ShouldBe(GoalProgressLevel.NearlyComplete);
        }

        [Fact]
        public void Should_Calculate_Milestone_Completion_Rate()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");
            var milestone1 = new GoalMilestone(Guid.NewGuid(), goal.Id, "Milestone 1");
            var milestone2 = new GoalMilestone(Guid.NewGuid(), goal.Id, "Milestone 2");
            var milestone3 = new GoalMilestone(Guid.NewGuid(), goal.Id, "Milestone 3");

            goal.AddMilestone(milestone1);
            goal.AddMilestone(milestone2);
            goal.AddMilestone(milestone3);

            milestone1.Complete();

            // Act
            var completionRate = goal.GetMilestoneCompletionRate();

            // Assert
            completionRate.ShouldBe(1.0 / 3.0, 0.001);
            goal.GetCompletedMilestonesCount().ShouldBe(1);
        }

        [Fact]
        public void Should_Return_Zero_Milestone_Completion_Rate_When_No_Milestones()
        {
            // Arrange
            var goal = new Goal(Guid.NewGuid(), _userId, "Test Goal");

            // Act
            var completionRate = goal.GetMilestoneCompletionRate();

            // Assert
            completionRate.ShouldBe(0.0);
        }
    }
}
