using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos
{
    public class UpdatePipelineStageDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
