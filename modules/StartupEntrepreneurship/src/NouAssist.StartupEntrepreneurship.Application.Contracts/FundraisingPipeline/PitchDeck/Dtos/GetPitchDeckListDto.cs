using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class GetPitchDeckListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? FundraisingRoundId { get; set; }
    }
}
