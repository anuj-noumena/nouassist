using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Habits.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;
using DayOfWeek = NouAssist.Habits.Enums.DayOfWeek;

namespace NouAssist.Habits.Aggregates
{
    public class HabitTests
    {
        [Fact]
        public void Create_Habit_Should_Set_Properties_Correctly()
        {
            // Arrange & Act
            var habit = new Habit(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Morning Exercise",
                "Exercise for 30 minutes every morning",
                HabitType.Make
            );

            // Assert
            habit.Title.ShouldBe("Morning Exercise");
            habit.Goal.ShouldBe("Exercise for 30 minutes every morning");
            habit.HabitType.ShouldBe(HabitType.Make);
            habit.IsActive.ShouldBeTrue();
            habit.IsMakeHabit.ShouldBeTrue();
            habit.IsBreakHabit.ShouldBeFalse();
        }

        [Fact]
        public void AddCue_Should_Add_To_Collection()
        {
            // Arrange
            var habit = CreateTestHabit();

            // Act
            habit.AddCue(
                Guid.NewGuid(),
                CueType.Time,
                "Every morning at 7 AM",
                "07:00"
            );

            // Assert
            habit.Cues.Count.ShouldBe(1);
            habit.Cues.First().CueType.ShouldBe(CueType.Time);
        }

        [Fact]
        public void AddCue_Should_Throw_When_Duplicate_Id()
        {
            // Arrange
            var habit = CreateTestHabit();
            var cueId = Guid.NewGuid();
            habit.AddCue(cueId, CueType.Time, "Morning", "07:00");

            // Act & Assert
            Should.Throw<BusinessException>(() => 
                habit.AddCue(cueId, CueType.Time, "Evening", "19:00")
            );
        }

        [Fact]
        public void UpdateSchedule_Should_Set_Scheduled_Days()
        {
            // Arrange
            var habit = CreateTestHabit();

            // Act
            habit.UpdateSchedule(new List<DayOfWeek> 
            { 
                DayOfWeek.Monday, 
                DayOfWeek.Wednesday, 
                DayOfWeek.Friday 
            });

            // Assert
            habit.ScheduledDays.Count.ShouldBe(3);
            habit.HasSchedule.ShouldBeTrue();
            habit.IsEveryday.ShouldBeFalse();
        }

        [Fact]
        public void UpdateSchedule_Should_Remove_Duplicates()
        {
            // Arrange
            var habit = CreateTestHabit();

            // Act
            habit.UpdateSchedule(new List<DayOfWeek> 
            { 
                DayOfWeek.Monday, 
                DayOfWeek.Monday, 
                DayOfWeek.Tuesday 
            });

            // Assert
            habit.ScheduledDays.Count.ShouldBe(2);
        }

        [Fact]
        public void IsScheduledForDay_Should_Return_True_When_NoSchedule()
        {
            // Arrange
            var habit = CreateTestHabit();
            var testDate = new DateTime(2024, 1, 1);

            // Act
            var result = habit.IsScheduledForDay(testDate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsScheduledForDay_Should_Return_True_When_DayInSchedule()
        {
            // Arrange
            var habit = CreateTestHabit();
            var monday = new DateTime(2024, 1, 1); // A Monday
            habit.UpdateSchedule(new List<DayOfWeek> { DayOfWeek.Monday });

            // Act
            var result = habit.IsScheduledForDay(monday);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsScheduledForDay_Should_Return_False_When_DayNotInSchedule()
        {
            // Arrange
            var habit = CreateTestHabit();
            var tuesday = new DateTime(2024, 1, 2); // A Tuesday
            habit.UpdateSchedule(new List<DayOfWeek> { DayOfWeek.Monday });

            // Act
            var result = habit.IsScheduledForDay(tuesday);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Deactivate_Should_Set_IsActive_To_False()
        {
            // Arrange
            var habit = CreateTestHabit();

            // Act
            habit.Deactivate();

            // Assert
            habit.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void Activate_Should_Set_IsActive_To_True()
        {
            // Arrange
            var habit = CreateTestHabit();
            habit.Deactivate();

            // Act
            habit.Activate();

            // Assert
            habit.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void AddResponse_Should_Add_To_Collection()
        {
            // Arrange
            var habit = CreateTestHabit();

            // Act
            habit.AddResponse(
                Guid.NewGuid(),
                "Go for a 30-minute run",
                "Use running shoes and track",
                30
            );

            // Assert
            habit.Responses.Count.ShouldBe(1);
            habit.Responses.First().Description.ShouldBe("Go for a 30-minute run");
        }

        [Fact]
        public void AddReward_Should_Add_To_Collection()
        {
            // Arrange
            var habit = CreateTestHabit();

            // Act
            habit.AddReward(
                Guid.NewGuid(),
                "Feel energized for the day",
                "Feeling good about myself"
            );

            // Assert
            habit.Rewards.Count.ShouldBe(1);
            habit.Rewards.First().HasCraving.ShouldBeTrue();
        }

        [Fact]
        public void AddStrategy_Should_Add_To_Collection()
        {
            // Arrange
            var habit = CreateTestHabit();

            // Act
            habit.AddStrategy(
                Guid.NewGuid(),
                HabitStrategyType.MakeCueObvious,
                "Place running shoes by the door"
            );

            // Assert
            habit.Strategies.Count.ShouldBe(1);
        }

        [Fact]
        public void RemoveCue_Should_Remove_From_Collection()
        {
            // Arrange
            var habit = CreateTestHabit();
            var cueId = Guid.NewGuid();
            habit.AddCue(cueId, CueType.Time, "Morning", "07:00");

            // Act
            habit.RemoveCue(cueId);

            // Assert
            habit.Cues.Count.ShouldBe(0);
        }

        [Fact]
        public void RemoveCue_Should_Throw_When_NotFound()
        {
            // Arrange
            var habit = CreateTestHabit();

            // Act & Assert
            Should.Throw<BusinessException>(() => 
                habit.RemoveCue(Guid.NewGuid())
            );
        }

        private Habit CreateTestHabit()
        {
            return new Habit(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Habit",
                "Test Goal",
                HabitType.Make
            );
        }
    }
}
