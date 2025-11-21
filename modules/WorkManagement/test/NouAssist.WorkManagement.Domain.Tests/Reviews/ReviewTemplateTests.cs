using System;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Reviews
{
    public class ReviewTemplateTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Constructor_WithValidParameters_Should_CreateReviewTemplate()
        {
            // Arrange
            var title = "Weekly Review Template";
            var reviewType = ReviewType.Weekly;

            // Act
            var template = new ReviewTemplate(
                Guid.NewGuid(),
                reviewType,
                title,
                ownerId: Guid.NewGuid());

            // Assert
            template.ShouldNotBeNull();
            template.Title.ShouldBe(title);
            template.ReviewType.ShouldBe(reviewType);
        }
    }
}
