using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Specifications;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Services
{
    /// <summary>
    /// Domain service for aggregating and analyzing insights from customer interviews
    /// </summary>
    public class InsightAggregationService : DomainService, IInsightAggregationService
    {
        private readonly ICustomerInterviewRepository _interviewRepository;
        private readonly ICustomerSegmentRepository _segmentRepository;
        private readonly ILogger<InsightAggregationService> _logger;

        public InsightAggregationService(
            ICustomerInterviewRepository interviewRepository,
            ICustomerSegmentRepository segmentRepository,
            ILogger<InsightAggregationService> logger)
        {
            _interviewRepository = interviewRepository;
            _segmentRepository = segmentRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, List<Insight>>> AggregateInsightsByCategoryAsync(
            List<CustomerInterview> interviews)
        {
            try
            {
                var result = new Dictionary<string, List<Insight>>();

                foreach (var interview in interviews)
                {
                    foreach (var insight in interview.Insights)
                    {
                        if (!result.ContainsKey(insight.Category))
                        {
                            result[insight.Category] = new List<Insight>();
                        }
                        result[insight.Category].Add(insight);
                    }
                }

                // Sort insights within each category by importance
                foreach (var category in result.Keys.ToList())
                {
                    result[category] = result[category]
                        .OrderByDescending(i => i.Importance)
                        .ThenByDescending(i => i.ExtractedAt)
                        .ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to aggregate insights by category");
                throw;
            }
        }

        public async Task<List<InsightTheme>> IdentifyCommonThemesAsync(
            List<CustomerInterview> interviews,
            int minimumOccurrences = 2)
        {
            try
            {
                var themes = new List<InsightTheme>();
                var themeIndex = new Dictionary<string, InsightTheme>();

                // Simple keyword-based theme identification
                foreach (var interview in interviews)
                {
                    foreach (var insight in interview.Insights)
                    {
                        var keywords = ExtractKeywords(insight.Description);
                        
                        foreach (var keyword in keywords)
                        {
                            if (!themeIndex.ContainsKey(keyword))
                            {
                                themeIndex[keyword] = new InsightTheme
                                {
                                    Theme = keyword,
                                    Occurrences = 0,
                                    AverageImportance = 0,
                                    RelatedInterviewIds = new List<Guid>(),
                                    KeyQuotes = new List<string>()
                                };
                            }

                            var theme = themeIndex[keyword];
                            theme.Occurrences++;
                            theme.AverageImportance = (theme.AverageImportance + insight.Importance) / 2;
                            
                            if (!theme.RelatedInterviewIds.Contains(interview.Id))
                            {
                                theme.RelatedInterviewIds.Add(interview.Id);
                            }

                            if (theme.KeyQuotes.Count < 3 && insight.Description.Contains(keyword))
                            {
                                theme.KeyQuotes.Add(insight.Description);
                            }
                        }
                    }
                }

                // Filter themes by minimum occurrences and sort by importance
                themes = themeIndex.Values
                    .Where(t => t.Occurrences >= minimumOccurrences)
                    .OrderByDescending(t => t.AverageImportance * t.Occurrences)
                    .ToList();

                return themes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to identify common themes");
                throw;
            }
        }

        public async Task<InsightDistributionMetrics> CalculateDistributionMetricsAsync(
            List<CustomerInterview> interviews)
        {
            try
            {
                var allInsights = interviews.SelectMany(i => i.Insights).ToList();

                if (!allInsights.Any())
                {
                    return new InsightDistributionMetrics
                    {
                        TotalInsights = 0,
                        AverageImportance = 0,
                        CategoryDistribution = new Dictionary<string, int>(),
                        ImportanceDistribution = new Dictionary<int, int>(),
                        TopCategories = new List<string>()
                    };
                }

                var categoryDistribution = allInsights
                    .GroupBy(i => i.Category)
                    .ToDictionary(g => g.Key, g => g.Count());

                var importanceDistribution = allInsights
                    .GroupBy(i => i.Importance)
                    .ToDictionary(g => g.Key, g => g.Count());

                var topCategories = categoryDistribution
                    .OrderByDescending(kvp => kvp.Value)
                    .Take(5)
                    .Select(kvp => kvp.Key)
                    .ToList();

                return new InsightDistributionMetrics
                {
                    TotalInsights = allInsights.Count,
                    AverageImportance = (decimal)allInsights.Average(i => i.Importance),
                    CategoryDistribution = categoryDistribution,
                    ImportanceDistribution = importanceDistribution,
                    TopCategories = topCategories
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate distribution metrics");
                throw;
            }
        }

        public async Task<SegmentInsightSummary> GenerateSegmentSummaryAsync(Guid customerSegmentId)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(customerSegmentId);
                var spec = new InterviewInSegmentSpecification(customerSegmentId);
                var interviews = await _interviewRepository.GetListAsync(spec.ToExpression());

                if (!interviews.Any())
                {
                    return new SegmentInsightSummary
                    {
                        SegmentId = customerSegmentId,
                        SegmentName = segment.Name,
                        InterviewCount = 0,
                        TotalInsights = 0,
                        TopThemes = new List<InsightTheme>(),
                        KeyPainPoints = new List<string>(),
                        IdentifiedOpportunities = new List<string>(),
                        OverallSentiment = 0
                    };
                }

                var themes = await IdentifyCommonThemesAsync(interviews);
                var allInsights = interviews.SelectMany(i => i.Insights).ToList();

                var painPoints = allInsights
                    .Where(i => i.Tags.Any(t => t.Category == "Pain Point") || 
                               i.Description.ToLower().Contains("problem") ||
                               i.Description.ToLower().Contains("difficult"))
                    .OrderByDescending(i => i.Importance)
                    .Take(5)
                    .Select(i => i.Description)
                    .ToList();

                var opportunities = allInsights
                    .Where(i => i.Tags.Any(t => t.Category == "Opportunity") || 
                               i.Description.ToLower().Contains("opportunity") ||
                               i.Description.ToLower().Contains("improve"))
                    .OrderByDescending(i => i.Importance)
                    .Take(5)
                    .Select(i => i.Description)
                    .ToList();

                // Simple sentiment calculation based on importance and keywords
                var sentiment = CalculateBasicSentiment(allInsights);

                return new SegmentInsightSummary
                {
                    SegmentId = customerSegmentId,
                    SegmentName = segment.Name,
                    InterviewCount = interviews.Count,
                    TotalInsights = allInsights.Count,
                    TopThemes = themes.Take(5).ToList(),
                    KeyPainPoints = painPoints,
                    IdentifiedOpportunities = opportunities,
                    OverallSentiment = sentiment
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate segment summary for {SegmentId}", customerSegmentId);
                throw;
            }
        }

        private List<string> ExtractKeywords(string text)
        {
            // Simple keyword extraction - can be replaced with more sophisticated NLP
            var stopWords = new HashSet<string> 
            { 
                "the", "a", "an", "and", "or", "but", "in", "on", "at", "to", "for", 
                "of", "with", "by", "is", "are", "was", "were", "be", "been", "have", 
                "has", "had", "do", "does", "did", "will", "would", "could", "should" 
            };

            return text.ToLower()
                .Split(new char[] { ' ', ',', '.', '!', '?', ';', ':' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(word => word.Length > 3 && !stopWords.Contains(word))
                .Distinct()
                .ToList();
        }

        private decimal CalculateBasicSentiment(List<Insight> insights)
        {
            if (!insights.Any()) return 0;

            var positiveKeywords = new HashSet<string> 
            { 
                "good", "great", "excellent", "love", "like", "positive", "helpful", 
                "useful", "easy", "simple", "convenient", "benefit", "opportunity" 
            };

            var negativeKeywords = new HashSet<string> 
            { 
                "bad", "terrible", "awful", "hate", "dislike", "negative", "difficult", 
                "hard", "complex", "problem", "issue", "pain", "frustrating" 
            };

            decimal totalSentiment = 0;
            foreach (var insight in insights)
            {
                var text = insight.Description.ToLower();
                int positiveCount = positiveKeywords.Count(keyword => text.Contains(keyword));
                int negativeCount = negativeKeywords.Count(keyword => text.Contains(keyword));
                
                decimal insightSentiment = (positiveCount - negativeCount) * insight.Importance;
                totalSentiment += insightSentiment;
            }

            // Normalize to 0-100 scale
            var maxPossibleScore = insights.Sum(i => i.Importance) * 3; // Assume max 3 sentiment keywords per insight
            var minPossibleScore = -maxPossibleScore;
            
            if (maxPossibleScore == minPossibleScore) return 50;
            
            return ((totalSentiment - minPossibleScore) / (maxPossibleScore - minPossibleScore)) * 100;
        }
    }
}
