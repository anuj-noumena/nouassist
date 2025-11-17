using System;
using NouAssist.CreditScoreMonitoring.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Entities
{
    public class CreditAlert : CreationAuditedEntity<Guid>
    {
        public Guid CreditReportId { get; private set; }
        public AlertType Type { get; private set; }
        public string Message { get; private set; }
        public string Severity { get; private set; }
        public bool IsRead { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedDate { get; private set; }

        protected CreditAlert() { /* ORM */ }

        public CreditAlert(
            Guid id,
            Guid creditReportId,
            AlertType type,
            string message,
            string severity)
            : base(id)
        {
            CreditReportId = creditReportId;
            Type = type;
            Message = Check.NotNullOrWhiteSpace(message, nameof(message));
            Severity = Check.NotNullOrWhiteSpace(severity, nameof(severity));
            IsRead = false;
            IsResolved = false;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }

        public void Resolve()
        {
            IsResolved = true;
            ResolvedDate = DateTime.UtcNow;
        }
    }
}
