using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos
{
    public class CreateInteractionDto
    {
        [Required]
        public InteractionType Type { get; set; }

        [Required]
        [StringLength(500)]
        public string Summary { get; set; }

        [Required]
        public DateTime InteractionDate { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }
    }
}
