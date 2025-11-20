using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class UpdateTermSheetStatusDto
    {
        [Required]
        public TermSheetStatus Status { get; set; }
    }
}
