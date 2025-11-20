using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.DebtManagement.Services
{
    /// <summary>
    /// Domain service for analyzing credit score impact from debt management
    /// </summary>
    public class CreditScoreImpactService : DomainService
    {
        private readonly ILogger<CreditScoreImpactService> _logger;

        public CreditScoreImpactService(
            ILogger<CreditScoreImpactService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Estimate credit score impact from paying off debt
        /// </summary>
        public CreditScoreImpactEstimate EstimatePayoffImpact(
            decimal currentBalance,
            decimal creditLimit,
            int currentScore,
            string debtType)
        {
            _logger.LogInformation(
                "Estimating credit score impact for debt payoff: Balance={Balance}, Limit={Limit}, Type={DebtType}",
                currentBalance,
                creditLimit,
                debtType);

            // Calculate current utilization
            var currentUtilization = creditLimit > 0 ? (currentBalance / creditLimit) * 100 : 0;
            var newUtilization = 0m; // After payoff

            // Estimate score impact based on utilization change
            var estimatedIncrease = EstimateScoreIncrease(currentUtilization, newUtilization, currentScore);

            return new CreditScoreImpactEstimate
            {
                CurrentUtilization = currentUtilization,
                NewUtilization = newUtilization,
                UtilizationChange = currentUtilization - newUtilization,
                EstimatedScoreIncrease = estimatedIncrease,
                EstimatedNewScore = currentScore + estimatedIncrease,
                ImpactLevel = DetermineImpactLevel(estimatedIncrease),
                Recommendation = GenerateRecommendation(currentUtilization, debtType)
            };
        }

        /// <summary>
        /// Analyze credit utilization and suggest optimal payoff strategy
        /// </summary>
        public CreditUtilizationAnalysis AnalyzeCreditUtilization(
            List<DebtUtilizationInfo> debts,
            int currentCreditScore)
        {
            var totalBalance = 0m;
            var totalLimit = 0m;
            var highUtilizationDebts = new List<DebtUtilizationInfo>();

            foreach (var debt in debts)
            {
                totalBalance += debt.CurrentBalance;
                totalLimit += debt.CreditLimit;

                var utilization = debt.CreditLimit > 0 ? (debt.CurrentBalance / debt.CreditLimit) * 100 : 0;

                if (utilization > 30)
                {
                    highUtilizationDebts.Add(debt);
                }
            }

            var overallUtilization = totalLimit > 0 ? (totalBalance / totalLimit) * 100 : 0;

            return new CreditUtilizationAnalysis
            {
                OverallUtilization = overallUtilization,
                TotalBalance = totalBalance,
                TotalCreditLimit = totalLimit,
                HighUtilizationDebts = highUtilizationDebts,
                IsHealthy = overallUtilization < 30,
                SuggestedPayoffOrder = CalculateOptimalPayoffOrder(highUtilizationDebts),
                EstimatedScoreImpact = EstimateScoreIncrease(overallUtilization, 10, currentCreditScore)
            };
        }

        /// <summary>
        /// Track credit score improvement milestone after debt payoff
        /// </summary>
        public CreditMilestone RecordDebtPayoffMilestone(
            string debtName,
            decimal amountPaid,
            int scoreBefore,
            int scoreAfter,
            DateTime paidOffDate)
        {
            var scoreIncrease = scoreAfter - scoreBefore;

            return new CreditMilestone
            {
                MilestoneType = "DebtPayoff",
                DebtName = debtName,
                AmountPaid = amountPaid,
                ScoreBefore = scoreBefore,
                ScoreAfter = scoreAfter,
                ScoreIncrease = scoreIncrease,
                AchievedDate = paidOffDate,
                Message = GenerateMilestoneMessage(debtName, scoreIncrease)
            };
        }

        private int EstimateScoreIncrease(decimal currentUtilization, decimal newUtilization, int currentScore)
        {
            // Simplified model - real credit scoring is much more complex
            var utilizationDrop = currentUtilization - newUtilization;

            if (utilizationDrop <= 0) return 0;

            // High utilization (>70%) → bigger impact
            if (currentUtilization > 70)
            {
                if (newUtilization < 30)
                    return 40; // Major improvement
                if (newUtilization < 50)
                    return 25;
                return 15;
            }

            // Medium utilization (30-70%)
            if (currentUtilization > 30)
            {
                if (newUtilization < 10)
                    return 25;
                if (newUtilization < 30)
                    return 15;
                return 10;
            }

            // Low utilization (<30%) - minimal impact
            return 5;
        }

        private string DetermineImpactLevel(int scoreIncrease)
        {
            if (scoreIncrease >= 40) return "Major";
            if (scoreIncrease >= 20) return "Significant";
            if (scoreIncrease >= 10) return "Moderate";
            if (scoreIncrease > 0) return "Minor";
            return "None";
        }

        private string GenerateRecommendation(decimal currentUtilization, string debtType)
        {
            if (currentUtilization > 70)
                return $"URGENT: Pay down this {debtType} immediately. Utilization over 70% severely impacts your score.";

            if (currentUtilization > 50)
                return $"HIGH PRIORITY: Reduce this {debtType} below 30% utilization for optimal credit score.";

            if (currentUtilization > 30)
                return $"Paying off this {debtType} will improve your credit score. Target below 30% utilization.";

            return $"This {debtType} has healthy utilization. Maintain current payment schedule.";
        }

        private List<DebtUtilizationInfo> CalculateOptimalPayoffOrder(List<DebtUtilizationInfo> debts)
        {
            // Sort by utilization percentage (highest first) for maximum score impact
            var ordered = new List<DebtUtilizationInfo>(debts);
            ordered.Sort((a, b) =>
            {
                var utilizationA = a.CreditLimit > 0 ? (a.CurrentBalance / a.CreditLimit) * 100 : 0;
                var utilizationB = b.CreditLimit > 0 ? (b.CurrentBalance / b.CreditLimit) * 100 : 0;
                return utilizationB.CompareTo(utilizationA);
            });
            return ordered;
        }

        private string GenerateMilestoneMessage(string debtName, int scoreIncrease)
        {
            if (scoreIncrease >= 40)
                return $"🎉 Amazing! Paying off {debtName} boosted your credit score by {scoreIncrease} points!";

            if (scoreIncrease >= 20)
                return $"🎊 Great job! Your credit score increased by {scoreIncrease} points after paying off {debtName}!";

            if (scoreIncrease >= 10)
                return $"✅ Nice work! Paying off {debtName} improved your score by {scoreIncrease} points.";

            if (scoreIncrease > 0)
                return $"👍 {debtName} paid off! Your score improved by {scoreIncrease} points.";

            return $"✓ {debtName} paid off successfully!";
        }
    }

    public class CreditScoreImpactEstimate
    {
        public decimal CurrentUtilization { get; set; }
        public decimal NewUtilization { get; set; }
        public decimal UtilizationChange { get; set; }
        public int EstimatedScoreIncrease { get; set; }
        public int EstimatedNewScore { get; set; }
        public string ImpactLevel { get; set; }
        public string Recommendation { get; set; }
    }

    public class CreditUtilizationAnalysis
    {
        public decimal OverallUtilization { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalCreditLimit { get; set; }
        public List<DebtUtilizationInfo> HighUtilizationDebts { get; set; }
        public bool IsHealthy { get; set; }
        public List<DebtUtilizationInfo> SuggestedPayoffOrder { get; set; }
        public int EstimatedScoreImpact { get; set; }
    }

    public class DebtUtilizationInfo
    {
        public Guid DebtAccountId { get; set; }
        public string DebtName { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public string DebtType { get; set; }
    }

    public class CreditMilestone
    {
        public string MilestoneType { get; set; }
        public string DebtName { get; set; }
        public decimal AmountPaid { get; set; }
        public int ScoreBefore { get; set; }
        public int ScoreAfter { get; set; }
        public int ScoreIncrease { get; set; }
        public DateTime AchievedDate { get; set; }
        public string Message { get; set; }
    }
}
