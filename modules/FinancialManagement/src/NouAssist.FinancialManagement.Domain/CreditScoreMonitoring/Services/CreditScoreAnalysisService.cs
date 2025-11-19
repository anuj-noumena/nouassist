using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Services
{
    public class CreditScoreAnalysisService : DomainService
    {
        public CreditHealthReport AnalyzeCreditHealth(CreditReport report)
        {
            var factors = new List<CreditHealthFactor>();

            // Analyze credit utilization
            var utilizationHealth = AnalyzeUtilization(report);
            factors.Add(utilizationHealth);

            // Analyze payment history
            var paymentHealth = AnalyzePaymentHistory(report);
            factors.Add(paymentHealth);

            // Analyze credit age
            var ageHealth = AnalyzeCreditAge(report);
            factors.Add(ageHealth);

            // Analyze inquiries
            var inquiryHealth = AnalyzeInquiries(report);
            factors.Add(inquiryHealth);

            // Analyze derogatory marks
            var derogatoryHealth = AnalyzeDerogatoryMarks(report);
            factors.Add(derogatoryHealth);

            var overallScore = CalculateOverallHealthScore(factors);
            var healthLevel = DetermineHealthLevel(overallScore);

            return new CreditHealthReport(
                report.CreditScore,
                report.ScoreRange,
                overallScore,
                healthLevel,
                factors,
                report.GetCreditImprovementRecommendations()
            );
        }

        private CreditHealthFactor AnalyzeUtilization(CreditReport report)
        {
            var utilization = report.CreditUtilizationPercentage;
            var score = utilization switch
            {
                <= 10 => 100,
                <= 30 => 85,
                <= 50 => 60,
                <= 75 => 30,
                _ => 10
            };

            var impact = utilization <= 30 ? "Positive" : "Negative";
            var description = $"Credit utilization is {utilization:F1}%";

            return new CreditHealthFactor(
                "Credit Utilization",
                score,
                impact,
                description,
                35 // Weight percentage
            );
        }

        private CreditHealthFactor AnalyzePaymentHistory(CreditReport report)
        {
            var paymentHistory = report.GetPaymentHistory();
            var score = paymentHistory.OnTimePaymentPercentage;
            var impact = score >= 95 ? "Positive" : "Negative";
            var description = $"{paymentHistory.OnTimePaymentPercentage:F1}% on-time payments";

            return new CreditHealthFactor(
                "Payment History",
                (int)score,
                impact,
                description,
                35 // Weight percentage
            );
        }

        private CreditHealthFactor AnalyzeCreditAge(CreditReport report)
        {
            var ageMonths = report.AverageAccountAgeMonths;
            var score = ageMonths switch
            {
                >= 84 => 100, // 7+ years
                >= 60 => 85,  // 5+ years
                >= 36 => 70,  // 3+ years
                >= 24 => 50,  // 2+ years
                _ => 30
            };

            var impact = ageMonths >= 36 ? "Positive" : "Neutral";
            var description = $"Average account age: {ageMonths / 12} years {ageMonths % 12} months";

            return new CreditHealthFactor(
                "Credit History Length",
                score,
                impact,
                description,
                15 // Weight percentage
            );
        }

        private CreditHealthFactor AnalyzeInquiries(CreditReport report)
        {
            var hardInquiries = report.HardInquiriesLast12Months;
            var score = hardInquiries switch
            {
                0 => 100,
                1 => 90,
                2 => 75,
                3 => 60,
                4 => 40,
                _ => 20
            };

            var impact = hardInquiries <= 2 ? "Neutral" : "Negative";
            var description = $"{hardInquiries} hard inquiries in last 12 months";

            return new CreditHealthFactor(
                "Credit Inquiries",
                score,
                impact,
                description,
                10 // Weight percentage
            );
        }

        private CreditHealthFactor AnalyzeDerogatoryMarks(CreditReport report)
        {
            var totalNegative = report.DelinquentAccounts + 
                               report.CollectionAccounts + 
                               report.PublicRecords;

            var score = totalNegative switch
            {
                0 => 100,
                1 => 50,
                2 => 25,
                _ => 10
            };

            var impact = totalNegative == 0 ? "Positive" : "Very Negative";
            var description = $"{totalNegative} derogatory marks";

            return new CreditHealthFactor(
                "Derogatory Marks",
                score,
                impact,
                description,
                5 // Weight percentage
            );
        }

        private int CalculateOverallHealthScore(List<CreditHealthFactor> factors)
        {
            var weightedSum = factors.Sum(f => f.Score * f.Weight);
            var totalWeight = factors.Sum(f => f.Weight);
            return (int)(weightedSum / totalWeight);
        }

        private string DetermineHealthLevel(int score)
        {
            return score switch
            {
                >= 85 => "Excellent",
                >= 70 => "Good",
                >= 50 => "Fair",
                >= 30 => "Poor",
                _ => "Very Poor"
            };
        }

        public ScoreProjection ProjectScoreImprovement(
            CreditReport currentReport,
            CreditImprovementPlan plan)
        {
            var currentScore = currentReport.CreditScore;
            var projectedGain = 0;

            // Project utilization improvement
            if (plan.ReduceUtilizationTo.HasValue)
            {
                var currentUtil = currentReport.CreditUtilizationPercentage;
                var targetUtil = plan.ReduceUtilizationTo.Value;
                
                if (targetUtil < currentUtil)
                {
                    projectedGain += CalculateUtilizationImpact(currentUtil, targetUtil);
                }
            }

            // Project payment history improvement
            if (plan.PayOffDelinquentAccounts)
            {
                projectedGain += currentReport.DelinquentAccounts * 15;
            }

            // Project age impact
            var monthsToWait = plan.MonthsToWait;
            projectedGain += Math.Min(monthsToWait / 6, 10); // Small gains over time

            var projectedScore = Math.Min(850, currentScore + projectedGain);
            var timelineMonths = CalculateTimelineMonths(plan);

            return new ScoreProjection(
                currentScore,
                projectedScore,
                projectedGain,
                timelineMonths,
                DateTime.UtcNow.AddMonths(timelineMonths)
            );
        }

        private int CalculateUtilizationImpact(decimal current, decimal target)
        {
            if (current <= 30)
            {
                return 0;
            }

            if (target <= 10)
            {
                return 40;
            }

            if (target <= 30)
            {
                return 25;
            }

            return 10;
        }

        private int CalculateTimelineMonths(CreditImprovementPlan plan)
        {
            var months = 0;

            if (plan.ReduceUtilizationTo.HasValue)
            {
                months = Math.Max(months, 3); // 3 months for utilization impact
            }

            if (plan.PayOffDelinquentAccounts)
            {
                months = Math.Max(months, 6); // 6 months for payment history
            }

            months += plan.MonthsToWait;

            return Math.Min(months, 24); // Cap at 2 years
        }
    }

    public class CreditHealthReport
    {
        public int CreditScore { get; }
        public CreditScoreRange ScoreRange { get; }
        public int OverallHealthScore { get; }
        public string HealthLevel { get; }
        public List<CreditHealthFactor> Factors { get; }
        public List<string> Recommendations { get; }

        public CreditHealthReport(
            int creditScore,
            CreditScoreRange scoreRange,
            int overallHealthScore,
            string healthLevel,
            List<CreditHealthFactor> factors,
            List<string> recommendations)
        {
            CreditScore = creditScore;
            ScoreRange = scoreRange;
            OverallHealthScore = overallHealthScore;
            HealthLevel = healthLevel;
            Factors = factors;
            Recommendations = recommendations;
        }
    }

    public class CreditHealthFactor
    {
        public string Name { get; }
        public int Score { get; }
        public string Impact { get; }
        public string Description { get; }
        public int Weight { get; }

        public CreditHealthFactor(
            string name,
            int score,
            string impact,
            string description,
            int weight)
        {
            Name = name;
            Score = score;
            Impact = impact;
            Description = description;
            Weight = weight;
        }
    }

    public class CreditImprovementPlan
    {
        public decimal? ReduceUtilizationTo { get; set; }
        public bool PayOffDelinquentAccounts { get; set; }
        public int MonthsToWait { get; set; }
    }

    public class ScoreProjection
    {
        public int CurrentScore { get; }
        public int ProjectedScore { get; }
        public int ProjectedGain { get; }
        public int TimelineMonths { get; }
        public DateTime ProjectedAchievementDate { get; }

        public ScoreProjection(
            int currentScore,
            int projectedScore,
            int projectedGain,
            int timelineMonths,
            DateTime projectedAchievementDate)
        {
            CurrentScore = currentScore;
            ProjectedScore = projectedScore;
            ProjectedGain = projectedGain;
            TimelineMonths = timelineMonths;
            ProjectedAchievementDate = projectedAchievementDate;
        }
    }
}
