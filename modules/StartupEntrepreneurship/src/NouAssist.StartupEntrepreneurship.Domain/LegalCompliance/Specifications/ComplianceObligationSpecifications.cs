using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Specifications
{
    /// <summary>
    /// Base specification for compliance obligations
    /// </summary>
    public abstract class ComplianceObligationSpecification : Specification<ComplianceObligation>
    {
    }

    /// <summary>
    /// Specification for upcoming compliance obligations
    /// </summary>
    public class UpcomingObligationSpecification : ComplianceObligationSpecification
    {
        private readonly DateTime _referenceDate;
        private readonly int _daysThreshold;

        public UpcomingObligationSpecification(DateTime referenceDate, int daysThreshold = 30)
        {
            _referenceDate = referenceDate;
            _daysThreshold = daysThreshold;
        }

        public override Expression<Func<ComplianceObligation, bool>> ToExpression()
        {
            var thresholdDate = _referenceDate.AddDays(_daysThreshold);
            
            return obligation => 
                obligation.Status.Status != ComplianceStatus.Completed &&
                obligation.Status.Status != ComplianceStatus.Waived &&
                obligation.Status.Status != ComplianceStatus.NotApplicable &&
                obligation.Deadline.HasValue &&
                obligation.Deadline.Value > _referenceDate &&
                obligation.Deadline.Value <= thresholdDate;
        }
    }

    /// <summary>
    /// Specification for overdue compliance obligations
    /// </summary>
    public class OverdueObligationSpecification : ComplianceObligationSpecification
    {
        private readonly DateTime _referenceDate;

        public OverdueObligationSpecification(DateTime referenceDate)
        {
            _referenceDate = referenceDate;
        }

        public override Expression<Func<ComplianceObligation, bool>> ToExpression()
        {
            return obligation => 
                obligation.Status.Status != ComplianceStatus.Completed &&
                obligation.Status.Status != ComplianceStatus.Waived &&
                obligation.Status.Status != ComplianceStatus.NotApplicable &&
                obligation.Deadline.HasValue &&
                obligation.Deadline.Value < _referenceDate;
        }
    }

    /// <summary>
    /// Specification for recurring compliance obligations
    /// </summary>
    public class RecurringObligationSpecification : ComplianceObligationSpecification
    {
        public override Expression<Func<ComplianceObligation, bool>> ToExpression()
        {
            return obligation => 
                obligation.IsRecurring &&
                !string.IsNullOrEmpty(obligation.RecurrencePattern);
        }
    }
}
