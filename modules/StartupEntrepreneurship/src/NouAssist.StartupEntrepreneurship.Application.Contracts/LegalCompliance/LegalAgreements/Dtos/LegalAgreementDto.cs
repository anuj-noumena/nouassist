using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class LegalAgreementDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public LegalAgreementType Type { get; set; }
        
        // FilingStatus value object properties
        public FilingStatusType Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusNotes { get; set; }
        
        public string DocumentUrl { get; set; }
        public string CounterpartyName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ReferenceNumber { get; set; }
        
        // Jurisdiction value object properties
        public string Country { get; set; }
        public string State { get; set; }
        
        public decimal? Value { get; set; }
        public string Currency { get; set; }
        
        public List<AgreementTermDto> Terms { get; set; } = new List<AgreementTermDto>();
        public List<AgreementReminderDto> Reminders { get; set; } = new List<AgreementReminderDto>();
        
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
    }
