using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.CorporateRecords.Dtos
{
    public class CorporateRecordDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public CorporateRecordType Type { get; set; }
        
        // RecordType value object properties
        public string RecordTypeDescription { get; set; }
        public bool RequiresSignature { get; set; }
        public bool RequiresNotarization { get; set; }
        public bool RequiresFiling { get; set; }
        
        public DateTime RecordDate { get; set; }
        public string DocumentUrl { get; set; }
        
        // FilingStatus value object properties
        public FilingStatusType Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusNotes { get; set; }
        
        public string ReferenceNumber { get; set; }
    }
