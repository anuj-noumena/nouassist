using System;
using System.Collections.Generic;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for prediction model metadata
    /// </summary>
    public class PredictionModelMetadataDto
    {
        /// <summary>
        /// Name of the model
        /// </summary>
        public string ModelName { get; set; }
        
        /// <summary>
        /// Version of the model
        /// </summary>
        public string ModelVersion { get; set; }
        
        /// <summary>
        /// Features used by the model
        /// </summary>
        public string Features { get; set; }
        
        /// <summary>
        /// Feature importance mapping
        /// </summary>
        public Dictionary<string, double> FeatureImportance { get; set; }
    }
}
