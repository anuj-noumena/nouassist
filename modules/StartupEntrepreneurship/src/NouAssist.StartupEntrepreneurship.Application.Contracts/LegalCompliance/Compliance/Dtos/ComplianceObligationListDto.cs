using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Compliance.Dtos
{
    public class ComplianceObligationListDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public ComplianceStatus Status { get; set; }
        public string AuthorityName { get; set; }
        public string Country { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsOverdue { get; set; }
    }
