using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Data;
using NouAssist.WorkManagement.Reviews.Entities;
using NouAssist.WorkManagement.Reviews.Progress.Dtos;
using NouAssist.WorkManagement.Reviews.Reviews.Dtos;
using NouAssist.WorkManagement.Reviews.Services;
using NouAssist.WorkManagement.Reviews.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;
using Volo.Abp.Users;

namespace NouAssist.WorkManagement.Reviews.Reviews
{
    [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.ViewOwn)]
    public class ReviewAppService : ApplicationService, IReviewAppService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewItemRepository _reviewItemRepository;
        private readonly IReviewProgressRepository _progressRepository;
        private readonly ReviewProcessingService _processingService;

        public ReviewAppService(
            IReviewRepository reviewRepository,
            IReviewItemRepository reviewItemRepository,
            IReviewProgressRepository progressRepository,
            ReviewProcessingService processingService)
        {
            _reviewRepository = reviewRepository;
            _reviewItemRepository = reviewItemRepository;
            _progressRepository = progressRepository;
            _processingService = processingService;
        }

        public async Task<ReviewDto> GetAsync(Guid id)
        {
            var review = await _reviewRepository.GetWithDetailsAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to view this review");
            }

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        public async Task<PagedResultDto<ReviewDto>> GetListAsync(GetReviewListDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var specification = CreateListSpecification(input, ownerId);
            
            var reviews = await _reviewRepository.GetListAsync(specification);
            var totalCount = reviews.Count;

            var pagedReviews = reviews
                .OrderByDescending(r => r.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var dtos = ObjectMapper.Map<List<Review>, List<ReviewDto>>(pagedReviews);

            return new PagedResultDto<ReviewDto>(totalCount, dtos);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Create)]
        public async Task<ReviewDto> CreateAsync(CreateReviewDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var review = new Review(
                GuidGenerator.Create(),
                ownerId,
                input.ReviewType,
                null, // horizonLevels - can be extended
                input.PeriodStart,
                input.PeriodEnd,
                CurrentTenant.Id
            );

            await _reviewRepository.InsertAsync(review);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var review = await _reviewRepository.GetAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to delete this review");
            }

            if (review.IsComplete)
            {
                throw new BusinessException("Reviews:CannotDeleteCompleted", 
                    "Cannot delete a completed review");
            }

            await _reviewRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Edit)]
        public async Task<ReviewDto> UpdateReflectionsAsync(Guid id, UpdateReviewReflectionsDto input)
        {
            var review = await _reviewRepository.GetAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this review");
            }

            review.UpdateReflections(
                input.WhatWentWell,
                input.WhatNeedsAttention,
                input.Insights,
                input.LessonsLearned
            );

            if (!string.IsNullOrWhiteSpace(input.NextPeriodFocus))
            {
                review.NextPeriodFocus = input.NextPeriodFocus;
            }

            await _reviewRepository.UpdateAsync(review);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Edit)]
        public async Task<ReviewDto> SetScoresAsync(Guid id, SetReviewScoresDto input)
        {
            var review = await _reviewRepository.GetAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this review");
            }

            review.SetScores(
                input.CompletenessScore,
                input.SatisfactionScore,
                input.AlignmentScore,
                input.EnergyLevel
            );

            await _reviewRepository.UpdateAsync(review);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Complete)]
        public async Task<ReviewDto> CompleteAsync(Guid id, CompleteReviewDto input)
        {
            var review = await _reviewRepository.GetAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to complete this review");
            }

            review.Complete(
                input.CompletenessScore,
                input.SatisfactionScore,
                input.AlignmentScore
            );

            await _reviewRepository.UpdateAsync(review);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Edit)]
        public async Task<ReviewDto> RecordTimeSpentAsync(Guid id, RecordTimeSpentDto input)
        {
            var review = await _reviewRepository.GetAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this review");
            }

            review.RecordTimeSpent(input.Minutes);

            await _reviewRepository.UpdateAsync(review);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Edit)]
        public async Task<ReviewDto> AddActionItemAsync(Guid id, AddReviewActionItemDto input)
        {
            var review = await _reviewRepository.GetAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this review");
            }

            review.AddActionItem(input.Description, input.AssignedTo, input.DueBy);

            await _reviewRepository.UpdateAsync(review);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Edit)]
        public async Task<ReviewDto> CompleteActionItemAsync(Guid reviewId, int actionItemIndex)
        {
            var review = await _reviewRepository.GetAsync(reviewId);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this review");
            }

            review.CompleteActionItem(actionItemIndex);

            await _reviewRepository.UpdateAsync(review);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Edit)]
        public async Task<ReviewDto> AddReviewItemAsync(Guid id, AddReviewItemDto input)
        {
            var review = await _reviewRepository.GetWithDetailsAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this review");
            }

            review.AddReviewItem(
                GuidGenerator.Create(),
                input.ItemType,
                input.EntityId,
                input.ItemTitle
            );

            await _reviewRepository.UpdateAsync(review);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        [Authorize(WorkManagementPermissions.Reviews.ReviewManagement.Edit)]
        public async Task<ReviewItemDto> ReviewItemAsync(Guid reviewId, Guid itemId, ReviewItemDecisionDto input)
        {
            var review = await _reviewRepository.GetWithDetailsAsync(reviewId);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this review");
            }

            review.ReviewItem(itemId, input.Decision, input.Notes, input.ActionTaken);

            await _reviewRepository.UpdateAsync(review);

            // Update progress
            await _processingService.UpdateProgressAsync(review);

            var item = review.Items.First(i => i.Id == itemId);
            return ObjectMapper.Map<ReviewItem, ReviewItemDto>(item);
        }

        public async Task<PagedResultDto<ReviewItemDto>> GetReviewItemsAsync(Guid reviewId, bool? onlyUnreviewed = null)
        {
            var review = await _reviewRepository.GetAsync(reviewId);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to view this review");
            }

            List<ReviewItem> items;
            if (onlyUnreviewed == true)
            {
                items = await _reviewItemRepository.GetUnreviewedItemsAsync(reviewId);
            }
            else
            {
                items = await _reviewItemRepository.GetByReviewIdAsync(reviewId);
            }

            var dtos = ObjectMapper.Map<List<ReviewItem>, List<ReviewItemDto>>(items);

            return new PagedResultDto<ReviewItemDto>(dtos.Count, dtos);
        }

        public async Task<ReviewProgressDto> GetProgressAsync(Guid reviewId)
        {
            var review = await _reviewRepository.GetAsync(reviewId);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to view this review");
            }

            var progress = await _processingService.UpdateProgressAsync(review);
            
            if (progress.Id == Guid.Empty)
            {
                await _progressRepository.InsertAsync(progress);
            }
            else
            {
                await _progressRepository.UpdateAsync(progress);
            }

            return ObjectMapper.Map<ReviewProgress, ReviewProgressDto>(progress);
        }

        [Authorize(WorkManagementPermissions.Reviews.Analytics.ViewOwn)]
        public async Task<ReviewQualityAnalysisDto> GetQualityAnalysisAsync(Guid id)
        {
            var review = await _reviewRepository.GetAsync(id);
            
            if (review.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to view this review");
            }

            var analysis = _processingService.AnalyzeReviewQuality(review);

            return ObjectMapper.Map<ReviewQualityAnalysis, ReviewQualityAnalysisDto>(analysis);
        }

        // Private helper methods
        private ISpecification<Review> CreateListSpecification(GetReviewListDto input, Guid ownerId)
        {
            ISpecification<Review> spec = new BaseSpecification<Review>();

            spec = spec.And(new BaseSpecification<Review>(r => r.OwnerId == ownerId));

            if (input.ReviewType.HasValue)
            {
                spec = spec.And(new ReviewsByTypeSpecification(input.ReviewType.Value));
            }

            if (input.IsComplete.HasValue)
            {
                if (input.IsComplete.Value)
                {
                    spec = spec.And(new CompletedReviewsSpecification());
                }
                else
                {
                    spec = spec.And(new IncompleteReviewsSpecification());
                }
            }

            if (input.FromDate.HasValue)
            {
                spec = spec.And(new BaseSpecification<Review>(r => r.CreationTime >= input.FromDate.Value));
            }

            if (input.ToDate.HasValue)
            {
                spec = spec.And(new BaseSpecification<Review>(r => r.CreationTime <= input.ToDate.Value));
            }

            return spec;
        }
    }
}
