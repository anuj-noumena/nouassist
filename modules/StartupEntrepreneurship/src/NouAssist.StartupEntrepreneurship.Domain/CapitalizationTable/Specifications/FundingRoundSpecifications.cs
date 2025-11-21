using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications
{
    /// <summary>
    /// Specifications for FundingRound aggregate
    /// </summary>
    public static class FundingRoundSpecifications
    {
        public static CapTableSpecification<FundingRound> Active()
        {
            return new ActiveFundingRoundSpecification();
        }

        public static CapTableSpecification<FundingRound> Closed()
        {
            return new ClosedFundingRoundSpecification();
        }

        public static CapTableSpecification<FundingRound> InProgress()
        {
            return new FundingRoundInProgressSpecification();
        }

        public static CapTableSpecification<FundingRound> WithMinimumRaise(decimal minimumAmount)
        {
            return new FundingRoundWithMinimumRaiseSpecification(minimumAmount);
        }

        public static CapTableSpecification<FundingRound> CompletedAfter(DateTime date)
        {
            return new FundingRoundCompletedAfterSpecification(date);
        }
    }

    public class ActiveFundingRoundSpecification : CapTableSpecification<FundingRound>
    {
        public override Expression<Func<FundingRound, bool>> ToExpression()
        {
            return round => round.Status == FundingRoundStatus.Active;
        }
    }

    public class ClosedFundingRoundSpecification : CapTableSpecification<FundingRound>
    {
        public override Expression<Func<FundingRound, bool>> ToExpression()
        {
            return round => round.Status == FundingRoundStatus.Closed;
        }
    }

    public class FundingRoundInProgressSpecification : CapTableSpecification<FundingRound>
    {
        public override Expression<Func<FundingRound, bool>> ToExpression()
        {
            return round => round.Status == FundingRoundStatus.Active ||
                           round.Status == FundingRoundStatus.Planning;
        }
    }

    public class FundingRoundWithMinimumRaiseSpecification : CapTableSpecification<FundingRound>
    {
        private readonly decimal _minimumAmount;

        public FundingRoundWithMinimumRaiseSpecification(decimal minimumAmount)
        {
            _minimumAmount = minimumAmount;
        }

        public override Expression<Func<FundingRound, bool>> ToExpression()
        {
            return round => round.RaisedAmount.Value >= _minimumAmount;
        }
    }

    public class FundingRoundCompletedAfterSpecification : CapTableSpecification<FundingRound>
    {
        private readonly DateTime _date;

        public FundingRoundCompletedAfterSpecification(DateTime date)
        {
            _date = date;
        }

        public override Expression<Func<FundingRound, bool>> ToExpression()
        {
            return round => round.CloseDate.HasValue && round.CloseDate.Value > _date;
        }
    }
}
