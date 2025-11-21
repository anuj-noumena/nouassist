using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;
using DayOfWeek = NouAssist.PersonalDevelopment.Habits.Enums.DayOfWeek;

namespace NouAssist.PersonalDevelopment.Habits
{
    public class HabitTests : PersonalDevelopmentDomainTestBase<PersonalDevelopmentDomainTestModule>
    {
        private readonly Guid _userId = Guid.NewGuid();

        [Fact]
        public void Should_Create_Habit_With_Valid_Data()
        {
            // Arrange & Act
            var habit = new Habit(
                Guid.NewGuid(),
                _userId,
                "Exercise Daily",
                "Stay healthy and fit",
                HabitType.Make
            );

            // Assert
            habit.ShouldNotBeNull();
            habit.Title.ShouldBe("Exercise Daily");
            habit.Goal.ShouldBe("Stay healthy and fit");
            habit.OwnerId.ShouldBe(_userId);
            habit.HabitType.ShouldBe(HabitType.Make);
            habit.IsActive.ShouldBeTrue();
            habit.IsMakeHabit.ShouldBeTrue();
            habit.IsBreakHabit.ShouldBeFalse();
        }

        [Fact]
        public void Should_Create_Break_Habit()
        {
            // Arrange & Act
            var habit = new Habit(
                Guid.NewGuid(),
                _userId,
                "Stop Smoking",
                "Improve health",
                HabitType.Break
            );

            // Assert
            habit.HabitType.ShouldBe(HabitType.Break);
            habit.IsBreakHabit.ShouldBeTrue();
            habit.IsMakeHabit.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Create_Habit_With_Empty_Title()
        {
            // Assert
            Should.Throw<ArgumentException>(() =>
                new Habit(Guid.NewGuid(), _userId, "", "Goal", HabitType.Make));
        }

        [Fact]
        public void Should_Not_Create_Habit_With_Empty_Goal()
        {
            // Assert
            Should.Throw<ArgumentException>(() =>
                new Habit(Guid.NewGuid(), _userId, "Title", "", HabitType.Make));
        }

        [Fact]
        public void Should_Update_Schedule_Successfully()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var schedule = new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Wednesday,
                DayOfWeek.Friday
            };

            // Act
            habit.UpdateSchedule(schedule);

            // Assert
            habit.ScheduledDays.Count.ShouldBe(3);
            habit.HasSchedule.ShouldBeTrue();
            habit.IsEveryday.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Allow_More_Than_7_Days_In_Schedule()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var schedule = new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday,
                DayOfWeek.Monday // Duplicate, but still 8 items
            };

