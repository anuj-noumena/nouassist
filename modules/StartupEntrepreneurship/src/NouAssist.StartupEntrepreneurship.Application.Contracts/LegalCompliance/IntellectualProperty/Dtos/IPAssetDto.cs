using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos
{
    public class IPAssetDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IntellectualPropertyType Type { get; set; }
        
        // IPIdentifier value object properties
        public string Identifier { get; set; }
        public string IssuingAuthority { get; set; }
        
        // Jurisdiction value object properties
        public string Country { get; set; }
        public string State { get; set; }
        
        // FilingStatus value object properties
        public FilingStatusType Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusNotes { get; set; }
        
        public DateTime FilingDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        
        // IPValuation properties (optional)
        public decimal? ValuationAmount { get; set; }
        public string ValuationCurrency { get; set; }
        public DateTime? ValuationDate { get; set; }
        public string ValuationMethod { get; set; }
        
        public List<IPDeadlineDto> FilingDeadlines { get; set; } = new List<IPDeadlineDto>();
    }
