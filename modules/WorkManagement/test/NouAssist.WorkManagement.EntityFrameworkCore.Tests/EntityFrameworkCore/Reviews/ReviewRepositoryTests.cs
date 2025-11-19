using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Data;
using NouAssist.WorkManagement.Reviews.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Reviews
{
    public class ReviewRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewRepositoryTests()
        {
            _reviewRepository = GetRequiredService<IReviewRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertReview()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var review = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Daily);

                // Act
                var insertedReview = await _reviewRepository.InsertAsync(review);

                // Assert
                insertedReview.ShouldNotBeNull();
                insertedReview.Id.ShouldBe(review.Id);
                insertedReview.ReviewType.ShouldBe(ReviewType.Daily);
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnReview()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var review = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Weekly);

                await _reviewRepository.InsertAsync(review);

                var retrievedReview = await _reviewRepository.GetAsync(review.Id);

                // Assert
                retrievedReview.ShouldNotBeNull();
                retrievedReview.Id.ShouldBe(review.Id);
                retrievedReview.ReviewType.ShouldBe(ReviewType.Weekly);
            });
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateReview()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var review = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Weekly);

                await _reviewRepository.InsertAsync(review);

                review.WhatWentWell = "Updated reflection";
                await _reviewRepository.UpdateAsync(review);

                var updatedReview = await _reviewRepository.GetAsync(review.Id);

                // Assert
                updatedReview.WhatWentWell.ShouldBe("Updated reflection");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteReview()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var review = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Daily);

                await _reviewRepository.InsertAsync(review);
                await _reviewRepository.DeleteAsync(review);

                // Assert
                var deletedReview = await _reviewRepository.FindAsync(review.Id);
                deletedReview.ShouldBeNull();
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnReviews()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var review1 = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Daily);

                var review2 = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Weekly);

                await _reviewRepository.InsertAsync(review1);
                await _reviewRepository.InsertAsync(review2);

                var reviews = await _reviewRepository.GetListAsync();

                // Assert
                reviews.Count.ShouldBeGreaterThanOrEqualTo(2);
            });
        }

        [Fact]
        public async Task GetByOwnerAsync_Should_ReturnOwnerReviews()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var ownerId = Guid.NewGuid();

                var review1 = new Review(
                    Guid.NewGuid(),
                    ownerId,
                    ReviewType.Daily);

                var review2 = new Review(
                    Guid.NewGuid(),
                    ownerId,
                    ReviewType.Weekly);

                var otherReview = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Daily);

                await _reviewRepository.InsertAsync(review1);
                await _reviewRepository.InsertAsync(review2);
                await _reviewRepository.InsertAsync(otherReview);

                var ownerReviews = await _reviewRepository.GetByOwnerAsync(ownerId);

                // Assert
                ownerReviews.Count.ShouldBe(2);
                ownerReviews.ShouldAllBe(r => r.OwnerId == ownerId);
            });
        }

        [Fact]
        public async Task GetByTypeAsync_Should_ReturnFilteredReviews()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var review1 = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Daily);

                var review2 = new Review(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ReviewType.Weekly);

                await _reviewRepository.InsertAsync(review1);
                await _reviewRepository.InsertAsync(review2);

                var dailyReviews = await _reviewRepository.GetByTypeAsync(ReviewType.Daily);

                // Assert
                dailyReviews.ShouldContain(r => r.Id == review1.Id);
                dailyReviews.ShouldNotContain(r => r.Id == review2.Id);
            });
        }
    }
}
