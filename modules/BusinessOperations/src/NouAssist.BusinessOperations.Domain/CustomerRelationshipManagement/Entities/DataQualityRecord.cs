namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities
{
    /// <summary>
    /// Tracks data quality metrics for CRM entities
    /// </summary>
    public class DataQualityRecord : Entity
    {
        /// <summary>
        /// Entity this record belongs to
        /// </summary>
        public Guid EntityId { get; private set; }
        
        /// <summary>
        /// Type of entity
        /// </summary>
        public string EntityType { get; private set; }
        
        /// <summary>
        /// Overall quality score (0-100)
        /// </summary>
        public int QualityScore { get; private set; }
        
        /// <summary>
        /// Last time the record was updated
        /// </summary>
        public DateTime LastUpdated { get; private set; }
        
        /// <summary>
        /// When record completeness was last checked
        /// </summary>
        public DateTime? LastChecked { get; private set; }
        
        /// <summary>
        /// Detailed quality metrics in JSON format
        /// </summary>
        public string QualityMetrics { get; private set; }
        
        /// <summary>
        /// Suggested improvements in JSON format
        /// </summary>
        public string Suggestions { get; private set; }

        protected DataQualityRecord()
        {
        }

        internal DataQualityRecord(
            Guid entityId,
            string entityType,
            int qualityScore,
            DateTime lastUpdated,
            string qualityMetrics,
            string suggestions = null)
        {
            EntityId = entityId;
            EntityType = entityType;
            QualityScore = qualityScore;
            LastUpdated = lastUpdated;
            LastChecked = lastUpdated;
            QualityMetrics = qualityMetrics;
            Suggestions = suggestions;
        }

        internal void Update(
            int qualityScore,
            string qualityMetrics,
            string suggestions = null)
        {
            QualityScore = qualityScore;
            QualityMetrics = qualityMetrics;
            
            if (suggestions != null)
            {
                Suggestions = suggestions;
            }
            
            LastUpdated = Clock.Now;
            LastChecked = Clock.Now;
        }

        internal void UpdateLastChecked()
        {
            LastChecked = Clock.Now;
        }

        public override object[] GetKeys()
        {
            return new object[] { EntityId, EntityType };
        }
    }
}
