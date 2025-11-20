using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications
{
    /// <summary>
    /// Specifications for SecurityHolder aggregate
    /// </summary>
    public static class SecurityHolderSpecifications
    {
        public static CapTableSpecification<SecurityHolder> Active()
        {
            return new ActiveSecurityHolderSpecification();
        }

        public static CapTableSpecification<SecurityHolder> OfType(HolderType holderType)
        {
            return new SecurityHolderByTypeSpecification(holderType);
        }

        public static CapTableSpecification<SecurityHolder> WithVestingSchedule()
        {
            return new SecurityHolderWithVestingSpecification();
        }

        public static CapTableSpecification<SecurityHolder> FoundersOnly()
        {
            return new FounderSecurityHolderSpecification();
        }

        public static CapTableSpecification<SecurityHolder> InvestorsOnly()
        {
            return new InvestorSecurityHolderSpecification();
        }
    }

    public class ActiveSecurityHolderSpecification : CapTableSpecification<SecurityHolder>
    {
        public override Expression<Func<SecurityHolder, bool>> ToExpression()
        {
            return holder => holder.IsActive;
        }
    }

    public class SecurityHolderByTypeSpecification : CapTableSpecification<SecurityHolder>
    {
        private readonly HolderType _holderType;

        public SecurityHolderByTypeSpecification(HolderType holderType)
        {
            _holderType = holderType;
        }

        public override Expression<Func<SecurityHolder, bool>> ToExpression()
        {
            return holder => holder.HolderType == _holderType;
        }
    }

    public class SecurityHolderWithVestingSpecification : CapTableSpecification<SecurityHolder>
    {
        public override Expression<Func<SecurityHolder, bool>> ToExpression()
        {
            return holder => holder.VestingSchedules.Any();
        }
    }

    public class FounderSecurityHolderSpecification : CapTableSpecification<SecurityHolder>
    {
        public override Expression<Func<SecurityHolder, bool>> ToExpression()
        {
            return holder => holder.HolderType == HolderType.Founder && holder.IsActive;
        }
    }

    public class InvestorSecurityHolderSpecification : CapTableSpecification<SecurityHolder>
    {
        public override Expression<Func<SecurityHolder, bool>> ToExpression()
        {
            return holder => holder.HolderType == HolderType.Investor && holder.IsActive;
        }
    }
}
