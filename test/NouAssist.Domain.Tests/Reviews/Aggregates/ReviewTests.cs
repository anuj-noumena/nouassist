using System;
using System.Linq;
using NouAssist.Reviews.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Reviews.Aggregates
{
    public class ReviewTests
    {
        [Fact]
        public void Create_Review_Should_Set_Properties_Correctly()
        {
            // Arrange & Act
            var review = new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly
            );

            // Assert
            review.ReviewType.ShouldBe(ReviewType.Weekly);
            review.IsComplete.ShouldBeFalse();
            review.HasPeriod.ShouldBeTrue();
        }

        [Fact]
        public void AddActionItem_Should_Add_To_Collection()
        {
            // Arrange
            var review = CreateTestReview();

            // Act
            review.AddActionItem("Review all projects", "John Doe", DateTime.Now.AddDays(7));

            // Assert
            review.ActionItems.Count.ShouldBe(1);
            review.HasActionItems.ShouldBeTrue();
        }

        [Fact]
        public void CompleteActionItem_Should_Mark_As_Completed()
        {
            // Arrange
            var review = CreateTestReview();
            review.AddActionItem("Review all projects");

            // Act
            review.CompleteActionItem(0);

            // Assert
            review.ActionItems.First().IsCompleted.ShouldBeTrue();
            review.CompletedActionItems.ShouldBe(1);
        }

        [Fact]
        public void CompleteActionItem_Should_Throw_When_Invalid_Index()
        {
            // Arrange
            var review = CreateTestReview();

            // Act & Assert
            Should.Throw<BusinessException>(() => review.CompleteActionItem(0));
        }

        [Fact]
        public void Complete_Should_Set_IsComplete_True()
        {
            // Arrange
            var review = CreateTestReview();

            // Act
            review.Complete(90, 85, 80);

            // Assert
            review.IsComplete.ShouldBeTrue();
            review.CompletedAt.ShouldNotBeNull();
            review.CompletenessScore.ShouldBe(90);
            review.NextReviewDue.ShouldNotBeNull();
        }

        [Fact]
        public void Complete_Should_Throw_When_Already_Completed()
        {
            // Arrange
            var review = CreateTestReview();
            review.Complete();

            // Act & Assert
            Should.Throw<BusinessException>(() => review.Complete());
        }

        [Fact]
        public void UpdateReflections_Should_Update_Properties()
        {
            // Arrange
            var review = CreateTestReview();

            // Act
            review.UpdateReflections(
                whatWentWell: "Good progress on projects",
                whatNeedsAttention: "Need to focus on health",
                insights: "Morning routine needs adjustment"
            );

            // Assert
            review.WhatWentWell.ShouldBe("Good progress on projects");
            review.WhatNeedsAttention.ShouldBe("Need to focus on health");
            review.Insights.ShouldBe("Morning routine needs adjustment");
            review.HasReflections.ShouldBeTrue();
        }

        [Fact]
        public void SetScores_Should_Update_All_Scores()
        {
            // Arrange
            var review = CreateTestReview();

            // Act
            review.SetScores(
                completeness: 85,
                satisfaction: 90,
                alignment: 80,
                energy: 7
            );

            // Assert
            review.CompletenessScore.ShouldBe(85);
            review.SatisfactionScore.ShouldBe(90);
            review.AlignmentScore.ShouldBe(80);
            review.EnergyLevel.ShouldBe(7);
            review.HasScores.ShouldBeTrue();
        }

        [Fact]
        public void SetScores_Should_Throw_When_Invalid_Score()
        {
            // Arrange
            var review = CreateTestReview();

            // Act & Assert
            Should.Throw<BusinessException>(() => 
                review.SetScores(completeness: 150)
            );
        }

        [Fact]
        public void RecordTimeSpent_Should_Accumulate_Minutes()
        {
            // Arrange
            var review = CreateTestReview();

            // Act
            review.RecordTimeSpent(30);
            review.RecordTimeSpent(45);

            // Assert
            review.TimeSpentMinutes.ShouldBe(75);
        }

        [Fact]
        public void Quality_Should_Return_Correct_Level()
        {
            // Arrange
            var review = CreateTestReview();

            // Act & Assert - Incomplete
            review.Quality.ShouldBe(ReviewQuality.Incomplete);

            // Act & Assert - Excellent
            review.SetScores(completeness: 95);
            review.Quality.ShouldBe(ReviewQuality.Excellent);

            // Act & Assert - Good
            review.SetScores(completeness: 80);
            review.Quality.ShouldBe(ReviewQuality.Good);

            // Act & Assert - Adequate
            review.SetScores(completeness: 65);
            review.Quality.ShouldBe(ReviewQuality.Adequate);
        }

        [Fact]
        public void CalculatedCompletenessScore_Should_Calculate_Based_On_Filled_Fields()
        {
            // Arrange
            var review = CreateTestReview();

            // Act
            review.UpdateReflections(
                whatWentWell: "Something",
                whatNeedsAttention: "Something",
                insights: "Something"
            );
            review.SetScores(completeness: 90, satisfaction: 85);
            review.AddActionItem("Do something");

            // Assert
            review.CalculatedCompleteness.ShouldBeGreaterThan(0);
            review.CalculatedCompletenessScore.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void AddReviewItem_Should_Add_To_Collection()
        {
            // Arrange
            var review = CreateTestReview();
            var itemId = Guid.NewGuid();

            // Act
            review.AddReviewItem(itemId, ReviewItemType.Task, Guid.NewGuid(), "Test Task");

            // Assert
            review.Items.Count.ShouldBe(1);
            review.TotalReviewItems.ShouldBe(1);
            review.RemainingItems.ShouldBe(1);
        }

        [Fact]
        public void ReviewItem_Should_Mark_As_Reviewed()
        {
            // Arrange
            var review = CreateTestReview();
            var itemId = Guid.NewGuid();
            review.AddReviewItem(itemId, ReviewItemType.Task, Guid.NewGuid(), "Test Task");

            // Act
            review.ReviewItem(itemId, ReviewItemDecision.Complete, "Done", "Completed successfully");

            // Assert
            review.ReviewedItems.ShouldBe(1);
            review.RemainingItems.ShouldBe(0);
        }

        [Fact]
        public void ActionItemsCompletionRate_Should_Calculate_Correctly()
        {
            // Arrange
            var review = CreateTestReview();
            review.AddActionItem("Task 1");
            review.AddActionItem("Task 2");
            review.AddActionItem("Task 3");
            review.CompleteActionItem(0);
            review.CompleteActionItem(1);

            // Act
            var rate = review.ActionItemsCompletionRate;

            // Assert
            rate.ShouldBe(2.0 / 3.0, 0.01);
        }

        private Review CreateTestReview()
        {
            return new Review(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ReviewType.Weekly
            );
        }
    }
}
