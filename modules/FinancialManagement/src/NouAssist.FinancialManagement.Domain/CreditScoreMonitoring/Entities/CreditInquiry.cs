using System;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Entities
{
    public class CreditInquiry : Entity<Guid>
    {
        public Guid CreditReportId { get; private set; }
        public InquiryType Type { get; private set; }
        public string InquirerName { get; private set; }
        public DateTime InquiryDate { get; private set; }
        public string? Purpose { get; private set; }

        protected CreditInquiry() { /* ORM */ }

        public CreditInquiry(
            Guid id,
            Guid creditReportId,
            InquiryType type,
            string inquirerName,
            DateTime inquiryDate,
            string? purpose = null)
            : base(id)
        {
            CreditReportId = creditReportId;
            Type = type;
            InquirerName = Check.NotNullOrWhiteSpace(inquirerName, nameof(inquirerName));
            InquiryDate = inquiryDate;
            Purpose = purpose;
        }

        public bool IsRecent(int daysThreshold = 30)
        {
            return (DateTime.UtcNow - InquiryDate).Days <= daysThreshold;
        }
    }
}
