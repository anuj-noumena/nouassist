using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class GetLegalAgreementListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public LegalAgreementType? Type { get; set; }
        public FilingStatusType? Status { get; set; }
        public string CounterpartyName { get; set; }
        public string Country { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsExpiringSoon { get; set; }
    }
