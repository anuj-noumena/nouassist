using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Compliance.Dtos
{
    public class GetComplianceObligationListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public ComplianceStatus? Status { get; set; }
        public string AuthorityName { get; set; }
        public string Country { get; set; }
        public bool? IsOverdue { get; set; }
        public bool? IsRecurring { get; set; }
        public bool? HasDeadline { get; set; }
        public bool? IsUpcoming { get; set; }
    }
