namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Specifications
{
    /// <summary>
    /// Base specification for legal agreements
    /// </summary>
    public abstract class LegalAgreementSpecification : Specification<LegalAgreement>
    {
    }

    /// <summary>
    /// Specification for active legal agreements
    /// </summary>
    public class ActiveAgreementSpecification : LegalAgreementSpecification
    {
        private readonly DateTime _referenceDate;

        public ActiveAgreementSpecification(DateTime referenceDate)
        {
            _referenceDate = referenceDate;
        }

        public override Expression<Func<LegalAgreement, bool>> ToExpression()
        {
            return agreement => 
                (agreement.Status.Status == FilingStatusType.Approved || 
                 agreement.Status.Status == FilingStatusType.Renewed) &&
                agreement.EffectiveDate <= _referenceDate &&
                (!agreement.ExpirationDate.HasValue || 
                 agreement.ExpirationDate.Value > _referenceDate);
        }
    }

    /// <summary>
    /// Specification for agreements approaching expiration
    /// </summary>
    public class ExpiringAgreementSpecification : LegalAgreementSpecification
    {
        private readonly DateTime _referenceDate;
        private readonly int _daysThreshold;

        public ExpiringAgreementSpecification(DateTime referenceDate, int daysThreshold = 30)
        {
            _referenceDate = referenceDate;
            _daysThreshold = daysThreshold;
        }

        public override Expression<Func<LegalAgreement, bool>> ToExpression()
        {
            var thresholdDate = _referenceDate.AddDays(_daysThreshold);
            
            return agreement => 
                (agreement.Status.Status == FilingStatusType.Approved || 
                 agreement.Status.Status == FilingStatusType.Renewed) &&
                agreement.ExpirationDate.HasValue &&
                agreement.ExpirationDate.Value > _referenceDate &&
                agreement.ExpirationDate.Value <= thresholdDate;
        }
    }

    /// <summary>
    /// Specification for renewable agreements
    /// </summary>
    public class RenewableAgreementSpecification : LegalAgreementSpecification
    {
        public override Expression<Func<LegalAgreement, bool>> ToExpression()
        {
            return agreement => 
                (agreement.Status.Status == FilingStatusType.Approved || 
                 agreement.Status.Status == FilingStatusType.Expired) &&
                agreement.ExpirationDate.HasValue;
        }
    }
}
