using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications
{
    /// <summary>
    /// Specifications for SecurityIssuance aggregate
    /// </summary>
    public static class SecurityIssuanceSpecifications
    {
        public static CapTableSpecification<SecurityIssuance> Active()
        {
            return new ActiveSecurityIssuanceSpecification();
        }

        public static CapTableSpecification<SecurityIssuance> OfType(SecurityType securityType)
        {
            return new SecurityIssuanceByTypeSpecification(securityType);
        }

        public static CapTableSpecification<SecurityIssuance> WithAvailableShares()
        {
            return new SecurityIssuanceWithAvailableSharesSpecification();
        }

        public static CapTableSpecification<SecurityIssuance> ExpiredOptions(DateTime currentDate)
        {
            return new ExpiredOptionsSpecification(currentDate);
        }

        public static CapTableSpecification<SecurityIssuance> StockOnly()
        {
            return new StockSecurityIssuanceSpecification();
        }

        public static CapTableSpecification<SecurityIssuance> OptionsOnly()
        {
            return new OptionSecurityIssuanceSpecification();
        }
    }

    public class ActiveSecurityIssuanceSpecification : CapTableSpecification<SecurityIssuance>
    {
        public override Expression<Func<SecurityIssuance, bool>> ToExpression()
        {
            return issuance => issuance.IsActive;
        }
    }

    public class SecurityIssuanceByTypeSpecification : CapTableSpecification<SecurityIssuance>
    {
        private readonly SecurityType _securityType;

        public SecurityIssuanceByTypeSpecification(SecurityType securityType)
        {
            _securityType = securityType;
        }

        public override Expression<Func<SecurityIssuance, bool>> ToExpression()
        {
            return issuance => issuance.SecurityType == _securityType;
        }
    }

    public class SecurityIssuanceWithAvailableSharesSpecification : CapTableSpecification<SecurityIssuance>
    {
        public override Expression<Func<SecurityIssuance, bool>> ToExpression()
        {
            return issuance => issuance.TotalAuthorized.Value > issuance.TotalIssued.Value;
        }
    }

    public class ExpiredOptionsSpecification : CapTableSpecification<SecurityIssuance>
    {
        private readonly DateTime _currentDate;

        public ExpiredOptionsSpecification(DateTime currentDate)
        {
            _currentDate = currentDate;
        }

        public override Expression<Func<SecurityIssuance, bool>> ToExpression()
        {
            return issuance => issuance.SecurityType == SecurityType.StockOption &&
                              issuance.OptionDetails != null &&
                              issuance.OptionDetails.ExpiryDate.HasValue &&
                              issuance.OptionDetails.ExpiryDate.Value < _currentDate;
        }
    }

    public class StockSecurityIssuanceSpecification : CapTableSpecification<SecurityIssuance>
    {
        public override Expression<Func<SecurityIssuance, bool>> ToExpression()
        {
            return issuance => issuance.SecurityType == SecurityType.CommonStock ||
                              issuance.SecurityType == SecurityType.PreferredStock;
        }
    }

    public class OptionSecurityIssuanceSpecification : CapTableSpecification<SecurityIssuance>
    {
        public override Expression<Func<SecurityIssuance, bool>> ToExpression()
        {
            return issuance => issuance.SecurityType == SecurityType.StockOption;
        }
    }
}
