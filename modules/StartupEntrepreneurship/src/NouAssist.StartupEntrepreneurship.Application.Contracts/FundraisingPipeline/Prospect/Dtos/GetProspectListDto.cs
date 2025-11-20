using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos
{
    public class GetProspectListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public ProspectStatus? Status { get; set; }
        public Guid? FundraisingRoundId { get; set; }
        public bool? HighValueOnly { get; set; }
        public bool? NeedsFollowUpOnly { get; set; }
    }
}
