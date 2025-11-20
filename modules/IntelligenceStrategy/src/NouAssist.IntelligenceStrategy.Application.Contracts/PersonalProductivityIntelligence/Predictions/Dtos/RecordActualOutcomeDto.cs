using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for recording actual outcome
    /// </summary>
    public class RecordActualOutcomeDto
    {
        /// <summary>
        /// Prediction ID
        /// </summary>
        [Required]
        public Guid PredictionId { get; set; }
        
        /// <summary>
        /// Actual outcome value
        /// </summary>
        [Required]
        public double ActualValue { get; set; }
    }
}
