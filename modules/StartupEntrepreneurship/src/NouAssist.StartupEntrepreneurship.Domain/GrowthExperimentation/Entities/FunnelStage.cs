namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities
{
    /// <summary>
    /// Entity that represents a stage in a conversion funnel
    /// </summary>
    public class FunnelStage : Entity<Guid>
    {
        /// <summary>
        /// The funnel ID this stage belongs to
        /// </summary>
        public Guid FunnelId { get; private set; }
        
        /// <summary>
        /// Stage definition
        /// </summary>
        public StageDefinition Definition { get; private set; }
        
        /// <summary>
        /// Current conversion rate data for this stage
        /// </summary>
        public ConversionRatePerStage ConversionRate { get; private set; }
        
        /// <summary>
        /// Whether this is an entry stage (top of funnel)
        /// </summary>
        public bool IsEntryStage => Definition.Order == 1;
        
        /// <summary>
        /// Whether this is an exit stage (bottom of funnel)
        /// </summary>
        public bool IsExitStage { get; private set; }
        
        protected FunnelStage() { }
        
        internal FunnelStage(
            Guid id,
            Guid funnelId, 
            StageDefinition definition,
            bool isExitStage)
            : base(id)
        {
            FunnelId = funnelId;
            Definition = definition;
            IsExitStage = isExitStage;
        }
        
        internal void UpdateDefinition(StageDefinition definition)
        {
            Definition = definition;
        }
        
        internal void UpdateConversionRate(ConversionRatePerStage conversionRate)
        {
            ConversionRate = conversionRate;
        }
        
        internal void SetAsExitStage(bool isExit)
        {
            IsExitStage = isExit;
        }
    }
}
