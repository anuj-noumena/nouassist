using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Specifications
{
    /// <summary>
    /// Base specification class for Prospect aggregate
    /// </summary>
    public abstract class ProspectSpecification : Specification<Prospect>
    {
    }

    /// <summary>
    /// Specification that identifies high-value prospects
    /// </summary>
    public class HighValueProspectSpecification : ProspectSpecification
    {
        private readonly decimal _minimumOpportunityValue;
        
        public HighValueProspectSpecification(decimal minimumOpportunityValue = 1000000)
        {
            _minimumOpportunityValue = minimumOpportunityValue;
        }
        
        public override Expression<Func<Prospect, bool>> ToExpression()
        {
            return prospect => 
                prospect.OpportunityForecast != null &&
                prospect.OpportunityForecast.ExpectedAmount >= _minimumOpportunityValue &&
                prospect.PipelineStatus.Status >= ProspectStatus.Engaged;
        }
    }
    
    /// <summary>
    /// Specification that identifies prospects with high conversion probability
    /// </summary>
    public class HighProbabilityProspectSpecification : ProspectSpecification
    {
        private readonly double _minimumProbability;
        
        public HighProbabilityProspectSpecification(double minimumProbability = 0.7)
        {
            _minimumProbability = minimumProbability;
        }
        
        public override Expression<Func<Prospect, bool>> ToExpression()
        {
            return prospect => 
                prospect.PipelineStatus.ConversionProbability >= _minimumProbability &&
                prospect.PipelineStatus.Status >= ProspectStatus.Pitched &&
                prospect.PipelineStatus.Status < ProspectStatus.Closed;
        }
    }
    
    /// <summary>
    /// Specification that identifies prospects that need follow-up
    /// </summary>
    public class NeedsFollowUpProspectSpecification : ProspectSpecification
    {
        private readonly int _maxDaysSinceLastInteraction;
        
        public NeedsFollowUpProspectSpecification(int maxDaysSinceLastInteraction = 14)
        {
            _maxDaysSinceLastInteraction = maxDaysSinceLastInteraction;
        }
        
        public override Expression<Func<Prospect, bool>> ToExpression()
        {
            var cutoffDate = DateTime.Now.AddDays(-_maxDaysSinceLastInteraction);
            
            return prospect => 
                prospect.PipelineStatus.Status >= ProspectStatus.Contacted &&
                prospect.PipelineStatus.Status < ProspectStatus.Closed &&
                prospect.PipelineStatus.Status != ProspectStatus.Declined &&
                (!prospect.Interactions.Any() || 
                 prospect.Interactions.Max(i => i.InteractionDate) <= cutoffDate);
        }
    }
}
