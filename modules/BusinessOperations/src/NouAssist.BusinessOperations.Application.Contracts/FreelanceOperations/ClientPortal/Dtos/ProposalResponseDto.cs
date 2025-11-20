using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos
{
    public class ProposalResponseDto
    {
        [Required]
        public bool Accepted { get; set; }
    }
}
