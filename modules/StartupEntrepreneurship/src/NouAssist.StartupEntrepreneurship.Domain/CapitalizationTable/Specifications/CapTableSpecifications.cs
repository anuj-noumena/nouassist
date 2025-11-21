using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications
{
    /// <summary>
    /// Specifications for CapTable aggregate
    /// </summary>
    public static class CapTableSpecifications
    {
        public static CapTableSpecification<Aggregates.CapTable> Active()
        {
            return new ActiveCapTableSpecification();
        }

        public static CapTableSpecification<Aggregates.CapTable> ForCompany(string companyName)
        {
            return new CompanyCapTableSpecification(companyName);
        }

        public static CapTableSpecification<Aggregates.CapTable> WithTotalSharesGreaterThan(decimal shareCount)
        {
            return new CapTableWithMinimumSharesSpecification(shareCount);
        }
    }

    public class ActiveCapTableSpecification : CapTableSpecification<Aggregates.CapTable>
    {
        public override Expression<Func<Aggregates.CapTable, bool>> ToExpression()
        {
            return capTable => capTable.IsActive;
        }
    }

    public class CompanyCapTableSpecification : CapTableSpecification<Aggregates.CapTable>
    {
        private readonly string _companyName;

        public CompanyCapTableSpecification(string companyName)
        {
            _companyName = companyName;
        }

        public override Expression<Func<Aggregates.CapTable, bool>> ToExpression()
        {
            return capTable => capTable.CompanyName == _companyName;
        }
    }

    public class CapTableWithMinimumSharesSpecification : CapTableSpecification<Aggregates.CapTable>
    {
        private readonly decimal _minimumShares;

        public CapTableWithMinimumSharesSpecification(decimal minimumShares)
        {
            _minimumShares = minimumShares;
        }

        public override Expression<Func<Aggregates.CapTable, bool>> ToExpression()
        {
            return capTable => capTable.TotalAuthorizedShares.Value >= _minimumShares;
        }
    }
}
