using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class UpdatePitchDeckDto
    {
        [Required]
        [StringLength(FundraisingPipelineConsts.PitchDeck.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(FundraisingPipelineConsts.PitchDeck.MaxDescriptionLength)]
        public string Description { get; set; }

        public Guid? FundraisingRoundId { get; set; }
    }
}
