using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Specifications
{
    /// <summary>
    /// Base specification class for TermSheetOffer aggregate
    /// </summary>
    public abstract class TermSheetSpecification : Specification<TermSheetOffer>
    {
    }
    
    /// <summary>
    /// Specification that identifies favorable term sheets
    /// </summary>
    public class FavorableTermSheetSpecification : TermSheetSpecification
    {
        private readonly decimal _minimumValuation;
        private readonly double _maximumDilution;
        
        public FavorableTermSheetSpecification(
            decimal minimumValuation = 10000000,
            double maximumDilution = 20.0)
        {
            _minimumValuation = minimumValuation;
            _maximumDilution = maximumDilution;
        }
        
        public override Expression<Func<TermSheetOffer, bool>> ToExpression()
        {
            return termSheet => 
                termSheet.Status != TermSheetStatus.Rejected &&
                termSheet.Status != TermSheetStatus.Expired &&
                termSheet.TermData.PreMoneyValuation >= _minimumValuation &&
                termSheet.TermData.EquityPercentage <= (decimal)_maximumDilution &&
                !termSheet.TermData.HasLiquidationPreference;
        }
    }
    
    /// <summary>
    /// Specification that identifies urgent term sheets that need attention
    /// </summary>
    public class UrgentTermSheetSpecification : TermSheetSpecification
    {
        private readonly int _daysToExpiration;
        
        public UrgentTermSheetSpecification(int daysToExpiration = 7)
        {
            _daysToExpiration = daysToExpiration;
        }
        
        public override Expression<Func<TermSheetOffer, bool>> ToExpression()
        {
            var urgentDate = DateTime.Now.AddDays(_daysToExpiration);
            
            return termSheet => 
                (termSheet.Status == TermSheetStatus.Received || 
                 termSheet.Status == TermSheetStatus.UnderReview ||
                 termSheet.Status == TermSheetStatus.InNegotiation) &&
                termSheet.ExpirationDate.HasValue &&
                termSheet.ExpirationDate.Value <= urgentDate;
        }
    }
}
