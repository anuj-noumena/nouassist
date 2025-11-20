using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Compliance.Dtos
{
    public class ComplianceObligationDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        
        // ObligationDescription value object properties
        public string Overview { get; set; }
        public string Requirements { get; set; }
        public string Notes { get; set; }
        
        // Jurisdiction value object properties
        public string Country { get; set; }
        public string State { get; set; }
        
        // StatusValue value object properties
        public ComplianceStatus Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusNotes { get; set; }
        
        public string AuthorityName { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? Deadline { get; set; }
        public string DocumentUrl { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; }
        
        public bool IsOverdue { get; set; }
    }
