using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos
{
    public class UpdateRoundStageDto
    {
        [Required]
        public RoundStage Stage { get; set; }
    }
}
