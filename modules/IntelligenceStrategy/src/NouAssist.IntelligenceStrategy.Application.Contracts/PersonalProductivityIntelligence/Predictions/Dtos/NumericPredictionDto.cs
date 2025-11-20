using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for numeric predictions
    /// </summary>
    public class NumericPredictionDto : PredictionDto
    {
        /// <summary>
        /// Predicted outcome value
        /// </summary>
        public double OutcomeValue { get; set; }
    }
}
