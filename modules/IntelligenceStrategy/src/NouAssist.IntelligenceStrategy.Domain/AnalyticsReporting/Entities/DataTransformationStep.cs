using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// A step in a data transformation pipeline
    /// </summary>
    public class DataTransformationStep : Entity<Guid>
    {
        /// <summary>
        /// The data source this transformation belongs to
        /// </summary>
        public Guid DataSourceId { get; private set; }
        
        /// <summary>
        /// The transformation rule
        /// </summary>
        public TransformationRule Rule { get; private set; }
        
        /// <summary>
        /// Order of execution
        /// </summary>
        public int ExecutionOrder { get; private set; }
        
        /// <summary>
        /// Whether this step is enabled
        /// </summary>
        public bool IsEnabled { get; private set; }
        
        /// <summary>
        /// When this step was last modified
        /// </summary>
        public DateTime LastModified { get; private set; }
        
        protected DataTransformationStep()
        {
        }
        
        internal DataTransformationStep(
            Guid id,
            Guid dataSourceId,
            TransformationRule rule,
            int executionOrder,
            bool isEnabled = true,
            DateTime? lastModified = null)
            : base(id)
        {
            DataSourceId = dataSourceId;
            Rule = rule;
            ExecutionOrder = executionOrder;
            IsEnabled = isEnabled;
            LastModified = lastModified ?? DateTime.UtcNow;
        }
        
        internal void UpdateRule(TransformationRule rule)
        {
            Rule = rule;
            LastModified = DateTime.UtcNow;
        }
        
        internal void SetExecutionOrder(int executionOrder)
        {
            ExecutionOrder = executionOrder;
            LastModified = DateTime.UtcNow;
        }
        
        internal void SetEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
            LastModified = DateTime.UtcNow;
        }
    }
}
