using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos
{
    public class IPAssetListDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public IntellectualPropertyType Type { get; set; }
        public string Identifier { get; set; }
        public string Country { get; set; }
        public FilingStatusType Status { get; set; }
        public DateTime FilingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
