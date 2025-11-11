using System;
using System.Linq.Expressions;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Activity.Specifications
{
    public class AuditLogsByCategorySpecification : Specification<AuditLog>
    {
        private readonly AuditCategory _category;

        public AuditLogsByCategorySpecification(AuditCategory category)
        {
            _category = category;
        }

        public override Expression<Func<AuditLog, bool>> ToExpression() =>
            a => a.Category == _category;
    }

    public class CriticalAuditLogsSpecification : Specification<AuditLog>
    {
        public override Expression<Func<AuditLog, bool>> ToExpression() =>
            a => a.Severity == AuditSeverity.Critical || a.Severity == AuditSeverity.Error;
    }

    public class AuditLogsByDateRangeSpecification : Specification<AuditLog>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public AuditLogsByDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<AuditLog, bool>> ToExpression() =>
            a => a.Timestamp >= _startDate && a.Timestamp < _endDate;
    }
}
