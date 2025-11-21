using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Specifications
{
    /// <summary>
    /// Base specification for ConversionFunnelDefinition entity
    /// </summary>
    public abstract class ConversionFunnelSpecification : Specification<ConversionFunnelDefinition>
    {
    }
    
    /// <summary>
    /// Specification for active funnels
    /// </summary>
    public class ActiveFunnelSpecification : ConversionFunnelSpecification
    {
        public override Expression<Func<ConversionFunnelDefinition, bool>> ToExpression()
        {
            return f => f.IsActive;
        }
    }
    
    /// <summary>
    /// Specification for funnels with high overall conversion rate
    /// </summary>
    public class HighConversionFunnelSpecification : ConversionFunnelSpecification
    {
        private readonly double _minConversionRate;
        
        public HighConversionFunnelSpecification(double minConversionRate)
        {
            _minConversionRate = minConversionRate;
        }
        
        public override Expression<Func<ConversionFunnelDefinition, bool>> ToExpression()
        {
            return f => f.IsActive && f.OverallConversionRate >= _minConversionRate;
        }
    }
}
