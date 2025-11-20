using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos
{
    public class GetFundraisingRoundListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public RoundType? Type { get; set; }
        public RoundStage? Stage { get; set; }
        public bool? ActiveOnly { get; set; }
        public bool? AtRiskOnly { get; set; }
    }
}
