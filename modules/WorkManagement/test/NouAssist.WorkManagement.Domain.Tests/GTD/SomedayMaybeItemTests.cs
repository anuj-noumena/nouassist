using System;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class SomedayMaybeItemTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateSomedayMaybeItem()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Learn Piano";

            // Act
            var item = new SomedayMaybeItem(
                Guid.NewGuid(),
                ownerId,
                title);

            // Assert
            item.ShouldNotBeNull();
            item.OwnerId.ShouldBe(ownerId);
            item.Title.ShouldBe(title);
            item.Status.ShouldBe(SomedayMaybeStatus.Active);
        }

        [Fact]
        public void Promote_Should_SetPromotionDetails()
        {
            // Arrange
            var item = new SomedayMaybeItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Learn Piano");
            var promotedToId = Guid.NewGuid();

            // Act
            item.Promote("Project", promotedToId);

            // Assert
            item.Status.ShouldBe(SomedayMaybeStatus.Promoted);
            item.PromotedToType.ShouldBe("Project");
            item.PromotedToId.ShouldBe(promotedToId);
            item.PromotedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Archive_Should_ChangeStatusToArchived()
        {
            // Arrange
            var item = new SomedayMaybeItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Learn Piano");

            // Act
            item.Archive();

            // Assert
            item.Status.ShouldBe(SomedayMaybeStatus.Archived);
            item.ArchivedAt.ShouldNotBeNull();
        }

        [Fact]
        public void MarkReviewed_Should_UpdateReviewDetails()
        {
            // Arrange
            var item = new SomedayMaybeItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Learn Piano");
            var initialReviewCount = item.ReviewCount;

            // Act
            item.MarkReviewed();

            // Assert
            item.LastReviewedAt.ShouldNotBeNull();
            item.ReviewCount.ShouldBe(initialReviewCount + 1);
        }

        [Fact]
        public void UpdateImportanceAndUrgency_Should_UpdateScores()
        {
            // Arrange
            var item = new SomedayMaybeItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Learn Piano");

            // Act
            item.UpdateImportance(0.8);
            item.UpdateUrgency(0.6);

            // Assert
            item.Importance.Value.ShouldBe(0.8, tolerance: 0.01);
            item.Urgency.Value.ShouldBe(0.6, tolerance: 0.01);
        }
    }
}
