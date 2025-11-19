using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class RecurrenceRuleTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateRecurrenceRule()
        {
            // Act
            var rule = new RecurrenceRule(RecurrenceFrequency.Daily, interval: 1);

            // Assert
            rule.Frequency.ShouldBe(RecurrenceFrequency.Daily);
            rule.Interval.ShouldBe(1);
            rule.EndDate.ShouldBeNull();
            rule.Count.ShouldBeNull();
            rule.DaysOfWeek.ShouldNotBeNull();
        }

        [Fact]
        public void Constructor_WithIntervalLessThanOne_Should_ThrowException()
        {
            // Act & Assert
            Should.Throw<BusinessException>(() =>
                new RecurrenceRule(RecurrenceFrequency.Daily, interval: 0))
                .Code.ShouldBe("Tasks:InvalidRecurrenceInterval");
        }

        [Fact]
        public void Constructor_WithBothEndDateAndCount_Should_ThrowException()
        {
            // Act & Assert
            Should.Throw<BusinessException>(() =>
                new RecurrenceRule(
                    RecurrenceFrequency.Daily,
                    interval: 1,
                    endDate: DateTime.Now.AddDays(30),
                    count: 10))
                .Code.ShouldBe("Tasks:CannotHaveBothEndDateAndCount");
        }

        [Fact]
        public void Constructor_WithEndDate_Should_CreateRecurrenceRuleWithEndDate()
        {
            // Arrange
            var endDate = DateTime.Now.AddDays(30);

            // Act
            var rule = new RecurrenceRule(
                RecurrenceFrequency.Daily,
                interval: 1,
                endDate: endDate);

            // Assert
            rule.EndDate.ShouldBe(endDate);
            rule.Count.ShouldBeNull();
        }

        [Fact]
        public void Constructor_WithCount_Should_CreateRecurrenceRuleWithCount()
        {
            // Act
            var rule = new RecurrenceRule(
                RecurrenceFrequency.Daily,
                interval: 1,
                count: 10);

            // Assert
            rule.Count.ShouldBe(10);
            rule.EndDate.ShouldBeNull();
        }

        [Fact]
        public void Constructor_WithDaysOfWeek_Should_CreateRecurrenceRuleWithDaysOfWeek()
        {
            // Arrange
            var daysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };

            // Act
            var rule = new RecurrenceRule(
                RecurrenceFrequency.Weekly,
                interval: 1,
                daysOfWeek: daysOfWeek);

            // Assert
            rule.DaysOfWeek.ShouldBe(daysOfWeek);
        }

        [Fact]
        public void Constructor_WithoutDaysOfWeek_Should_CreateEmptyDaysOfWeekList()
        {
            // Act
            var rule = new RecurrenceRule(RecurrenceFrequency.Daily, interval: 1);

            // Assert
            rule.DaysOfWeek.ShouldNotBeNull();
            rule.DaysOfWeek.Count.ShouldBe(0);
        }

        [Fact]
        public void Equality_WithSameValues_Should_BeEqual()
        {
            // Arrange
            var rule1 = new RecurrenceRule(RecurrenceFrequency.Daily, interval: 1);
            var rule2 = new RecurrenceRule(RecurrenceFrequency.Daily, interval: 1);

            // Act & Assert
            rule1.Equals(rule2).ShouldBeTrue();
        }

        [Fact]
        public void Equality_WithDifferentFrequency_Should_NotBeEqual()
        {
            // Arrange
            var rule1 = new RecurrenceRule(RecurrenceFrequency.Daily, interval: 1);
            var rule2 = new RecurrenceRule(RecurrenceFrequency.Weekly, interval: 1);

            // Act & Assert
            rule1.Equals(rule2).ShouldBeFalse();
        }

        [Fact]
        public void Equality_WithDifferentInterval_Should_NotBeEqual()
        {
            // Arrange
            var rule1 = new RecurrenceRule(RecurrenceFrequency.Daily, interval: 1);
            var rule2 = new RecurrenceRule(RecurrenceFrequency.Daily, interval: 2);

            // Act & Assert
            rule1.Equals(rule2).ShouldBeFalse();
        }
    }
}
