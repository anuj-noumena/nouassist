using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Reviews.Aggregates;
using NouAssist.Reviews.Data;
using NouAssist.Reviews.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Reviews.Services
{
    public class ReviewProcessingService : DomainService
    {
        private readonly IReviewItemRepository _reviewItemRepository;
        private readonly IReviewProgressRepository _progressRepository;

        public ReviewProcessingService(
            IReviewItemRepository reviewItemRepository,
            IReviewProgressRepository progressRepository
        )
        {
            _reviewItemRepository = reviewItemRepository;
            _progressRepository = progressRepository;
        }

        public async Task<ReviewProgress> UpdateProgressAsync(Review review)
        {
            var items = await _reviewItemRepository.GetByReviewIdAsync(review.Id);
            var reviewedCount = items.Count(i => i.IsReviewed);
            
            var progress = await _progressRepository.GetByReviewIdAsync(review.Id);
            
            if (progress == null)
            {
                progress = new ReviewProgress(
                    GuidGenerator.Create(),
                    review.Id,
                    review.OwnerId,
                    items.Count,
                    reviewedCount,
                    review.TenantId
                );
            }
            else
            {
                progress.UpdateProgress(reviewedCount, items.Count);
            }

            return progress;
        }

        public ReviewQualityAnalysis AnalyzeReviewQuality(Review review)
        {
            var analysis = new ReviewQualityAnalysis();

            // Check completeness
            analysis.HasReflections = review.HasReflections;
            analysis.HasScores = review.HasScores;
            analysis.HasActionItems = review.HasActionItems;
            analysis.CompletenessScore = review.CalculatedCompletenessScore;

            // Check depth
            analysis.ReflectionDepth = CalculateReflectionDepth(review);
            analysis.ActionItemQuality = CalculateActionItemQuality(review);

            // Overall quality
            analysis.OverallQuality = review.Quality;
            analysis.QualityScore = CalculateQualityScore(review);

            // Recommendations
            analysis.Recommendations = GenerateRecommendations(review, analysis);

            return analysis;
        }

        private int CalculateReflectionDepth(Review review)
        {
            int depth = 0;

            if (!string.IsNullOrWhiteSpace(review.WhatWentWell) && review.WhatWentWell.Length > 50)
                depth += 25;

            if (!string.IsNullOrWhiteSpace(review.WhatNeedsAttention) && review.WhatNeedsAttention.Length > 50)
                depth += 25;

            if (!string.IsNullOrWhiteSpace(review.Insights) && review.Insights.Length > 50)
                depth += 25;

            if (!string.IsNullOrWhiteSpace(review.LessonsLearned) && review.LessonsLearned.Length > 50)
                depth += 25;

            return depth;
        }

        private int CalculateActionItemQuality(Review review)
        {
            if (!review.HasActionItems) return 0;

            int quality = 0;
            var items = review.ActionItems.ToList();

            // Items with due dates are higher quality
            var withDates = items.Count(i => i.HasDueDate);
            quality += (int)((double)withDates / items.Count * 40);

            // Items with assignments are higher quality
            var assigned = items.Count(i => i.IsAssigned);
            quality += (int)((double)assigned / items.Count * 30);

            // Completed items show follow-through
            var completed = items.Count(i => i.IsCompleted);
            quality += (int)((double)completed / items.Count * 30);

            return quality;
        }

        private int CalculateQualityScore(Review review)
        {
            int score = 0;

            // Completeness (40 points)
            score += (int)(review.CalculatedCompleteness * 40);

            // Scores provided (20 points)
            if (review.CompletenessScore.HasValue) score += 7;
            if (review.SatisfactionScore.HasValue) score += 7;
            if (review.AlignmentScore.HasValue) score += 6;

            // Reflection depth (20 points)
            score += (int)(CalculateReflectionDepth(review) * 0.2);

            // Action items (20 points)
            score += (int)(CalculateActionItemQuality(review) * 0.2);

            return Math.Min(score, 100);
        }

        private List<string> GenerateRecommendations(Review review, ReviewQualityAnalysis analysis)
        {
            var recommendations = new List<string>();

            if (!review.HasReflections)
            {
                recommendations.Add("Add reflections on what went well and what needs attention");
            }

            if (!review.HasScores)
            {
                recommendations.Add("Rate your completeness, satisfaction, and alignment");
            }

            if (!review.HasActionItems)
            {
                recommendations.Add("Create action items based on your insights");
            }

            if (review.HasActionItems && analysis.ActionItemQuality < 50)
            {
                recommendations.Add("Add due dates and assignments to your action items");
            }

            if (analysis.ReflectionDepth < 50)
            {
                recommendations.Add("Provide more detailed reflections for deeper insights");
            }

            if (review.TimeSpentMinutes.HasValue && review.TimeSpentMinutes.Value < 15)
            {
                recommendations.Add("Consider spending more time on your review for better results");
            }

            return recommendations;
        }
    }

    public class ReviewQualityAnalysis
    {
        public bool HasReflections { get; set; }
        public bool HasScores { get; set; }
        public bool HasActionItems { get; set; }
        public int CompletenessScore { get; set; }
        public int ReflectionDepth { get; set; }
        public int ActionItemQuality { get; set; }
        public ReviewQuality OverallQuality { get; set; }
        public int QualityScore { get; set; }
        public List<string> Recommendations { get; set; } = new();
    }
}
