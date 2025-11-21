using System;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class GtdHorizonTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateGtdHorizon()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "Life Purpose";
            var horizonLevel = GtdHorizonLevel.Horizon5Purpose;

            // Act
            var horizon = new GtdHorizon(
                Guid.NewGuid(),
                ownerId,
                horizonLevel,
                title);

            // Assert
            horizon.ShouldNotBeNull();
            horizon.OwnerId.ShouldBe(ownerId);
            horizon.Title.ShouldBe(title);
            horizon.HorizonLevel.ShouldBe(horizonLevel);
            horizon.IsActive.ShouldBeTrue();
            horizon.IsDraft.ShouldBeTrue();
        }

        [Fact]
        public void Publish_Should_ChangeIsDraftToFalse()
        {
            // Arrange
            var horizon = new GtdHorizon(
                Guid.NewGuid(),
                Guid.NewGuid(),
                GtdHorizonLevel.Horizon5Purpose,
                "Life Purpose");

            // Act
            horizon.Publish();

            // Assert
            horizon.IsDraft.ShouldBeFalse();
        }

        [Fact]
        public void Deactivate_Should_SetIsActiveToFalse()
        {
            // Arrange
            var horizon = new GtdHorizon(
                Guid.NewGuid(),
                Guid.NewGuid(),
                GtdHorizonLevel.Horizon5Purpose,
                "Life Purpose");

            // Act
            horizon.IsActive = false;

            // Assert
            horizon.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void AddCoreValue_Should_AddValueToList()
        {
            // Arrange
            var horizon = new GtdHorizon(
                Guid.NewGuid(),
                Guid.NewGuid(),
                GtdHorizonLevel.Horizon5Purpose,
                "Life Purpose");

            // Act
            horizon.AddCoreValue("Integrity");

            // Assert
            horizon.CoreValues.Count.ShouldBe(1);
            horizon.CoreValues.ShouldContain("Integrity");
        }

        [Fact]
        public void RemoveCoreValue_Should_RemoveValueFromList()
        {
            // Arrange
            var horizon = new GtdHorizon(
                Guid.NewGuid(),
                Guid.NewGuid(),
                GtdHorizonLevel.Horizon5Purpose,
                "Life Purpose");
            horizon.AddCoreValue("Integrity");

            // Act
            horizon.CoreValues.Remove("Integrity");

            // Assert
            horizon.CoreValues.Count.ShouldBe(0);
        }

        [Fact]
        public void MarkReviewed_Should_UpdateReviewDetails()
        {
            // Arrange
            var horizon = new GtdHorizon(
                Guid.NewGuid(),
                Guid.NewGuid(),
                GtdHorizonLevel.Horizon5Purpose,
                "Life Purpose");
            var initialReviewCount = horizon.ReviewCount;

            // Act
            horizon.MarkAsReviewed();

            // Assert
            horizon.LastReviewedAt.ShouldNotBeNull();
            horizon.ReviewCount.ShouldBe(initialReviewCount + 1);
        }

        [Fact]
        public void SetReviewFrequency_Should_UpdateReviewFrequency()
        {
            // Arrange
            var horizon = new GtdHorizon(
                Guid.NewGuid(),
                Guid.NewGuid(),
                GtdHorizonLevel.Horizon5Purpose,
                "Life Purpose");

            // Act
            horizon.UpdateReviewFrequency(HorizonReviewFrequency.Annually);

            // Assert
            horizon.ReviewFrequency.ShouldBe(HorizonReviewFrequency.Annually);
        }
    }
}