            // Assert
            Should.Throw<BusinessException>(() => habit.UpdateSchedule(schedule));
        }

        [Fact]
        public void Should_Remove_Duplicate_Days_In_Schedule()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var schedule = new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday
            };

            // Act
            habit.UpdateSchedule(schedule);

            // Assert
            habit.ScheduledDays.Count.ShouldBe(2);
        }

        [Fact]
        public void Should_Set_Everyday_Schedule()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Meditate", "Peace of mind", HabitType.Make);
            var schedule = new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            // Act
            habit.UpdateSchedule(schedule);

            // Assert
            habit.IsEveryday.ShouldBeTrue();
            habit.ScheduledDays.Count.ShouldBe(7);
        }

        [Fact]
        public void Should_Check_If_Scheduled_For_Specific_Day()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            habit.UpdateSchedule(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday });

            // Get a Monday
            var monday = DateTime.UtcNow;
            while (monday.DayOfWeek != (System.DayOfWeek)1) // Monday
            {
                monday = monday.AddDays(1);
            }

            // Get a Tuesday
            var tuesday = monday.AddDays(1);

            // Assert
            habit.IsScheduledForDay(monday).ShouldBeTrue();
            habit.IsScheduledForDay(tuesday).ShouldBeFalse();
        }

        [Fact]
        public void Should_Activate_Habit()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            habit.Deactivate();

            // Act
            habit.Activate();

            // Assert
            habit.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void Should_Deactivate_Habit()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);

            // Act
            habit.Deactivate();

            // Assert
            habit.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void Should_Add_Cue_Successfully()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var cueId = Guid.NewGuid();

            // Act
            habit.AddCue(cueId, CueType.Time, "7:00 AM every morning", "07:00");

            // Assert
            habit.Cues.Count.ShouldBe(1);
            habit.Cues.First().CueType.ShouldBe(CueType.Time);
        }

        [Fact]
        public void Should_Not_Add_Duplicate_Cue()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var cueId = Guid.NewGuid();
            habit.AddCue(cueId, CueType.Time, "7:00 AM", "07:00");

            // Assert
            Should.Throw<BusinessException>(() =>
                habit.AddCue(cueId, CueType.Location, "Gym", null));
        }

        [Fact]
        public void Should_Remove_Cue_Successfully()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var cueId = Guid.NewGuid();
            habit.AddCue(cueId, CueType.Time, "7:00 AM", "07:00");

            // Act
            habit.RemoveCue(cueId);

            // Assert
            habit.Cues.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Throw_When_Removing_Non_Existent_Cue()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);

            // Assert
            Should.Throw<BusinessException>(() => habit.RemoveCue(Guid.NewGuid()));
        }

        [Fact]
        public void Should_Add_Response_Successfully()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var responseId = Guid.NewGuid();

            // Act
            habit.AddResponse(responseId, "30 minutes of jogging", "In the park", 30);

            // Assert
            habit.Responses.Count.ShouldBe(1);
            habit.Responses.First().Description.ShouldBe("30 minutes of jogging");
        }

        [Fact]
        public void Should_Not_Add_Duplicate_Response()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var responseId = Guid.NewGuid();
            habit.AddResponse(responseId, "Jogging", null, 30);

            // Assert
            Should.Throw<BusinessException>(() =>
                habit.AddResponse(responseId, "Running", null, 45));
        }

        [Fact]
        public void Should_Add_Reward_Successfully()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var rewardId = Guid.NewGuid();

            // Act
            habit.AddReward(rewardId, "Feel energized", "Feel more energetic throughout the day");

            // Assert
            habit.Rewards.Count.ShouldBe(1);
            habit.Rewards.First().Description.ShouldBe("Feel energized");
        }

        [Fact]
        public void Should_Not_Add_Duplicate_Reward()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var rewardId = Guid.NewGuid();
            habit.AddReward(rewardId, "Feel good", null);

            // Assert
            Should.Throw<BusinessException>(() =>
                habit.AddReward(rewardId, "Feel great", null));
        }

        [Fact]
        public void Should_Add_Strategy_Successfully()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var strategyId = Guid.NewGuid();

            // Act
            habit.AddStrategy(strategyId, HabitStrategyType.MakeCueObvious,
                "If it's 7 AM, then I will go for a jog");

            // Assert
            habit.Strategies.Count.ShouldBe(1);
            habit.Strategies.First().StrategyType.ShouldBe(HabitStrategyType.MakeCueObvious);
        }

        [Fact]
        public void Should_Update_Title_Successfully()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);

            // Act
            habit.UpdateTitle("Daily Exercise Routine");

            // Assert
            habit.Title.ShouldBe("Daily Exercise Routine");
        }

        [Fact]
        public void Should_Update_Goal_Successfully()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);

            // Act
            habit.UpdateGoal("Build strength and endurance");

            // Assert
            habit.Goal.ShouldBe("Build strength and endurance");
        }

        [Fact]
        public void Should_Assign_To_Area()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var areaId = Guid.NewGuid();

            // Act
            habit.AssignToArea(areaId);

            // Assert
            habit.AreaId.ShouldBe(areaId);
        }

        [Fact]
        public void Should_Assign_To_Context()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);
            var contextId = Guid.NewGuid();

            // Act
            habit.AssignToContext(contextId);

            // Assert
            habit.ContextId.ShouldBe(contextId);
        }

        [Fact]
        public void Should_Default_To_Everyday_When_No_Schedule()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), _userId, "Exercise", "Stay fit", HabitType.Make);

            // Assert
            habit.HasSchedule.ShouldBeFalse();
            // Without a schedule, isScheduledForDay should return true (everyday)
            habit.IsScheduledForDay(DateTime.UtcNow).ShouldBeTrue();
        }
    }
}
