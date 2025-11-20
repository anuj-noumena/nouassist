namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Specifications
{
    /// <summary>
    /// Base specification for MarketingChannelPerformance entity
    /// </summary>
    public abstract class MarketingChannelSpecification : Specification<MarketingChannelPerformance>
    {
    }
    
    /// <summary>
    /// Specification for active marketing channels
    /// </summary>
    public class ActiveChannelSpecification : MarketingChannelSpecification
    {
        public override Expression<Func<MarketingChannelPerformance, bool>> ToExpression()
        {
            return c => c.IsActive;
        }
    }
    
    /// <summary>
    /// Specification for channels of a specific type
    /// </summary>
    public class ChannelTypeSpecification : MarketingChannelSpecification
    {
        private readonly MarketingChannelType _channelType;
        
        public ChannelTypeSpecification(MarketingChannelType channelType)
        {
            _channelType = channelType;
        }
        
        public override Expression<Func<MarketingChannelPerformance, bool>> ToExpression()
        {
            return c => c.Type == _channelType;
        }
    }
    
    /// <summary>
    /// Specification for channels with high return on ad spend
    /// </summary>
    public class HighRoasChannelSpecification : MarketingChannelSpecification
    {
        private readonly decimal _minRoasValue;
        
        public HighRoasChannelSpecification(decimal minRoasValue)
        {
            _minRoasValue = minRoasValue;
        }
        
        public override Expression<Func<MarketingChannelPerformance, bool>> ToExpression()
        {
            return c => c.IsActive && 
                       c.ReturnOnAdSpend != null && 
                       c.ReturnOnAdSpend.Value >= _minRoasValue;
        }
    }
    
    /// <summary>
    /// Specification for channels with low cost per acquisition
    /// </summary>
    public class LowCpaChannelSpecification : MarketingChannelSpecification
    {
        private readonly decimal _maxCpaValue;
        
        public LowCpaChannelSpecification(decimal maxCpaValue)
        {
            _maxCpaValue = maxCpaValue;
        }
        
        public override Expression<Func<MarketingChannelPerformance, bool>> ToExpression()
        {
            return c => c.IsActive && 
                       c.CostPerAcquisition != null && 
                       c.CostPerAcquisition.Value <= _maxCpaValue;
        }
    }
}
