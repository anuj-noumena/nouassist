using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos
{
    public class CreateFundraisingRoundDto
    {
        [Required]
        [StringLength(FundraisingPipelineConsts.FundraisingRound.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(FundraisingPipelineConsts.FundraisingRound.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public RoundType Type { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Target amount must be greater than zero.")]
        public decimal TargetAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? TargetCloseDate { get; set; }
    }
}
