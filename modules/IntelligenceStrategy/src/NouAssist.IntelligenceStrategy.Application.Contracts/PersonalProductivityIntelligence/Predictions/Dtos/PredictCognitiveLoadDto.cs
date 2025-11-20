using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for predicting cognitive load
    /// </summary>
    public class PredictCognitiveLoadDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Target date to predict for
        /// </summary>
        [Required]
        public DateTime TargetDate { get; set; }
    }
}
