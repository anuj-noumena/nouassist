using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for risk predictions
    /// </summary>
    public class RiskPredictionDto : PredictionDto
    {
        /// <summary>
        /// Risk probability value (0-1)
        /// </summary>
        public double RiskValue { get; set; }
        
        /// <summary>
        /// Risk level as a readable category
        /// </summary>
        public string RiskLevel { get; set; }
        
        /// <summary>
        /// Formatted risk percentage
        /// </summary>
        public string FormattedRiskPercentage { get; set; }
    }
}
