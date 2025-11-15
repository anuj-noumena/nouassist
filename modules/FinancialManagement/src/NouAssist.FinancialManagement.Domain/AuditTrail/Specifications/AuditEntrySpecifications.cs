using System;
using System.Linq.Expressions;
using NouAssist.AuditTrail.Aggregates;
using NouAssist.AuditTrail.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.AuditTrail.Specifications
{
    public class AuditEntriesByEntitySpecification : Specification<AuditEntry>
    {
        private readonly string _entityType;
        private readonly string _entityId;

        public AuditEntriesByEntitySpecification(string entityType, string entityId = null)
        {
            _entityType = entityType;
            _entityId = entityId;
        }

        public override Expression<Func<AuditEntry, bool>> ToExpression()
        {
            if (string.IsNullOrEmpty(_entityId))
            {
                return entry => entry.EntityReference.EntityType == _entityType;
            }

            return entry => entry.EntityReference.EntityType == _entityType &&
                          entry.EntityReference.EntityId == _entityId;
        }
    }

    public class AuditEntriesByComplianceFlagSpecification : Specification<AuditEntry>
    {
        private readonly ComplianceFlag _flag;

        public AuditEntriesByComplianceFlagSpecification(ComplianceFlag flag)
        {
            _flag = flag;
        }

        public override Expression<Func<AuditEntry, bool>> ToExpression()
        {
            return entry => (entry.ComplianceFlags & _flag) == _flag;
        }
    }

    public class AuditEntriesBySeveritySpecification : Specification<AuditEntry>
    {
        private readonly AuditSeverity _minimumSeverity;

        public AuditEntriesBySeveritySpecification(AuditSeverity minimumSeverity)
        {
            _minimumSeverity = minimumSeverity;
        }

        public override Expression<Func<AuditEntry, bool>> ToExpression()
        {
            return entry => entry.Severity >= _minimumSeverity;
        }
    }

    public class ArchivedAuditEntriesSpecification : Specification<AuditEntry>
    {
        private readonly bool _includeArchived;

        public ArchivedAuditEntriesSpecification(bool includeArchived)
        {
            _includeArchived = includeArchived;
        }

        public override Expression<Func<AuditEntry, bool>> ToExpression()
        {
            return _includeArchived 
                ? entry => true 
                : entry => !entry.IsArchived;
        }
    }
}
