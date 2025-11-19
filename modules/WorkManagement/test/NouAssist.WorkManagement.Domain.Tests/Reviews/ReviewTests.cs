using System;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Reviews
{
    public class ReviewTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateReview()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var reviewType = ReviewType.Daily;
            var periodStart = DateTime.Now.AddDays(-1);
            var periodEnd = DateTime.Now;

            // Act
            var review = new Review(
                Guid.NewGuid(),
                ownerId,
                reviewType,
                periodStart,
                periodEnd);

            // Assert
            review.ShouldNotBeNull();
            review.OwnerId.ShouldBe(ownerId);
            review.ReviewType.ShouldBe(reviewType);
            review.PeriodStart.ShouldBe(periodStart);
            review.PeriodEnd.ShouldBe(periodEnd);
            review.IsComplete.ShouldBeFalse();
        }

        [Fact]
        public void Complete_Should_MarkReviewAsComplete()
        {
            // Arrange
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Daily);

            // Act
            review.Complete();

            // Assert
            review.IsComplete.ShouldBeTrue();
            review.CompletedAt.ShouldNotBeNull();
        }

        [Fact]
        public void SetScores_Should_UpdateScores()
        {
            // Arrange
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Daily);

            // Act
            review.SetScores(
                completenessScore: 8,
                satisfactionScore: 7,
                alignmentScore: 9,
                energyLevel: 6);

            // Assert
            review.CompletenessScore.ShouldBe(8);
            review.SatisfactionScore.ShouldBe(7);
            review.AlignmentScore.ShouldBe(9);
            review.EnergyLevel.ShouldBe(6);
        }

        [Fact]
        public void SetReflections_Should_UpdateReflectionFields()
        {
            // Arrange
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly);

            // Act
            review.WhatWentWell = "Completed all tasks";
            review.WhatNeedsAttention = "Need to improve time management";
            review.Insights = "Focus on priorities";
            review.LessonsLearned = "Break large tasks into smaller ones";
            review.NextPeriodFocus = "Work on strategic projects";

            // Assert
            review.WhatWentWell.ShouldBe("Completed all tasks");
            review.WhatNeedsAttention.ShouldBe("Need to improve time management");
            review.Insights.ShouldBe("Focus on priorities");
            review.LessonsLearned.ShouldBe("Break large tasks into smaller ones");
            review.NextPeriodFocus.ShouldBe("Work on strategic projects");
            review.HasReflections.ShouldBeTrue();
        }

        [Fact]
        public void AddActionItem_Should_AddItemToActionItems()
        {
            // Arrange
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly);

            // Act
            review.AddActionItem("Improve documentation", 1);

            // Assert
            review.ActionItems.Count.ShouldBe(1);
            review.HasActionItems.ShouldBeTrue();
        }

        [Fact]
        public void AddReviewItem_Should_AddItemToReviewItems()
        {
            // Arrange
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly);

            // Act
            review.AddReviewItem(ReviewItemType.Project, Guid.NewGuid(), "Project Name");

            // Assert
            review.Items.Count.ShouldBe(1);
        }

        [Fact]
        public void SetNextReviewDue_Should_UpdateNextReviewDue()
        {
            // Arrange
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly);
            var nextReviewDue = DateTime.Now.AddDays(7);

            // Act
            review.SetNextReviewDue(nextReviewDue);

            // Assert
            review.NextReviewDue.ShouldBe(nextReviewDue);
        }

        [Fact]
        public void SetTimeSpent_Should_UpdateTimeSpentMinutes()
        {
            // Arrange
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly);

            // Act
            review.SetTimeSpent(45);

            // Assert
            review.TimeSpentMinutes.ShouldBe(45);
        }

        [Fact]
        public void HasPeriod_Should_ReturnTrueWhenPeriodIsSet()
        {
            // Arrange
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly,
                DateTime.Now.AddDays(-7),
                DateTime.Now);

            // Act & Assert
            review.HasPeriod.ShouldBeTrue();
        }

        [Fact]
        public void PeriodDuration_Should_CalculateCorrectDuration()
        {
            // Arrange
            var periodStart = DateTime.Now.AddDays(-7);
            var periodEnd = DateTime.Now;
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly,
                periodStart,
                periodEnd);

            // Act
            var duration = review.PeriodDuration;

            // Assert
            duration.ShouldNotBeNull();
            duration.Value.TotalDays.ShouldBe(7, tolerance: 0.1);
        }
    }
}
