using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos
{
    public class GetIPAssetListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public IntellectualPropertyType? Type { get; set; }
        public FilingStatusType? Status { get; set; }
        public string Country { get; set; }
        public bool? HasDeadlines { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsExpiringSoon { get; set; }
    }
