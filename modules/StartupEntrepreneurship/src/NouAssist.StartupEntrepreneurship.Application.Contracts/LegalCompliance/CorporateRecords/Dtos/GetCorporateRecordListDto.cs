using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.CorporateRecords.Dtos
{
    public class GetCorporateRecordListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public CorporateRecordType? Type { get; set; }
        public FilingStatusType? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? RequiresFiling { get; set; }
    }
