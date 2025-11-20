using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Specifications
{
    /// <summary>
    /// Base specification class for FundraisingRound aggregate
    /// </summary>
    public abstract class FundraisingRoundSpecification : Specification<FundraisingRound>
    {
    }
    
    /// <summary>
    /// Specification that identifies fundraising rounds that are on track
    /// </summary>
    public class OnTrackFundraisingRoundSpecification : FundraisingRoundSpecification
    {
        private readonly double _minimumProgressPercentage;
        
        public OnTrackFundraisingRoundSpecification(double minimumProgressPercentage = 0.5)
        {
            _minimumProgressPercentage = minimumProgressPercentage;
        }
        
        public override Expression<Func<FundraisingRound, bool>> ToExpression()
        {
            return round => 
                round.Stage == RoundStage.Active &&
                round.TargetCloseDate.HasValue &&
                round.TargetCloseDate.Value > DateTime.Now &&
                round.CommittedAmount > 0 &&
                (double)round.CommittedAmount / (double)round.TargetAmount >= _minimumProgressPercentage;
        }
    }
    
    /// <summary>
    /// Specification that identifies fundraising rounds at risk
    /// </summary>
    public class AtRiskFundraisingRoundSpecification : FundraisingRoundSpecification
    {
        private readonly double _minimumProgressPercentage;
        private readonly int _daysToDeadline;
        
        public AtRiskFundraisingRoundSpecification(
            double minimumProgressPercentage = 0.5,
            int daysToDeadline = 30)
        {
            _minimumProgressPercentage = minimumProgressPercentage;
            _daysToDeadline = daysToDeadline;
        }
        
        public override Expression<Func<FundraisingRound, bool>> ToExpression()
        {
            var deadlineApproaching = DateTime.Now.AddDays(_daysToDeadline);
            
            return round => 
                round.Stage == RoundStage.Active &&
                round.TargetCloseDate.HasValue &&
                round.TargetCloseDate.Value <= deadlineApproaching &&
                round.TargetCloseDate.Value > DateTime.Now &&
                (double)round.CommittedAmount / (double)round.TargetAmount < _minimumProgressPercentage;
        }
    }
}
