namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents the setup of an experiment variant
    /// </summary>
    public class VariantSetup : ValueObject
    {
        /// <summary>
        /// Traffic allocation percentage for this variant (0-1)
        /// </summary>
        public double TrafficAllocation { get; private set; }
        
        /// <summary>
        /// Configuration details for this variant in JSON format
        /// </summary>
        public string Configuration { get; private set; }
        
        /// <summary>
        /// URL for the variant if applicable
        /// </summary>
        public string Url { get; private set; }
        
        protected VariantSetup() { }
        
        public VariantSetup(double trafficAllocation, string configuration, string url = null)
        {
            SetTrafficAllocation(trafficAllocation);
            Configuration = configuration;
            Url = url;
        }
        
        private void SetTrafficAllocation(double trafficAllocation)
        {
            if (trafficAllocation < 0 || trafficAllocation > 1)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", trafficAllocation)
                    .WithData("property", "TrafficAllocation");
            }
            
            TrafficAllocation = trafficAllocation;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TrafficAllocation;
            
            if (!string.IsNullOrEmpty(Configuration))
            {
                yield return Configuration;
            }
            
            if (!string.IsNullOrEmpty(Url))
            {
                yield return Url;
            }
        }
    }
}
