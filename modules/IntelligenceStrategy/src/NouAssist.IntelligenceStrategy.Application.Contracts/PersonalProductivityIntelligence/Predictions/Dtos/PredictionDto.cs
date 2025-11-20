using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// Base DTO for predictions
    /// </summary>
    public class PredictionDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Type of prediction
        /// </summary>
        public string PredictionType { get; set; }
        
        /// <summary>
        /// Title of the prediction
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Description of the prediction
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Confidence level
        /// </summary>
        public string ConfidenceLevel { get; set; }
        
        /// <summary>
        /// Numeric confidence value (0-1)
        /// </summary>
        public double ConfidenceValue { get; set; }
        
        /// <summary>
        /// When the prediction was made
        /// </summary>
        public DateTime PredictionTime { get; set; }
        
        /// <summary>
        /// Target time for the prediction (if applicable)
        /// </summary>
        public DateTime? TargetTime { get; set; }
        
        /// <summary>
        /// Target entity type
        /// </summary>
        public string TargetEntityType { get; set; }
        
        /// <summary>
        /// Target entity ID
        /// </summary>
        public Guid? TargetEntityId { get; set; }
        
        /// <summary>
        /// Model metadata
        /// </summary>
        public PredictionModelMetadataDto ModelMetadata { get; set; }
        
        /// <summary>
        /// Explainability logs
        /// </summary>
        public List<ExplainabilityLogDto> ExplainabilityLogs { get; set; }
    }
}
