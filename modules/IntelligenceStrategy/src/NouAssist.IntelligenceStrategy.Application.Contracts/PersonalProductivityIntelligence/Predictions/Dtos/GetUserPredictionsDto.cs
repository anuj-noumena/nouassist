using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for getting user predictions
    /// </summary>
    public class GetUserPredictionsDto : PagedResultRequestDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Filter by prediction type
        /// </summary>
        public string PredictionType { get; set; }
        
        /// <summary>
        /// Filter by minimum confidence level
        /// </summary>
        public string MinConfidenceLevel { get; set; }
        
        /// <summary>
        /// Filter by specific entity
        /// </summary>
        public string TargetEntityType { get; set; }
        
        /// <summary>
        /// Filter by specific entity ID
        /// </summary>
        public Guid? TargetEntityId { get; set; }
    }
}
