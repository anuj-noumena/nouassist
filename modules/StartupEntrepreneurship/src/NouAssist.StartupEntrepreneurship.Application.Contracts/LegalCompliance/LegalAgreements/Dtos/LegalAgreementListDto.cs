using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class LegalAgreementListDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public LegalAgreementType Type { get; set; }
        public FilingStatusType Status { get; set; }
        public string CounterpartyName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal? Value { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
    }
