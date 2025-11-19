using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Reviews.Reviews;
using NouAssist.WorkManagement.Reviews.Reviews.Dtos;
using Shouldly;
using Volo.Abp.Validation;
using Xunit;

namespace NouAssist.WorkManagement.Reviews
{
    public class ReviewAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IReviewAppService _reviewAppService;

        public ReviewAppServiceTests()
        {
            _reviewAppService = GetRequiredService<IReviewAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateReview()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateReviewDto
                {
                    ReviewType = Reviews.Enums.ReviewType.Daily,
                    PeriodStart = DateTime.Now.AddDays(-1),
                    PeriodEnd = DateTime.Now
                };

                // Act
                var result = await _reviewAppService.CreateAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Id.ShouldNotBe(Guid.Empty);
                result.ReviewType.ShouldBe(input.ReviewType);
            });
        }

        [Fact]
        public async Task GetAsync_WithValidId_Should_ReturnReview()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateReviewDto
                {
                    ReviewType = Reviews.Enums.ReviewType.Weekly
                };

                var createdReview = await _reviewAppService.CreateAsync(createInput);
                var retrievedReview = await _reviewAppService.GetAsync(createdReview.Id);

                // Assert
                retrievedReview.ShouldNotBeNull();
                retrievedReview.Id.ShouldBe(createdReview.Id);
                retrievedReview.ReviewType.ShouldBe(createdReview.ReviewType);
            });
        }

        [Fact]
        public async Task UpdateAsync_WithValidInput_Should_UpdateReview()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateReviewDto
                {
                    ReviewType = Reviews.Enums.ReviewType.Weekly
                };

                var createdReview = await _reviewAppService.CreateAsync(createInput);

                var updateInput = new UpdateReviewDto
                {
                    WhatWentWell = "All tasks completed",
                    WhatNeedsAttention = "Improve time management",
                    Insights = "Focus on priorities"
                };

                var updatedReview = await _reviewAppService.UpdateAsync(createdReview.Id, updateInput);

                // Assert
                updatedReview.ShouldNotBeNull();
                updatedReview.WhatWentWell.ShouldBe("All tasks completed");
                updatedReview.WhatNeedsAttention.ShouldBe("Improve time management");
                updatedReview.Insights.ShouldBe("Focus on priorities");
            });
        }

        [Fact]
        public async Task CompleteAsync_Should_MarkReviewAsComplete()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateReviewDto
                {
                    ReviewType = Reviews.Enums.ReviewType.Daily
                };

                var createdReview = await _reviewAppService.CreateAsync(createInput);
                var completedReview = await _reviewAppService.CompleteAsync(createdReview.Id);

                // Assert
                completedReview.ShouldNotBeNull();
                completedReview.IsComplete.ShouldBeTrue();
                completedReview.CompletedAt.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteReview()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateReviewDto
                {
                    ReviewType = Reviews.Enums.ReviewType.Daily
                };

                var createdReview = await _reviewAppService.CreateAsync(createInput);
                await _reviewAppService.DeleteAsync(createdReview.Id);

                // Assert
                await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(async () =>
                    await _reviewAppService.GetAsync(createdReview.Id));
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnPagedResults()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                // Create test reviews
                await _reviewAppService.CreateAsync(new CreateReviewDto
                {
                    ReviewType = Reviews.Enums.ReviewType.Daily
                });
                await _reviewAppService.CreateAsync(new CreateReviewDto
                {
                    ReviewType = Reviews.Enums.ReviewType.Weekly
                });

                var input = new GetReviewListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0
                };

                var result = await _reviewAppService.GetListAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Items.ShouldNotBeEmpty();
                result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            });
        }

        [Fact]
        public async Task SetScoresAsync_Should_UpdateScores()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateReviewDto
                {
                    ReviewType = Reviews.Enums.ReviewType.Weekly
                };

                var createdReview = await _reviewAppService.CreateAsync(createInput);

                var scoresInput = new SetReviewScoresDto
                {
                    CompletenessScore = 8,
                    SatisfactionScore = 7,
                    AlignmentScore = 9,
                    EnergyLevel = 6
                };

                var updatedReview = await _reviewAppService.SetScoresAsync(createdReview.Id, scoresInput);

                // Assert
                updatedReview.ShouldNotBeNull();
                updatedReview.CompletenessScore.ShouldBe(8);
                updatedReview.SatisfactionScore.ShouldBe(7);
                updatedReview.AlignmentScore.ShouldBe(9);
                updatedReview.EnergyLevel.ShouldBe(6);
            });
        }
    }
}
