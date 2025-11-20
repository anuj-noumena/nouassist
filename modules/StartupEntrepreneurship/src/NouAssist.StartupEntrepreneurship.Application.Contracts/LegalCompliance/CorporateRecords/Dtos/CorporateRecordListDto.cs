using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.CorporateRecords.Dtos
{
    public class CorporateRecordListDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public CorporateRecordType Type { get; set; }
        public DateTime RecordDate { get; set; }
        public FilingStatusType Status { get; set; }
        public string ReferenceNumber { get; set; }
        public bool RequiresFiling { get; set; }
    }
