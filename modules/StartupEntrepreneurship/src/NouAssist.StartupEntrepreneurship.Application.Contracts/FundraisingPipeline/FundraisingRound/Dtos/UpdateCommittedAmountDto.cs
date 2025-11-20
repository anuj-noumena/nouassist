using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos
{
    public class UpdateCommittedAmountDto
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Committed amount cannot be negative.")]
        public decimal CommittedAmount { get; set; }
    }
}
