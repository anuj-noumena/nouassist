using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents a work pattern model for a user
    /// </summary>
    public class WorkPatternModel : Entity<Guid>
    {
        /// <summary>
        /// ID of the user productivity profile this model belongs to
        /// </summary>
        public Guid UserProductivityProfileId { get; protected set; }
        
        /// <summary>
        /// Description of the pattern
        /// </summary>
        public string PatternDescription { get; protected set; }
        
        /// <summary>
        /// Confidence in the pattern (0-1)
        /// </summary>
        public double PatternConfidence { get; protected set; }
        
        /// <summary>
        /// Model data (JSON or serialized format)
        /// </summary>
        public string ModelData { get; protected set; }
        
        /// <summary>
        /// When the model was created
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        protected WorkPatternModel()
        {
        }

        /// <summary>
        /// Creates a new work pattern model
        /// </summary>
        public WorkPatternModel(
            Guid id,
            Guid userProductivityProfileId,
            string patternDescription,
            double patternConfidence,
            string modelData,
            DateTime createdAt) : base(id)
        {
            UserProductivityProfileId = userProductivityProfileId;
            PatternDescription = patternDescription;
            PatternConfidence = patternConfidence;
            ModelData = modelData;
            CreatedAt = createdAt;
        }
    }
}
