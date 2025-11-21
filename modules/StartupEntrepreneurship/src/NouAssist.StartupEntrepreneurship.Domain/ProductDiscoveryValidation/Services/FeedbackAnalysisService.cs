using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Services
{
    /// <summary>
    /// Domain service for analyzing user feedback from testing sessions
    /// </summary>
    public class FeedbackAnalysisService : DomainService, IFeedbackAnalysisService
    {
        private readonly IUserFeedbackSessionRepository _sessionRepository;
        private readonly ISolutionPrototypeRepository _prototypeRepository;
        private readonly ILogger<FeedbackAnalysisService> _logger;

        public FeedbackAnalysisService(
            IUserFeedbackSessionRepository sessionRepository,
            ISolutionPrototypeRepository prototypeRepository,
            ILogger<FeedbackAnalysisService> logger)
        {
            _sessionRepository = sessionRepository;
            _prototypeRepository = prototypeRepository;
            _logger = logger;
        }

        public async Task<FeedbackPatternAnalysis> AnalyzeFeedbackPatternsAsync(List<UserFeedbackSession> sessions)
        {
            try
            {
                var analysis = new FeedbackPatternAnalysis
                {
                    PatternsByCategory = new Dictionary<string, List<FeedbackPattern>>(),
                    RecurringIssues = new List<string>(),
                    ConsistentPositives = new List<string>(),
                    CategorySatisfactionScores = new Dictionary<string, decimal>(),
                    Correlations = new List<FeedbackCorrelation>()
                };

                var allFeedback = sessions.SelectMany(s => s.FeedbackData).ToList();
                if (!allFeedback.Any()) return analysis;

                // Group feedback by category
                var feedbackByCategory = allFeedback.GroupBy(f => f.Category).ToDictionary(g => g.Key, g => g.ToList());

                // Analyze patterns within each category
                foreach (var categoryGroup in feedbackByCategory)
                {
                    var patterns = IdentifyPatternsInCategory(categoryGroup.Value);
                    analysis.PatternsByCategory[categoryGroup.Key] = patterns;

                    // Calculate category satisfaction score
                    var averageRating = categoryGroup.Value.Average(f => f.Rating.PercentageScore());
                    analysis.CategorySatisfactionScores[categoryGroup.Key] = averageRating;
                }

                // Identify recurring issues (patterns mentioned in multiple sessions)
                analysis.RecurringIssues = IdentifyRecurringIssues(sessions);

                // Identify consistent positives
                analysis.ConsistentPositives = IdentifyConsistentPositives(sessions);

                // Calculate correlations between categories
                analysis.Correlations = CalculateCategoryCorrelations(feedbackByCategory);

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze feedback patterns");
                throw;
            }
        }

        public async Task<SatisfactionMetrics> CalculateSatisfactionMetricsAsync(List<UserFeedbackSession> sessions)
        {
            try
            {
                var allFeedback = sessions.SelectMany(s => s.FeedbackData).ToList();
                if (!allFeedback.Any())
                {
                    return new SatisfactionMetrics
                    {
                        OverallSatisfaction = 0,
                        CategorySatisfaction = new Dictionary<string, decimal>(),
                        ResponseDistribution = new Dictionary<string, int>(),
                        NetPromoterScore = 0,
                        TopPositives = new List<string>(),
                        TopConcerns = new List<string>(),
                        TotalResponses = 0
                    };
                }

                // Calculate overall satisfaction
                var overallSatisfaction = allFeedback.Average(f => f.Rating.PercentageScore());

                // Calculate satisfaction by category
                var categorySatisfaction = allFeedback
                    .GroupBy(f => f.Category)
                    .ToDictionary(g => g.Key, g => g.Average(f => f.Rating.PercentageScore()));

                // Calculate response distribution
                var responseDistribution = allFeedback
                    .GroupBy(f => f.Rating.GetRatingLevel())
                    .ToDictionary(g => g.Key, g => g.Count());

                // Calculate NPS (simplified - based on overall satisfaction)
                var nps = CalculateNetPromoterScore(allFeedback);

                // Extract top positives and concerns
                var topPositives = ExtractTopSentiments(allFeedback, positive: true);
                var topConcerns = ExtractTopSentiments(allFeedback, positive: false);

                return new SatisfactionMetrics
                {
                    OverallSatisfaction = overallSatisfaction,
                    CategorySatisfaction = categorySatisfaction,
                    ResponseDistribution = responseDistribution,
                    NetPromoterScore = nps,
                    TopPositives = topPositives,
                    TopConcerns = topConcerns,
                    TotalResponses = allFeedback.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate satisfaction metrics");
                throw;
            }
        }

        public async Task<List<ImprovementOpportunity>> IdentifyImprovementOpportunitiesAsync(List<UserFeedbackSession> sessions)
        {
            try
            {
                var opportunities = new List<ImprovementOpportunity>();
                var allFeedback = sessions.SelectMany(s => s.FeedbackData).ToList();

                if (!allFeedback.Any()) return opportunities;

                // Analyze low-satisfaction areas
                var lowSatisfactionAreas = allFeedback
                    .GroupBy(f => f.Category)
                    .Where(g => g.Average(f => f.Rating.PercentageScore()) < 60)
                    .OrderBy(g => g.Average(f => f.Rating.PercentageScore()))
                    .ToList();

                foreach (var area in lowSatisfactionAreas)
                {
                    var avgRating = area.Average(f => f.Rating.PercentageScore());
                    var affectedUsers = area.Select(f => f.FeedbackSessionId).Distinct().Count();
                    var commonIssues = ExtractCommonIssues(area.ToList());

                    opportunities.Add(new ImprovementOpportunity
                    {
                        Area = area.Key,
                        Description = $"Low satisfaction in {area.Key} (avg: {avgRating:F1}%)",
                        Impact = 100 - avgRating, // Impact is inverse of satisfaction
                        Evidence = string.Join(", ", commonIssues),
                        SuggestedActions = GenerateImprovementActions(area.Key, commonIssues),
                        Priority = DetermineImprovementPriority(avgRating, affectedUsers),
                        AffectedUsers = affectedUsers
                    });
                }

                // Analyze frequently mentioned pain points
                var painPointPatterns = IdentifyPainPointPatterns(allFeedback);
                foreach (var pattern in painPointPatterns.Take(5))
                {
                    opportunities.Add(new ImprovementOpportunity
                    {
                        Area = "User Experience",
                        Description = pattern.Pattern,
                        Impact = pattern.Frequency * pattern.AverageRating,
                        Evidence = string.Join("; ", pattern.Examples),
                        SuggestedActions = new List<string> { $"Address recurring issue: {pattern.Pattern}" },
                        Priority = pattern.Frequency > 5 ? FeaturePriority.High : FeaturePriority.Medium,
                        AffectedUsers = pattern.Frequency
                    });
                }

                return opportunities.OrderByDescending(o => o.Impact).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to identify improvement opportunities");
                throw;
            }
        }

        public async Task<PrototypeFeedbackSummary> GeneratePrototypeFeedbackSummaryAsync(Guid prototypeId)
        {
            try
            {
                var prototype = await _prototypeRepository.GetAsync(prototypeId);
                var sessions = await _sessionRepository.GetByPrototypeAsync(prototypeId, includeDetails: true);

                if (!sessions.Any())
                {
                    return new PrototypeFeedbackSummary
                    {
                        PrototypeId = prototypeId,
                        PrototypeName = prototype.Name,
                        SessionCount = 0,
                        ParticipantCount = 0,
                        Satisfaction = new SatisfactionMetrics(),
                        TopOpportunities = new List<ImprovementOpportunity>(),
                        KeyInsights = new List<string>(),
                        VersionComparisons = new Dictionary<string, string>(),
                        OverallAssessment = "No feedback available"
                    };
                }

                var satisfaction = await CalculateSatisfactionMetricsAsync(sessions);
                var opportunities = await IdentifyImprovementOpportunitiesAsync(sessions);
                var keyInsights = ExtractKeyInsights(sessions);
                var versionComparisons = CompareVersionFeedback(sessions);
                var overallAssessment = GenerateOverallAssessment(satisfaction, opportunities);

                return new PrototypeFeedbackSummary
                {
                    PrototypeId = prototypeId,
                    PrototypeName = prototype.Name,
                    SessionCount = sessions.Count,
                    ParticipantCount = sessions.Select(s => s.ParticipantEmail).Distinct().Count(),
                    Satisfaction = satisfaction,
                    TopOpportunities = opportunities.Take(5).ToList(),
                    KeyInsights = keyInsights,
                    VersionComparisons = versionComparisons,
                    OverallAssessment = overallAssessment
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate prototype feedback summary for {PrototypeId}", prototypeId);
                throw;
            }
        }

        public async Task<SentimentAnalysisResult> AnalyzeFeedbackSentimentAsync(List<UserFeedbackSession> sessions)
        {
            try
            {
                var allFeedback = sessions.SelectMany(s => s.FeedbackData).ToList();
                if (!allFeedback.Any())
                {
                    return new SentimentAnalysisResult
                    {
                        OverallSentiment = 0,
                        CategorySentiment = new Dictionary<string, decimal>(),
                        PositiveFeedback = new List<string>(),
                        NegativeFeedback = new List<string>(),
                        NeutralFeedback = new List<string>(),
                        EmotionDistribution = new Dictionary<string, int>(),
                        Trends = new List<SentimentTrend>()
                    };
                }

                // Calculate overall sentiment based on ratings and content
                var overallSentiment = CalculateOverallSentiment(allFeedback);

                // Calculate sentiment by category
                var categorySentiment = allFeedback
                    .GroupBy(f => f.Category)
                    .ToDictionary(g => g.Key, g => CalculateCategorySentiment(g.ToList()));

                // Categorize feedback by sentiment
                var positiveFeedback = new List<string>();
                var negativeFeedback = new List<string>();
                var neutralFeedback = new List<string>();

                foreach (var feedback in allFeedback)
                {
                    var sentiment = AnalyzeResponseSentiment(feedback.Response);
                    if (sentiment > 0.1m)
                        positiveFeedback.Add(feedback.Response);
                    else if (sentiment < -0.1m)
                        negativeFeedback.Add(feedback.Response);
                    else
                        neutralFeedback.Add(feedback.Response);
                }

                // Analyze emotion distribution
                var emotionDistribution = AnalyzeEmotions(allFeedback);

                // Calculate sentiment trends over time
                var trends = CalculateSentimentTrends(sessions);

                return new SentimentAnalysisResult
                {
                    OverallSentiment = overallSentiment,
                    CategorySentiment = categorySentiment,
                    PositiveFeedback = positiveFeedback.Take(10).ToList(),
                    NegativeFeedback = negativeFeedback.Take(10).ToList(),
                    NeutralFeedback = neutralFeedback.Take(5).ToList(),
                    EmotionDistribution = emotionDistribution,
                    Trends = trends
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze feedback sentiment");
                throw;
            }
        }

        #region Private Methods

        private List<FeedbackPattern> IdentifyPatternsInCategory(List<FeedbackData> categoryFeedback)
        {
            var patterns = new List<FeedbackPattern>();

            // Simple keyword-based pattern identification
            var wordCounts = new Dictionary<string, List<FeedbackData>>();

            foreach (var feedback in categoryFeedback)
            {
                var words = ExtractKeywords(feedback.Response);
                foreach (var word in words)
                {
                    if (!wordCounts.ContainsKey(word))
                        wordCounts[word] = new List<FeedbackData>();
                    wordCounts[word].Add(feedback);
                }
            }

            foreach (var wordGroup in wordCounts.Where(w => w.Value.Count >= 2))
            {
                patterns.Add(new FeedbackPattern
                {
                    Pattern = wordGroup.Key,
                    Frequency = wordGroup.Value.Count,
                    AverageRating = wordGroup.Value.Average(f => f.Rating.PercentageScore()),
                    Examples = wordGroup.Value.Take(3).Select(f => f.Response).ToList(),
                    Impact = wordGroup.Value.Count > 3 ? "High" : "Medium"
                });
            }

            return patterns.OrderByDescending(p => p.Frequency).Take(10).ToList();
        }

        private List<string> IdentifyRecurringIssues(List<UserFeedbackSession> sessions)
        {
            var issueKeywords = new[] { "problem", "issue", "difficult", "confusing", "error", "bug", "slow" };
            var issuePatterns = new Dictionary<string, int>();

            foreach (var session in sessions)
            {
                foreach (var feedback in session.FeedbackData)
                {
                    if (feedback.Rating.PercentageScore() < 50) // Low rating indicates issue
                    {
                        var words = ExtractKeywords(feedback.Response);
                        foreach (var word in words.Intersect(issueKeywords))
                        {
                            issuePatterns[word] = issuePatterns.GetValueOrDefault(word) + 1;
                        }
                    }
                }
            }

            return issuePatterns
                .Where(kvp => kvp.Value >= 3) // Mentioned in at least 3 sessions
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        private List<string> IdentifyConsistentPositives(List<UserFeedbackSession> sessions)
        {
            var positiveKeywords = new[] { "good", "great", "excellent", "easy", "intuitive", "helpful", "fast" };
            var positivePatterns = new Dictionary<string, int>();

            foreach (var session in sessions)
            {
                foreach (var feedback in session.FeedbackData)
                {
                    if (feedback.Rating.PercentageScore() >= 80) // High rating indicates positive
                    {
                        var words = ExtractKeywords(feedback.Response);
                        foreach (var word in words.Intersect(positiveKeywords))
                        {
                            positivePatterns[word] = positivePatterns.GetValueOrDefault(word) + 1;
                        }
                    }
                }
            }

            return positivePatterns
                .Where(kvp => kvp.Value >= 3)
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        private List<FeedbackCorrelation> CalculateCategoryCorrelations(Dictionary<string, List<FeedbackData>> feedbackByCategory)
        {
            var correlations = new List<FeedbackCorrelation>();
            var categories = feedbackByCategory.Keys.ToList();

            for (int i = 0; i < categories.Count; i++)
            {
                for (int j = i + 1; j < categories.Count; j++)
                {
                    var cat1 = categories[i];
                    var cat2 = categories[j];

                    var correlation = CalculateCorrelation(
                        feedbackByCategory[cat1].Select(f => (double)f.Rating.PercentageScore()).ToList(),
                        feedbackByCategory[cat2].Select(f => (double)f.Rating.PercentageScore()).ToList()
                    );

                    if (Math.Abs(correlation) > 0.3) // Only include significant correlations
                    {
                        correlations.Add(new FeedbackCorrelation
                        {
                            Category1 = cat1,
                            Category2 = cat2,
                            CorrelationCoefficient = (decimal)correlation,
                            Interpretation = InterpretCorrelation(correlation)
                        });
                    }
                }
            }

            return correlations;
        }

        private double CalculateCorrelation(List<double> x, List<double> y)
        {
            if (x.Count != y.Count || x.Count < 2) return 0;

            var avgX = x.Average();
            var avgY = y.Average();

            var numerator = x.Zip(y, (xi, yi) => (xi - avgX) * (yi - avgY)).Sum();
            var denominator = Math.Sqrt(x.Sum(xi => Math.Pow(xi - avgX, 2)) * y.Sum(yi => Math.Pow(yi - avgY, 2)));

            return denominator == 0 ? 0 : numerator / denominator;
        }

        private string InterpretCorrelation(double correlation)
        {
            var absCorr = Math.Abs(correlation);
            return absCorr switch
            {
                >= 0.7 => correlation > 0 ? "Strong positive correlation" : "Strong negative correlation",
                >= 0.3 => correlation > 0 ? "Moderate positive correlation" : "Moderate negative correlation",
                _ => "Weak correlation"
            };
        }

        private decimal CalculateNetPromoterScore(List<FeedbackData> allFeedback)
        {
            // Simplified NPS calculation based on rating percentages
            var promoters = allFeedback.Count(f => f.Rating.PercentageScore() >= 80);
            var detractors = allFeedback.Count(f => f.Rating.PercentageScore() < 60);
            var total = allFeedback.Count;

            if (total == 0) return 0;

            return ((decimal)(promoters - detractors) / total) * 100;
        }

        private List<string> ExtractTopSentiments(List<FeedbackData> allFeedback, bool positive)
        {
            var threshold = positive ? 80m : 40m;
            var filteredFeedback = allFeedback.Where(f => positive ? 
                f.Rating.PercentageScore() >= threshold : 
                f.Rating.PercentageScore() <= threshold);

            return filteredFeedback
                .GroupBy(f => ExtractMainSentiment(f.Response))
                .Where(g => !string.IsNullOrEmpty(g.Key))
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();
        }

        private List<string> ExtractCommonIssues(List<FeedbackData> feedbackList)
        {
            var negativeKeywords = new[] { "difficult", "confusing", "slow", "error", "problem", "issue", "hard" };
            var issueCounts = new Dictionary<string, int>();

            foreach (var feedback in feedbackList)
            {
                var words = ExtractKeywords(feedback.Response);
                foreach (var word in words.Intersect(negativeKeywords))
                {
                    issueCounts[word] = issueCounts.GetValueOrDefault(word) + 1;
                }
            }

            return issueCounts
                .OrderByDescending(kvp => kvp.Value)
                .Take(3)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        private List<string> GenerateImprovementActions(string area, List<string> commonIssues)
        {
            var actions = new List<string>();

            if (commonIssues.Contains("difficult") || commonIssues.Contains("hard"))
                actions.Add($"Simplify the {area} interface and workflows");

            if (commonIssues.Contains("confusing"))
                actions.Add($"Improve clarity and labeling in {area}");

            if (commonIssues.Contains("slow"))
                actions.Add($"Optimize performance for {area} functions");

            if (commonIssues.Contains("error") || commonIssues.Contains("problem"))
                actions.Add($"Fix technical issues and bugs in {area}");

            if (!actions.Any())
                actions.Add($"Investigate and address user concerns in {area}");

            return actions;
        }

        private FeaturePriority DetermineImprovementPriority(decimal avgRating, int affectedUsers)
        {
            if (avgRating < 40 && affectedUsers >= 5)
                return FeaturePriority.Critical;
            if (avgRating < 60 && affectedUsers >= 3)
                return FeaturePriority.High;
            if (avgRating < 70)
                return FeaturePriority.Medium;
            
            return FeaturePriority.Low;
        }

        private List<FeedbackPattern> IdentifyPainPointPatterns(List<FeedbackData> allFeedback)
        {
            var lowRatingFeedback = allFeedback.Where(f => f.Rating.PercentageScore() < 60).ToList();
            return IdentifyPatternsInCategory(lowRatingFeedback);
        }

        private List<string> ExtractKeyInsights(List<UserFeedbackSession> sessions)
        {
            var insights = new List<string>();

            // Calculate overall satisfaction trend
            var avgSatisfaction = sessions.SelectMany(s => s.FeedbackData).Average(f => f.Rating.PercentageScore());
            insights.Add($"Overall satisfaction: {avgSatisfaction:F1}%");

            // Identify most praised features
            var positivePatterns = sessions.SelectMany(s => s.FeedbackData)
                .Where(f => f.Rating.PercentageScore() >= 80)
                .GroupBy(f => f.Category)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (positivePatterns != null)
                insights.Add($"Most praised aspect: {positivePatterns.Key}");

            // Identify biggest concern
            var negativePatterns = sessions.SelectMany(s => s.FeedbackData)
                .Where(f => f.Rating.PercentageScore() < 50)
                .GroupBy(f => f.Category)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (negativePatterns != null)
                insights.Add($"Biggest concern: {negativePatterns.Key}");

            return insights;
        }

        private Dictionary<string, string> CompareVersionFeedback(List<UserFeedbackSession> sessions)
        {
            var comparisons = new Dictionary<string, string>();

            var sessionsByVersion = sessions
                .Where(s => s.PrototypeVersionId.HasValue)
                .GroupBy(s => s.PrototypeVersionId.Value)
                .ToList();

            if (sessionsByVersion.Count >= 2)
            {
                var latestVersion = sessionsByVersion.OrderByDescending(g => g.Max(s => s.CreationTime)).First();
                var previousVersion = sessionsByVersion.OrderByDescending(g => g.Max(s => s.CreationTime)).Skip(1).First();

                var latestAvg = latestVersion.SelectMany(s => s.FeedbackData).Average(f => f.Rating.PercentageScore());
                var previousAvg = previousVersion.SelectMany(s => s.FeedbackData).Average(f => f.Rating.PercentageScore());

                var improvement = latestAvg - previousAvg;
                comparisons["Version Comparison"] = $"Latest version {(improvement > 0 ? "improved" : "declined")} by {Math.Abs(improvement):F1}%";
            }

            return comparisons;
        }

        private string GenerateOverallAssessment(SatisfactionMetrics satisfaction, List<ImprovementOpportunity> opportunities)
        {
            if (satisfaction.OverallSatisfaction >= 80)
                return "Excellent performance with high user satisfaction. Focus on maintaining quality.";
            if (satisfaction.OverallSatisfaction >= 60)
                return "Good performance with some areas for improvement identified.";
            if (satisfaction.OverallSatisfaction >= 40)
                return "Moderate performance. Several improvement opportunities identified.";
            
            return "Poor performance. Significant improvements needed across multiple areas.";
        }

        private decimal CalculateOverallSentiment(List<FeedbackData> allFeedback)
        {
            var totalSentiment = 0m;
            foreach (var feedback in allFeedback)
            {
                var ratingSentiment = (feedback.Rating.PercentageScore() - 50) / 50; // Normalize to -1 to 1
                var textSentiment = AnalyzeResponseSentiment(feedback.Response);
                totalSentiment += (ratingSentiment + textSentiment) / 2;
            }

            return allFeedback.Any() ? totalSentiment / allFeedback.Count : 0;
        }

        private decimal CalculateCategorySentiment(List<FeedbackData> categoryFeedback)
        {
            return CalculateOverallSentiment(categoryFeedback);
        }

        private decimal AnalyzeResponseSentiment(string response)
        {
            // Simple sentiment analysis based on keywords
            var positiveWords = new[] { "good", "great", "excellent", "love", "like", "amazing", "helpful", "easy" };
            var negativeWords = new[] { "bad", "terrible", "hate", "dislike", "difficult", "confusing", "slow", "annoying" };

            var words = ExtractKeywords(response);
            var positiveCount = words.Intersect(positiveWords).Count();
            var negativeCount = words.Intersect(negativeWords).Count();

            if (positiveCount + negativeCount == 0) return 0;
            return (decimal)(positiveCount - negativeCount) / (positiveCount + negativeCount);
        }

        private Dictionary<string, int> AnalyzeEmotions(List<FeedbackData> allFeedback)
        {
            var emotions = new Dictionary<string, int>
            {
                ["Positive"] = 0,
                ["Negative"] = 0,
                ["Neutral"] = 0,
                ["Frustrated"] = 0,
                ["Satisfied"] = 0
            };

            foreach (var feedback in allFeedback)
            {
                var sentiment = AnalyzeResponseSentiment(feedback.Response);
                var rating = feedback.Rating.PercentageScore();

                if (sentiment > 0.2m || rating >= 80)
                {
                    emotions["Positive"]++;
                    if (rating >= 90) emotions["Satisfied"]++;
                }
                else if (sentiment < -0.2m || rating <= 40)
                {
                    emotions["Negative"]++;
                    if (rating <= 30) emotions["Frustrated"]++;
                }
                else
                {
                    emotions["Neutral"]++;
                }
            }

            return emotions;
        }

        private List<SentimentTrend> CalculateSentimentTrends(List<UserFeedbackSession> sessions)
        {
            var trends = new List<SentimentTrend>();

            var sessionsByDate = sessions
                .Where(s => s.CompletedDate.HasValue)
                .GroupBy(s => s.CompletedDate.Value.Date)
                .OrderBy(g => g.Key)
                .ToList();

            foreach (var dateGroup in sessionsByDate)
            {
                var dailyFeedback = dateGroup.SelectMany(s => s.FeedbackData).ToList();
                if (dailyFeedback.Any())
                {
                    var avgSentiment = CalculateOverallSentiment(dailyFeedback);
                    trends.Add(new SentimentTrend
                    {
                        Date = dateGroup.Key,
                        SentimentScore = avgSentiment,
                        ResponseCount = dailyFeedback.Count,
                        Category = "Overall"
                    });
                }
            }

            return trends;
        }

        private List<string> ExtractKeywords(string text)
        {
            var stopWords = new HashSet<string> 
            { 
                "the", "a", "an", "and", "or", "but", "in", "on", "at", "to", "for", 
                "of", "with", "by", "is", "are", "was", "were", "be", "been", "have", 
                "has", "had", "do", "does", "did", "will", "would", "could", "should" 
            };

            return text.ToLower()
                .Split(new char[] { ' ', ',', '.', '!', '?', ';', ':' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(word => word.Length > 2 && !stopWords.Contains(word))
                .Distinct()
                .ToList();
        }

        private string ExtractMainSentiment(string response)
        {
            if (string.IsNullOrWhiteSpace(response)) return string.Empty;

            var sentences = response.Split('.', '!', '?');
            return sentences.OrderByDescending(s => s.Length).FirstOrDefault()?.Trim() ?? string.Empty;
        }

        #endregion
    }
}
