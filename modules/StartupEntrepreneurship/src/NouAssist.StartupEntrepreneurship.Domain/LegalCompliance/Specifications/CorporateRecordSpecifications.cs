namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Specifications
{
    /// <summary>
    /// Base specification for corporate records
    /// </summary>
    public abstract class CorporateRecordSpecification : Specification<CorporateRecord>
    {
    }

    /// <summary>
    /// Specification for corporate records by type and date range
    /// </summary>
    public class CorporateRecordByTypeAndDateSpecification : CorporateRecordSpecification
    {
        private readonly CorporateRecordType _type;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public CorporateRecordByTypeAndDateSpecification(
            CorporateRecordType type,
            DateTime startDate,
            DateTime endDate)
        {
            _type = type;
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<CorporateRecord, bool>> ToExpression()
        {
            return record => 
                record.Type == _type &&
                record.RecordDate >= _startDate &&
                record.RecordDate <= _endDate;
        }
    }

    /// <summary>
    /// Specification for corporate records requiring filing
    /// </summary>
    public class RequiresFilingRecordSpecification : CorporateRecordSpecification
    {
        public override Expression<Func<CorporateRecord, bool>> ToExpression()
        {
            return record => 
                record.RecordType.RequiresFiling &&
                (record.Status.Status == FilingStatusType.Draft || 
                 record.Status.Status == FilingStatusType.Pending);
        }
    }
}
