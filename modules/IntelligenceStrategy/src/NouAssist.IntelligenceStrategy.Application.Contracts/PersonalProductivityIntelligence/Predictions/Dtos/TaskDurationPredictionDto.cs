using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for task duration predictions
    /// </summary>
    public class TaskDurationPredictionDto : PredictionDto
    {
        /// <summary>
        /// Minimum time estimate in minutes
        /// </summary>
        public int MinMinutes { get; set; }
        
        /// <summary>
        /// Most likely time estimate in minutes
        /// </summary>
        public int MostLikelyMinutes { get; set; }
        
        /// <summary>
        /// Maximum time estimate in minutes
        /// </summary>
        public int MaxMinutes { get; set; }
        
        /// <summary>
        /// Expected time (weighted average) in minutes
        /// </summary>
        public double ExpectedMinutes { get; set; }
        
        /// <summary>
        /// Formatted time estimate
        /// </summary>
        public string FormattedEstimate { get; set; }
    }
}
