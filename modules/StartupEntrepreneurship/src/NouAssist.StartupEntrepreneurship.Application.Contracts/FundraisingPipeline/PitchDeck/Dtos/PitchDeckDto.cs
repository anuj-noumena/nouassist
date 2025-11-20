using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class PitchDeckDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? FundraisingRoundId { get; set; }
        public string FundraisingRoundName { get; set; }
        public List<PitchVersionDto> Versions { get; set; }
        public int PresentationCount { get; set; }
        public int FeedbackCount { get; set; }
        public double AverageRating { get; set; }
    }
}
