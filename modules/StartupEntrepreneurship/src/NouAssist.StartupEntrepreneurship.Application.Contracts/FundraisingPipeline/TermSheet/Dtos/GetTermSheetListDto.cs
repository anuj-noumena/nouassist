using System;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class GetTermSheetListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? ProspectId { get; set; }
        public Guid? FundraisingRoundId { get; set; }
        public TermSheetStatus? Status { get; set; }
        public bool? UrgentOnly { get; set; }
        public bool? FavorableOnly { get; set; }
    }
}
