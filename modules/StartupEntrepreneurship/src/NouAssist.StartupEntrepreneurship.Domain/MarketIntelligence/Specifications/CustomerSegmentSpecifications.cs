using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Specifications
{
    /// <summary>
    /// Base specification for customer segments
    /// </summary>
    public abstract class CustomerSegmentSpecification : Specification<CustomerSegmentProfile>
    {
    }

    /// <summary>
    /// Specification for active customer segments
    /// </summary>
    public class ActiveCustomerSegmentSpecification : CustomerSegmentSpecification
    {
        public override Expression<Func<CustomerSegmentProfile, bool>> ToExpression()
        {
            return s => s.IsActive;
        }
    }

    /// <summary>
    /// Specification for primary customer segments
    /// </summary>
    public class PrimaryCustomerSegmentSpecification : CustomerSegmentSpecification
    {
        public override Expression<Func<CustomerSegmentProfile, bool>> ToExpression()
        {
            return s => s.Type == SegmentType.Primary;
        }
    }

    /// <summary>
    /// Specification for high-value customer segments
    /// </summary>
    public class HighValueCustomerSegmentSpecification : CustomerSegmentSpecification
    {
        private readonly decimal _minimumValue;

        public HighValueCustomerSegmentSpecification(decimal minimumValue = 1000000m)
        {
            _minimumValue = minimumValue;
        }

        public override Expression<Func<CustomerSegmentProfile, bool>> ToExpression()
        {
            return s => s.EstimatedValue >= _minimumValue;
        }
    }

    /// <summary>
    /// Specification for customer segments with complete profile data
    /// </summary>
    public class CompleteProfileCustomerSegmentSpecification : CustomerSegmentSpecification
    {
        public override Expression<Func<CustomerSegmentProfile, bool>> ToExpression()
        {
            return s => !string.IsNullOrEmpty(s.Demographics.AgeRange) &&
                        !string.IsNullOrEmpty(s.Demographics.Income) &&
                        !string.IsNullOrEmpty(s.Psychographics.Interests) &&
                        !string.IsNullOrEmpty(s.PrimaryNeeds) &&
                        s.Characteristics.Count >= 3;
        }
    }

    /// <summary>
    /// Specification for customer segments by demographic criteria
    /// </summary>
    public class DemographicCustomerSegmentSpecification : CustomerSegmentSpecification
    {
        private readonly string _ageRange;
        private readonly string _income;
        private readonly string _location;

        public DemographicCustomerSegmentSpecification(
            string ageRange = null,
            string income = null,
            string location = null)
        {
            _ageRange = ageRange;
            _income = income;
            _location = location;
        }

        public override Expression<Func<CustomerSegmentProfile, bool>> ToExpression()
        {
            return s => (string.IsNullOrEmpty(_ageRange) || s.Demographics.AgeRange.Contains(_ageRange)) &&
                        (string.IsNullOrEmpty(_income) || s.Demographics.Income.Contains(_income)) &&
                        (string.IsNullOrEmpty(_location) || s.Demographics.Location.Contains(_location));
        }
    }
}
